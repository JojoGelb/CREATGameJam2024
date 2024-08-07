using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

//Author: Jordy Gelb
public class InputManager : Singleton<InputManager>
{
    private PlayerControls playerInputAction;

    public PlayerControls GetPlayerControls() => playerInputAction;

    //public static event Action<InputActionMap> actionMapChange;


    protected override void Awake()
    {
        base.Awake();
        playerInputAction = new PlayerControls();
        playerInputAction.Enable();
    }

    private void OnDestroy()
    {
        Debug.Log("Disabled");
        playerInputAction.Dispose();
        playerInputAction = null;
    }

    public void DisableGameplayInput()
    {
        playerInputAction.Gameplay.Disable();
    }

    public void EnableGameplayInput()
    {
        playerInputAction.Gameplay.Enable();
    }

    public void RegisterToEscape(Action<InputAction.CallbackContext> callback)
    {
        playerInputAction.UI.Pause.started += callback;
    }

    public Vector2 GetMovementVectorNormalized()
    {
        if (playerInputAction == null) return Vector2.zero;
        Vector2 inputVector = playerInputAction.Gameplay.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }

    public void RegisterToFireEventStarted(Action<InputAction.CallbackContext> callback)
    {
        if (playerInputAction == null) return;
        playerInputAction.Gameplay.Fire.started += callback;
    }

    public void RegisterToFireEventCanceled(Action<InputAction.CallbackContext> callback)
    {
        if (playerInputAction == null) return;
        playerInputAction.Gameplay.Fire.canceled += callback;
    }

    public void UnRegisterToFireEventCanceled(Action<InputAction.CallbackContext> callback)
    {
        if (playerInputAction == null) return;
        playerInputAction.Gameplay.Fire.canceled -= callback;
    }

    public void UnRegisterToFireEventStarted(Action<InputAction.CallbackContext> callback)
    {
        if (playerInputAction == null) return;
        playerInputAction.Gameplay.Fire.started -= callback;
    }

    public void RegisterToJumpEvent(Action<InputAction.CallbackContext> callback)
    {
        if (playerInputAction == null) return;
        playerInputAction.Gameplay.Jump.started += callback;
    }

    public void UnRegisterToJumpEvent(Action<InputAction.CallbackContext> callback)
    {
        if (playerInputAction == null) return;
        playerInputAction.Gameplay.Jump.started -= callback;
    }

    public void RegisterToBEvent(Action<InputAction.CallbackContext> callback)
    {
        if (playerInputAction == null) return;
        playerInputAction.UI.B.started += callback;
    }

    public void UnRegisterToBEvent(Action<InputAction.CallbackContext> callback)
    {
        if (playerInputAction == null) return;
        playerInputAction.UI.B.started -= callback;
    }


}
