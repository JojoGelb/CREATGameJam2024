using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Script.Managers
{
    public class VibrationManager : Singleton<VibrationManager>
    {
        public enum LastInputType
        {
            Keyboard,
            Gamepad
        }

        private bool isEnable = true;
        private int currentWateringCount = 0;
        public LastInputType lastInputType = LastInputType.Keyboard;

        public void StartOilPipeBeingShoot()
        {
            currentWateringCount++;
            if (isEnable && lastInputType == LastInputType.Gamepad)
            {
                Gamepad.current.SetMotorSpeeds(0.00f, 0.05f);
            }
        }

        public void StopOilPipeBeingShoot()
        {
            currentWateringCount--;
            Gamepad.current.SetMotorSpeeds(0.00f, 0.00f);
        }

        public void PlayOilPipeDestructed()
        {
            if (isEnable && lastInputType == LastInputType.Gamepad)
            {
                Gamepad.current.SetMotorSpeeds(0.30f, 0.20f);
                StartCoroutine(StopPlayingOilPipeDestructed(.3f));
            }
        }

        private IEnumerator StopPlayingOilPipeDestructed(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (currentWateringCount == 0)
            {
                Gamepad.current.SetMotorSpeeds(0.00f, 0.00f);
            }
            else
            {
                Gamepad.current.SetMotorSpeeds(0.00f, 0.05f);
            }
        }

        public void UpdateLastInput()
        {
            // trick for detecting whether a gamepad is in use
            if (Input.GetMouseButtonDown(0))
            {
                if (lastInputType == LastInputType.Gamepad)
                {
                    lastInputType = LastInputType.Keyboard;
                }
            } else if (lastInputType == LastInputType.Keyboard)
            {
                lastInputType = LastInputType.Gamepad;
            }
        }
    }
}