using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public string levelToLoad;
    public GameObject settingsWindow;
    public GameObject creditsWindow;

    public GameObject mainWindow;

    public Button FirstSelectedMainMenu;
    public Dropdown FirstSelectedSettings;
    public Button FirstSelectedCredits;

    private void Start()
    {
        InputManager.Instance.RegisterToBEvent(OnBpressed);
    }

    private void OnDisable()
    {
        InputManager.Instance?.UnRegisterToBEvent(OnBpressed);
    }

    private void OnBpressed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (settingsWindow.activeSelf == true)
        {
            CloseSettingsWindow();
        }

        if (creditsWindow.activeSelf == true)
        {
            CloseCredits();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("Tuto");
    }

    public void Settings()
    {
        settingsWindow.SetActive(true);
        FirstSelectedSettings.Select();
        mainWindow.SetActive(false);
    }

    public void Credits()
    {
        creditsWindow.SetActive(true);
        mainWindow.SetActive(false);
        FirstSelectedCredits.Select();
    }

    public void CloseCredits()
    {
        creditsWindow.SetActive(false);
        mainWindow.SetActive(true);
        FirstSelectedMainMenu.Select();
    }

    public void CloseSettingsWindow()
    {
        settingsWindow.SetActive(false);
        FirstSelectedMainMenu.Select();
        mainWindow.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
