using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string levelToLoad;
    public GameObject settingsWindow;
    public GameObject creditsWindow;

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
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
