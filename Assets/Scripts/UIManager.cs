using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private VisualElement mainUIRoot, textureEditorUI, winScreenUI, pickupUI;
    
    private Label gemsLabel;
    private int totalGemCount = 0;

    private TextureEditor textureEditor;


    void Awake()
    {
        mainUIRoot = GetComponent<UIDocument>().rootVisualElement;
        textureEditorUI = mainUIRoot.Q<VisualElement>("TextureEditorContainer");
        textureEditor = GetComponent<TextureEditor>();
        textureEditorUI.style.display = DisplayStyle.None;
        textureEditor.enabled = false;
        winScreenUI = mainUIRoot.Q<VisualElement>("WinScreenContainer");
        winScreenUI.style.display = DisplayStyle.None;
        pickupUI = mainUIRoot.Q<VisualElement>("GemCollectionContainer");
        pickupUI.style.display = DisplayStyle.Flex;
        gemsLabel = mainUIRoot.Q<Label>("GemScoreLabel");
    }

    void Start()
    {
        totalGemCount = GameObject.FindGameObjectsWithTag("Pickup").Length;
        gemsLabel.text = $"0 / {totalGemCount}";
    }

    //TODO: Disable Player InputMap when in any UI mode and re-enable when exiting UI mode
    
    public void StartTextureEditorForObject(GameObject obj)
    {
        textureEditor.enabled = true;
        textureEditor.InitializeForObject(obj);
        textureEditorUI.style.display = DisplayStyle.Flex;
    }

    public void SetGemCount(int count)
    {
        gemsLabel.text = $"{count} / {totalGemCount}";
    }
    public void AddGem()
    {
        int currentCount = int.Parse(gemsLabel.text.Split('/')[0].Trim());
        currentCount++;
        gemsLabel.text = $"{currentCount} / {totalGemCount}";
    }

    public void ShowWinScreen()
    {
        winScreenUI.style.display = DisplayStyle.Flex;
        pickupUI.style.display = DisplayStyle.None;
    }
}
