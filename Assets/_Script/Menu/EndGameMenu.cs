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

        SaveHighScores(minutes, seconds);
    }

    public List<HighScoreEntry> LoadHighScores()
    {
        List<HighScoreEntry> highScoreEntries = new List<HighScoreEntry>();

        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey("HighScore_Minutes_" + i))
            {
                float minutes = PlayerPrefs.GetFloat("HighScore_Minutes_" + i);
                float seconds = PlayerPrefs.GetFloat("HighScore_Seconds_" + i);
                string name = PlayerPrefs.GetString("HighScore_Name_" + i);

                highScoreEntries.Add(new HighScoreEntry(minutes, seconds, name));
            }
        }

        return highScoreEntries;
    }

    public void SaveHighScores(float minutes, float seconds)
    {
        List<HighScoreEntry> highScoreEntries = LoadHighScores();

        if (highScoreEntries.Count < 5) {
            highScoreEntries.Add(new HighScoreEntry(minutes,seconds,PlayerPrefs.GetString("CurrentName")));
        } else
        {
            int index = highScoreEntries.Count - 1;
            if ((highScoreEntries[index].minutes < minutes) ||
                (highScoreEntries[index].minutes == minutes && (highScoreEntries[index].seconds < seconds))) {
                highScoreEntries[index] = new HighScoreEntry(minutes, seconds, PlayerPrefs.GetString("CurrentName"));
            }
        }

        highScoreEntries.Sort((x, y) => y.TotalTime().CompareTo(x.TotalTime()));

        for (int i = 0; i < highScoreEntries.Count; i++)
        {
                PlayerPrefs.SetFloat("HighScore_Minutes_" + i, highScoreEntries[i].minutes);
                PlayerPrefs.SetFloat("HighScore_Seconds_" + i, highScoreEntries[i].seconds);
                PlayerPrefs.SetString("HighScore_Name_" + i, highScoreEntries[i].name);
        }
        PlayerPrefs.Save();
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
