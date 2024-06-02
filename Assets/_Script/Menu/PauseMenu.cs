using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static GameManager;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject settingsWindow;

    private void Start()
    {
        InputManager.Instance?.RegisterToEscape(OnPausePressed);
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        if(GameManager.Instance.state == GameState.Pause)
        {
            if (settingsWindow.activeSelf)
            {
                CloseSettingsWindow();
                return;
            }

            if (pauseMenu.activeSelf)
            {
                Resume();
            }
            else
            {
                Debug.LogError("Wtf: escape was pressed, game was in pause but pauseMenu wasn't open, setting neither");
            }
        }else
        {
            pauseMenu.SetActive(true);
            GameManager.Instance.ChangeState(GameState.Pause);
        }


    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        GameManager.Instance.ResetToPrecedentState(); //ChangeState(GameState.Playing);
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
