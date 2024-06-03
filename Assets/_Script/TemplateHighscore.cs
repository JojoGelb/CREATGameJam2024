using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TemplateHighscore : MonoBehaviour
{

    public TextMeshProUGUI textName;
    public TextMeshProUGUI textTimer;

    public void SetUI(HighScoreEntry entry)
    {
        string formattedTime = string.Format("{0:00}:{1:00}", entry.minutes, entry.seconds);
        textTimer.text = formattedTime;

        textName.text = entry.name;
    }
}
