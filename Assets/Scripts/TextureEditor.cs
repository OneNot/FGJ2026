using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

// Simple texture-painting editor for a UI Toolkit `Image` that allows
// painting into a duplicated `Texture2D` and saving the result back to
// the target object's material (`_OpacityMask`).
public class TextureEditor : MonoBehaviour
{
    // UI Toolkit elements
    private Image imageElement; // displays the editable texture in the UI
    private Button applyButtonElement, exitButtonElement; // UI buttons
    private Action onApplyClicked, onExitClicked; // stored delegates so we can unsubscribe

    // Input actions from the Input System
    private InputAction drawPointerAction, drawWhiteAction, drawBlackAction;

    // The working copy of the texture that we paint into
    private Texture2D editableTexture;

    // Whether the editor has been successfully initialized
    private bool isInitialized = false;

    [SerializeField]
    // Brush size in pixels (square brush)
    private int brushSize = 20;

    void OnDisable()
    {
        // Clean up state and unsubscribe UI handlers to avoid duplicate
        // subscriptions and potential memory leaks when the component is
        // disabled or re-enabled.
        isInitialized = false;
        if (applyButtonElement != null && onApplyClicked != null)
            applyButtonElement.clicked -= onApplyClicked;
        if (exitButtonElement != null && onExitClicked != null)
            exitButtonElement.clicked -= onExitClicked;
        if (imageElement != null)
            imageElement.image = null;
    }

    void Update()
    {
        if(isInitialized)
        {
            RunDrawLogic();
        }
    }

    // Handle pointer input and paint into the `editableTexture`.
    // Reads the pointer from the Input System, converts to UV space
    // relative to the UI `Image`, and paints a square brush of `brushSize`.
    private void RunDrawLogic() {
        //only run draw logic if the draw action is pressed
        if(drawWhiteAction.IsPressed() || drawBlackAction.IsPressed()) {
            Vector2 pointerPosition = drawPointerAction.ReadValue<Vector2>();
            Vector2 uvPos = GetUVFromScreenPosition(pointerPosition);
            Vector2 uvPosClamped = new Vector2(
                Mathf.Clamp01(uvPos.x),
                Mathf.Clamp01(uvPos.y)
            );

            // Map UV to pixel coordinates on the editable texture
            Vector2 pixelPos = new Vector2(
                uvPosClamped.x * editableTexture.width,
                uvPosClamped.y * editableTexture.height
            );
            
            // Only draw if UV cordinates are within valid range
            if(Vector2.Distance(uvPos, Vector2.zero) >= 0 && Vector2.Distance(uvPos, Vector2.one) >= 0)
            {
                //Debug.Log($"Drawing at UV: {uvPosClamped}, Pixel: {pixelPos}, Texture size: {editableTexture.width}x{editableTexture.height}");

                int px = (int)pixelPos.x;
                int py = (int)pixelPos.y;
                // Simple square brush — iterate over pixels and set color.
                for(int x = px - brushSize/2; x <= px + brushSize/2; x++)
                {
                    for(int y = py - brushSize/2; y <= py + brushSize/2; y++)
                    {
                        if(x >= 0 && x < editableTexture.width && y >= 0 && y < editableTexture.height)
                        {
                            editableTexture.SetPixel(x, y, drawWhiteAction.IsPressed() ? Color.white : Color.black);
                        }
                    }
                }

                // Apply changes to the texture and refresh the UI image
                editableTexture.Apply();
                imageElement.image = editableTexture; // Force UI refresh
            }
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


    /// <summary>
    /// Prepare the editor for a specific target object. This duplicates the
    /// target's `_OpacityMask` texture into a writable `Texture2D`, binds UI
    /// elements and input actions, and wires Save/Exit button handlers.
    /// </summary>
    public void InitializeForObject(GameObject obj)
    {
        Material[] materials = obj.GetComponent<Renderer>().materials;
        Texture2D newTexture = null;
        Material mat = null;
        foreach (Material m in materials)
        {
            newTexture = m.GetTexture("_OpacityMask") as Texture2D; 
            if (newTexture != null)
            {
                mat = m;
                break;
            }
        }

        Debug.Log("TextureEditor initializing with texture: " + newTexture.name);

        // Cache input actions
        drawPointerAction = InputSystem.actions.FindAction("Point");
        drawWhiteAction = InputSystem.actions.FindAction("Click");
        drawBlackAction = InputSystem.actions.FindAction("RightClick");

        // Find UI elements in the UIDocument
        VisualElement TextureEditorUI = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("TextureEditorContainer");
        imageElement = TextureEditorUI.Q<Image>("TextureElement");

        applyButtonElement = TextureEditorUI.Q<Button>("SaveButton");
        exitButtonElement = TextureEditorUI.Q<Button>("ExitButton");
        
        // Ensure we don't double-subscribe: remove previous handlers if present,
        // then store the delegate and subscribe the fresh handler.
        if (applyButtonElement != null && onApplyClicked != null)
            applyButtonElement.clicked -= onApplyClicked;
        // OnApply: set the target object's material texture to the edited version and disable the editor
        onApplyClicked = () => {
            mat.SetTexture("_OpacityMask", editableTexture);
            TextureEditorUI.style.display = DisplayStyle.None;
            this.enabled = false;
        };
        applyButtonElement.clicked += onApplyClicked;

        if (exitButtonElement != null && onExitClicked != null)
            exitButtonElement.clicked -= onExitClicked;
        // OnExit: simply disable the editor without saving changes
        onExitClicked = () => {
            TextureEditorUI.style.display = DisplayStyle.None;
            this.enabled = false;
        };
        exitButtonElement.clicked += onExitClicked;

        // Create a writable copy of the source texture for editing in memory
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

            // Show the working copy in the UI
            imageElement.image = editableTexture;

            isInitialized = true;
        }
        else
        {
            Debug.LogError("TextureEditor initialization failed: newTexture or imageElement is null.");
        }
    }
}
