using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class EndGameMenu : Singleton<EndGameMenu>
{
    public GameObject ParentRectTransform;
    public TextMeshProUGUI TextTitle;
    public TextMeshProUGUI TextScore;
    public TextMeshProUGUI TextExplanation;

    public Button ButtonNewGame;

    private const string victoryText = "VICTORY";
    private const string defeatText = "DEFEAT";

    public void EndGame(bool isVictory, string explanation = "")
    {

        GameManager.Instance.ChangeState(GameState.Finishing);

        if (isVictory)
        {
            TextTitle.text = victoryText;
        }else
        {
            TextTitle.text = defeatText;
        }

        if (explanation == "") TextExplanation.text = "THE MONKING IS PROUD OF YOUR PERFORMANCE AND IS EAGER TO SEE YOUR NEXT ATTEMPT";
        else TextExplanation.text = explanation;

        float timer = ScoreTimer.Instance.timer;
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        string formattedTime = string.Format("SCORE: {0:00}:{1:00}", minutes, seconds);
        TextScore.text = formattedTime;

        ButtonNewGame.Select();
        ParentRectTransform.SetActive(true);
    }

    public void OnButtonNewGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnButtomMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
