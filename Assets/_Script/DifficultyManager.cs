using System;
using UnityEngine;

namespace _Script
{
    public class DifficultyManager : Singleton<DifficultyManager>
    {
        private float timer;
        private float lastFrame;
        private float timeBetweenBurstOfPoiSon;

        public float GetTimeBetweenBurstOfPoiSon() => timeBetweenBurstOfPoiSon;

        private void Start()
        {
            //RocketManager.Instance.SpawnInterval = 5f;
            //PollutionManager.Instance.FramesBetweenDilatationPass = 150;

            timeBetweenBurstOfPoiSon = 2f;
        }

        private void Update()
        {
            timer = ScoreTimer.Instance.timer;

            if (Mathf.CeilToInt(timer) % 15 == 0 && Mathf.CeilToInt(lastFrame) % 15 != 0)
            {
                RocketManager.Instance.SpawnInterval = Mathf.Max(.4f, RocketManager.Instance.SpawnInterval * 0.85f);
                timeBetweenBurstOfPoiSon = Mathf.Max(.2f, timeBetweenBurstOfPoiSon * .9f);

                //int tmpDilatationPass = Mathf.CeilToInt(PollutionManager.Instance.FramesBetweenDilatationPass* 0.85f);
                //PollutionManager.Instance.FramesBetweenDilatationPass = tmpDilatationPass;
            }

            lastFrame = timer;
        }
    }
}