using System;
using UnityEngine;

public class PlayerWaterShooter: MonoBehaviour
{
    private void Start()
    {
        InputManager.Instance.RegisterToFireEventStarted(OnFireStarted);
        InputManager.Instance.RegisterToFireEventCanceled(OnFireCanceled);
    }

    private void OnDisable()
    {
        InputManager.Instance.UnRegisterToFireEventStarted(OnFireStarted);
        InputManager.Instance.UnRegisterToFireEventCanceled(OnFireCanceled);
    }
    private void OnFireStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //Launch water
        Debug.Log("Shoot water");
    }

    private void OnFireCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //Stop water
        Debug.Log("Stop water");
    }


}
