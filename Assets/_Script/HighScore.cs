using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public List<TemplateHighscore> templateList = new List<TemplateHighscore>();

    public TextMeshProUGUI textEmpty;

    public TMP_InputField inputField;

    public GameObject mainMenu;
    public Button BaseSelect;


    private void Awake()
    {
        string name = PlayerPrefs.GetString("CurrentName");

        if(name == "")
        {
            inputField.text = "Monkeyta";
        } else
        {
            inputField.text = name;
        }
    }

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
        if(transform.GetChild(0).gameObject.activeSelf == true){
            CloseHighScore();
        }
    }

    public void CloseHighScore()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        PlayerPrefs.SetString("CurrentName", inputField.text);
        mainMenu.SetActive(true);
        BaseSelect.Select();
    }

    public void OnHighScore()
    {
        mainMenu.SetActive(false);
        inputField.Select();
        transform.GetChild(0).gameObject.SetActive(true);

        List<HighScoreEntry> highScoreEntries = LoadHighScores();
        for (int i = 0; i < templateList.Count; i++)
        {
            if(i < highScoreEntries.Count)
            {
                templateList[i].SetUI(highScoreEntries[i]);
            } else
            {
                templateList[i].gameObject.SetActive(false);
            }
        }

        if(highScoreEntries.Count == 0)
        {
            textEmpty.gameObject.SetActive(true);
        }
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

}

[System.Serializable]
public class HighScoreEntry
{
    public float minutes;
    public float seconds;
    public string name;

    public HighScoreEntry(float minutes, float seconds, string name)
    {
        this.minutes = minutes;
        this.seconds = seconds;
        this.name = name;
    }

    // Calculate total time in seconds for comparison
    public float TotalTime()
    {
        return minutes * 60f + seconds;
    }
}
