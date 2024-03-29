using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    
    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float countdouwnToStartTimer = 3;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 60;
    private bool isGamePaused;

    void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        DeliveryManager.Instance.OnRecipeSucess += DeliveryManager_OnRecipeSuccess;
        GameOverUI.OnResetGame += GameOverUI_OnGameReset;
    }

    private void GameOverUI_OnGameReset(object sender, EventArgs e)
    {
        Loader.Load(Loader.Scene.MainMenuScene);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        gamePlayingTimer += 10;
        if (gamePlayingTimer > gamePlayingTimerMax)
        {
            gamePlayingTimer = gamePlayingTimerMax;
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountDownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:

                break;
            case State.CountDownToStart:
                GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
                countdouwnToStartTimer -= Time.deltaTime;
                if (countdouwnToStartTimer < 0)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0)
                {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountDownToStart;
    }

    public float GetCoundownToStartTimer()
    {
        return countdouwnToStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimeNormalized()
    {
        return gamePlayingTimer / gamePlayingTimerMax;
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
