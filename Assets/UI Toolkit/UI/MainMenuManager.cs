using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    
    private Button _playButton;
    private Button _settingsButton;
    private Button _creditsButton;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        if (_uiDocument == null)
        {
            Debug.LogError("No UIDocument found on MainMenuManager.");
        }
        
        _playButton = _uiDocument.rootVisualElement.Q<Button>("Play_Button");
        
    }

}