using _Script;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    public Slider slider;
    public Image fillBar;
    public float UpdateRateInSeconds = 0.5f;

    [Range(0f, 1f)]
    public float lostPercentage = 0.9f;

    private WaitForSeconds w;

    [Range(0f, 1f)]
    public float DangerousPercentage = 0.7f;

    private Color initialColor;

    private bool flash = false;
    private float currentTime;
    bool reverse = false;
    

    private void Start()
    {
        w = new WaitForSeconds(UpdateRateInSeconds);
        StartCoroutine(UpdateHealthBar());
        slider.value = 0;
        slider.maxValue = lostPercentage;
        initialColor = fillBar.color;
    }

    void Update()
    {
        if(flash)
        {
            float duration = 0.5f;
            float halfDuration = duration / 2;
            currentTime += Time.deltaTime;

            Color c;

            if (currentTime<halfDuration)
            {
                float t = currentTime / halfDuration;
                c = Color.Lerp(initialColor, Color.red, t);
            }
            else
            {
                float t = (currentTime-halfDuration) / halfDuration;
                c = Color.Lerp(initialColor, Color.red, t);
            }

            fillBar.color = c;

            if (currentTime>duration)
            {
                currentTime = 0;
                flash = false;
                fillBar.color = initialColor;
            }
        }
    }

    IEnumerator UpdateHealthBar()
    {
        yield return new WaitForSeconds(7); //quick fix: strangely the texture2D return 1 on the first few frames
        while (true)
        {
            float health = PollutionManager.Instance.GetPercentageTextureFilled();

            if(health >= lostPercentage)
            {
                break;
            }

            slider.DOValue(health, 1);

            if (health > DangerousPercentage)
            {
                flash = true;
            }

            yield return w;
        }

        slider.value = slider.maxValue;

        EndGameMenu.Instance.EndGame(false, "THE HUMAN POLLUTION WAS TOO MUCH FOR YOUR PLANET TO HANDLE");
    }
}
