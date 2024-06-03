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

    public GameState state { get; private set; }
    private GameState previousState;

    private void Start()
    {
        ChangeState(GameState.BeforeStart);
    }

    public void ChangeState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.BeforeStart:
                Time.timeScale = 1f;
                //Do some start things
                Invoke(nameof(HandleBeforeStart),0.1f);
                break;
            case GameState.Starting:
                //Do some start things
                break;
            case GameState.Pause:
                //Do some pause things
                //InputManager.Instance.DisableGameplayInput();
                Time.timeScale = 0;
                break;
            case GameState.Playing:
                //Do some start things
                InputManager.Instance.EnableGameplayInput();
                Time.timeScale = 1;
                break;
            case GameState.Finishing:
                InputManager.Instance.DisableGameplayInput();
                Time.timeScale = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnStateChanged?.Invoke(this, new EventGameStateArgs{ state = state });
    }

    private void HandleBeforeStart()
    {
        //Animation 3-2-1;
        ChangeState(GameState.Playing);
    }

    public enum GameState {
        BeforeStart = 0,
        Starting = 1,
        Playing = 2,
        Finishing = 3,
        Pause = 4
    }

    public void ResetToPrecedentState()
    {
        if(previousState == GameState.Playing)
            InputManager.Instance.EnableGameplayInput();
        ChangeState(previousState);
    }
}
