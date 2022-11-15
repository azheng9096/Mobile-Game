using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager
{
    private static GameStateManager _instance;
    public static GameStateManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameStateManager();
 
            return _instance;
        }
    }

    public GameState CurrentGameState { get; private set; } = GameState.Cutscene;
    public GameState PreviousGameState { get; private set; } = GameState.Cutscene;
    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChanged;
 
    public void SetState(GameState newGameState)
    {
        if (newGameState == CurrentGameState)
            return;
        PreviousGameState = CurrentGameState;
        CurrentGameState = newGameState;
        OnGameStateChanged?.Invoke(newGameState);
    }

}
