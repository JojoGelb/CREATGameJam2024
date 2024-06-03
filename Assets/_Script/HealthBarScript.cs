using _Script;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    public Slider slider;
    public float UpdateRateInSeconds = 0.2f;

    [Range(0f, 1f)]
    public float lostPercentage = 0.9f;

    private WaitForSeconds w;

    private void Start()
    {
        w = new WaitForSeconds(UpdateRateInSeconds);
        StartCoroutine(UpdateHealthBar());
        slider.value = 0;
    }

    IEnumerator UpdateHealthBar()
    {
        yield return new WaitForSeconds(3); //quick fix: strangely the texture2D return 1 on the first few frames
        while (true)
        {
            float health = PollutionManager.Instance.GetPercentageTextureFilled();
            slider.DOValue(health, 1);
            if(health > lostPercentage)
            {
                break;
            }
            yield return w;
        }

        EndGameMenu.Instance.EndGame(false, "THE HUMAN POLLUTION WAS TOO MUCH FOR YOUR PLANET TO HANDLE");
    }
}
