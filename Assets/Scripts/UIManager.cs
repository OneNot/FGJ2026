using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private UIDocument uiDocument;

    private TextureEditor textureEditor;


    void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        textureEditor = GetComponent<TextureEditor>();
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        textureEditor.enabled = false;
    }

    //TODO: Disable Player InputMap when in any UI mode and re-enable when exiting UI mode

    public void StartTextureEditorWithTexture(Texture2D texture)
    {
        Debug.Log("UIManager starting Texture Editor (" + textureEditor.name + ") with texture: " + texture.name);
        textureEditor.enabled = true;
        textureEditor.InitializeWithTexture(texture);
        uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
    }
}
