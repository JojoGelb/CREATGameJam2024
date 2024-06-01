using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject settingsWindow;

    public void Resume()
    {
        pauseMenu.SetActive(false);
        GameManager.Instance.ChangeState(GameState.Pause);
    }

    public void Settings()
    {
        settingsWindow.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void CloseSettingsWindow()
    {
        settingsWindow.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
