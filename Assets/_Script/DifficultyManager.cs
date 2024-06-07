using System;
using UnityEngine;

namespace _Script
{
    public class DifficultyManager : Singleton<DifficultyManager>
    {
        private float timer;
        private float lastFrame;
        private float timeBetweenBurstOfPoiSon;

        private float BASE_ROCKET_INTERVAL = 5f;
        private float BASE_BUBBLES_INTERVAL = 2.5f;

        public float GetTimeBetweenBurstOfPoiSon() => timeBetweenBurstOfPoiSon;

        private void Start()
        {
            PollutionManager.Instance.FramesBetweenDilatationPass = 75;
            RocketManager.Instance.SpawnInterval = 5;
            timeBetweenBurstOfPoiSon = BASE_BUBBLES_INTERVAL;
        }

        private void Update()
        {
            timer = ScoreTimer.Instance.timer;

            if (Mathf.CeilToInt(timer) % 15 == 0 && Mathf.CeilToInt(lastFrame) % 15 != 0)
            {
                RocketManager.Instance.SpawnInterval =
                    2 + ((BASE_ROCKET_INTERVAL - 2) * Mathf.Pow(0.93f, (int)(timer / 15)));

                timeBetweenBurstOfPoiSon =
                    .5f + ((BASE_BUBBLES_INTERVAL - .5f) * Mathf.Pow(0.93f, (int)(timer / 15)));
                Debug.Log(timer + " s       : " + timeBetweenBurstOfPoiSon);
            }

            lastFrame = timer;
        }
    }
}