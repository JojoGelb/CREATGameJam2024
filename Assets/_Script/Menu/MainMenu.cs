using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public string levelToLoad;
    public GameObject settingsWindow;
    public GameObject creditsWindow;

    public Button FirstSelectedMainMenu;
    public Dropdown FirstSelectedSettings;

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
    }

    public void Credits()
    {
        creditsWindow.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsWindow.SetActive(false);
    }

    public void CloseSettingsWindow()
    {
        settingsWindow.SetActive(false);
        FirstSelectedMainMenu.Select();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
