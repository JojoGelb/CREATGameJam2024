using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateUiTimer : MonoBehaviour
{
    public TextMeshProUGUI TmptText;
    private WaitForSecondsRealtime w = new WaitForSecondsRealtime(1);
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TextUpdate());
    }

    IEnumerator TextUpdate()
    {
        while (true)
        {
            float timer = ScoreTimer.Instance.timer;
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);

            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            TmptText.text = formattedTime;
            yield return w;
        }
    }
}
