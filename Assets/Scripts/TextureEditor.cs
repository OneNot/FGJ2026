using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TextureEditor : MonoBehaviour
{
    private Image imageElement;
    private Button applyButtonElement, exitButtonElement;

    private InputAction drawPointerAction, drawAction;

    private Texture2D editableTexture;

    private bool isInitialized = false;

    [SerializeField]
    private int brushSize = 20;

    void OnDisable()
    {
        isInitialized = false;
        imageElement.image = null;
    }

    //Using FixedUpdate for now, just to avoid doing draw logic every frame (replace with something better later)
    void FixedUpdate()
    {
        if(isInitialized)
        {
            RunDrawLogic();
        }
    }

    private void RunDrawLogic() {
        Vector2 pointerPosition = drawPointerAction.ReadValue<Vector2>();
        Vector2 uvPos = GetUVFromScreenPosition(pointerPosition);
        Vector2 uvPosClamped = new Vector2(
            Mathf.Clamp01(uvPos.x),
            Mathf.Clamp01(uvPos.y)
        );
        Vector2 pixelPos = new Vector2(
            uvPosClamped.x * editableTexture.width,
            uvPosClamped.y * editableTexture.height
        );
        
        if(drawAction.IsPressed() && Vector2.Distance(uvPos, Vector2.zero) >= 0 && Vector2.Distance(uvPos, Vector2.one) >= 0)
        {
            Debug.Log($"Drawing at UV: {uvPosClamped}, Pixel: {pixelPos}, Texture size: {editableTexture.width}x{editableTexture.height}");

            int px = (int)pixelPos.x;
            int py = (int)pixelPos.y;
            for(int x = px - brushSize/2; x <= px + brushSize/2; x++)
            {
                for(int y = py - brushSize/2; y <= py + brushSize/2; y++)
                {
                    if(x >= 0 && x < editableTexture.width && y >= 0 && y < editableTexture.height)
                    {
                        editableTexture.SetPixel(x, y, Color.black);
                    }
                }
            }

            editableTexture.Apply();
            imageElement.image = editableTexture; // Force UI refresh
        }
    }


    private Vector2 GetUVFromScreenPosition(Vector2 screenPosition)
    {
        // Convert screen position to panel space (UI Toolkit coordinates)
        Vector2 panelPosition = RuntimePanelUtils.ScreenToPanel(
            imageElement.panel,
            new Vector2(screenPosition.x, screenPosition.y)
        );
        
        // Get the element's world bound (position and size in panel space)
        Rect worldBound = imageElement.worldBound;
        
        // Convert to local coordinates within the element
        Vector2 localPosition = panelPosition - new Vector2(worldBound.x, worldBound.y);
        
        // Normalize to UV space (0-1)
        Vector2 uv = new Vector2(
            localPosition.x / worldBound.width,
            localPosition.y / worldBound.height
        );
        
        return uv;
    }


    public void InitializeWithTexture(Texture2D newTexture)
    {
        Debug.Log("TextureEditor initializing with texture: " + newTexture.name);
        drawPointerAction = InputSystem.actions.FindAction("Point");
        drawAction = InputSystem.actions.FindAction("Click");

        UIDocument uiDocument = GetComponent<UIDocument>();
        imageElement = uiDocument.rootVisualElement.Q<Image>("TextureElement");
        applyButtonElement = uiDocument.rootVisualElement.Q<Button>("SaveButton");
        exitButtonElement = uiDocument.rootVisualElement.Q<Button>("ExitButton");
        applyButtonElement.clicked += () => 
        {
            Debug.Log("Apply button clicked - implement saving logic here.");
        };
        exitButtonElement.clicked += () => 
        {
            uiDocument.rootVisualElement.style.display = DisplayStyle.None;
            this.enabled = false;
        };

        if(newTexture != null && imageElement != null)
        {
            editableTexture = new Texture2D(
                newTexture.width, 
                newTexture.height, 
                newTexture.format, 
                false
            );

            editableTexture.SetPixels(newTexture.GetPixels());
            editableTexture.Apply();

            imageElement.image = editableTexture;

            isInitialized = true;
        }
        else
        {
            Debug.LogError("TextureEditor initialization failed: newTexture or imageElement is null.");
        }
    }
}
