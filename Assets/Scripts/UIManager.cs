using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private VisualElement TextureEditorUI;

    private TextureEditor textureEditor;


    void Awake()
    {
        TextureEditorUI = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("TextureEditorContainer");
        textureEditor = GetComponent<TextureEditor>();
        TextureEditorUI.style.display = DisplayStyle.None;
        textureEditor.enabled = false;
    }

    //TODO: Disable Player InputMap when in any UI mode and re-enable when exiting UI mode
    
    public void StartTextureEditorForObject(GameObject obj)
    {
        textureEditor.enabled = true;
        textureEditor.InitializeForObject(obj);
        TextureEditorUI.style.display = DisplayStyle.Flex;
    }
}
