using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//Author: Jordy Gelb
public class InputManager : Singleton<InputManager>
{
    //private PlayerControls playerInputAction;

    public static event Action<InputActionMap> actionMapChange;


    protected override void Awake()
    {
        base.Awake();
        //playerInputAction = new PlayerControls();
        //playerInputAction.Enable();
    }

    private void OnDestroy()
    {
        //playerInputAction.Dispose();
        //playerInputAction = null;

    }

    public void DisableGameplayInput()
    {
        //playerInputAction.Gameplay.Disable();
    }

    public void EnableGameplayInput()
    {
        //playerInputAction.Gameplay.Enable();
    }

    /*public Vector2 GetMovementVectorNormalized()
    {
        //Vector2 inputVector = playerInputAction.Gameplay.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }*/

    public void RegisterToFireEvent(Action<InputAction.CallbackContext> callback)
    {
        //playerInputAction.Gameplay.Fire.started += callback;
    }

    public void UnRegisterToFireEvent(Action<InputAction.CallbackContext> callback)
    {
        //playerInputAction.Gameplay.Fire.started -= callback;
    }
}
