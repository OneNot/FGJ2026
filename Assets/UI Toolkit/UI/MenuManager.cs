using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    
    VisualElement root;
    VisualElement mainpage,settingspage,creditspage;
    private Button _playButton;
    private Button _settingsButton;
    private Button _creditsButton;
    
    private Button _exitButton;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        if (_uiDocument == null)
        {
            Debug.LogError("No UIDocument found on MainMenuManager.");
        }
        root = _uiDocument.rootVisualElement;
        mainpage = _uiDocument.rootVisualElement.Q<VisualElement>("MainMenuContainer");
        settingspage = _uiDocument.rootVisualElement.Q<VisualElement>("SettingsMenu");
        creditspage = _uiDocument.rootVisualElement.Q<VisualElement>("CreditsMenu");

        root.style.display = DisplayStyle.Flex;
        mainpage.style.display=DisplayStyle.Flex;

        settingspage.style.display=DisplayStyle.None;
        creditspage.style.display=DisplayStyle.None;

        _creditsButton = _uiDocument.rootVisualElement.Q<Button>("CreditsButton");
        _creditsButton.clicked+=loadCredits;

        _settingsButton = _uiDocument.rootVisualElement.Q<Button>("OptionsButton");
        _settingsButton.clicked+=loadSettings;

        _playButton = _uiDocument.rootVisualElement.Q<Button>("StartGameButton");
        _playButton.clicked +=loadLevel;
    }


    private void loadCredits()
    {
        Debug.Log("Credits clicked");
        mainpage.style.display=DisplayStyle.None;
        settingspage.style.display=DisplayStyle.None;
        creditspage.style.display=DisplayStyle.Flex;
        
    }

    private void loadSettings()
    {
        
        Debug.Log("Settings clicked");
        mainpage.style.display=DisplayStyle.None;
        settingspage.style.display=DisplayStyle.Flex;
        creditspage.style.display=DisplayStyle.None;
    }

    private void loadLevel()
    {
        Destroy(gameObject);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        Time.timeScale = 1f;
        Debug.Log("Start Game clicked");
        SceneManager.LoadScene("Level-one-two",LoadSceneMode.Additive);
    
    }


}

