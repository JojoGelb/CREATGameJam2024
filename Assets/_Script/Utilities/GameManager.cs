using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

//Author @Jordy

public class GameManager : Singleton<GameManager>
{

    public event EventHandler<EventGameStateArgs> OnStateChanged;

    public class EventGameStateArgs: EventArgs
    {
        public GameState state;
    }

    public Transform Player;

    public GameState state { get; private set; }

    private void Start()
    {
        ChangeState(GameState.BeforeStart);
        /*InputManager.Instance.SubscribeToEscape(InputManager_OnPausePressed);*/
    }

    public void ChangeState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.BeforeStart:
                Time.timeScale = 1f;
                //Do some start things
                HandleBeforeStart();
                break;
            case GameState.Starting:
                //Do some start things
                break;
            case GameState.Pause:
                //Do some pause things
                Time.timeScale = 0;
                break;
            case GameState.Playing:
                //Do some start things
                Time.timeScale = 1;
                //Doing things like UnitManager.Instance.SpawnNumbers() in the handler of this event
                break;
            case GameState.Finishing:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnStateChanged?.Invoke(this, new EventGameStateArgs{ state = state });
    }

    private void HandleBeforeStart()
    {
    }

    public enum GameState {
        BeforeStart = 0,
        Starting = 1,
        Playing = 2,
        Finishing = 3,
        Pause = 4
    }

    /*private void InputManager_OnPausePressed(InputAction.CallbackContext e)
    {
        if (state == GameState.Pause)
        {
            InputManager.Instance.EnableInput();
            
            ChangeState(GameState.Playing);
            
        } else
        {
            InputManager.Instance.DisableInput();
            
            ChangeState(GameState.Pause);
        }
        
    }*/
}
