using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameOverUI : MonoBehaviour
{
    public static event EventHandler OnResetGame;
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private TextMeshProUGUI pressInteractButtonText;
    private float restartTimer;
    private float restartTimerMax = 5;
    private bool isShowed;
    private bool canReset;

    void Awake()
    {
        restartTimer = restartTimerMax;
    }

    void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

        Hide();
        pressInteractButtonText.gameObject.SetActive(false);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (canReset)
        {
            OnResetGame?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            Show();
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        isShowed = true;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        isShowed = false;
    }

    void Update()
    {
        if (isShowed)
        {
            restartTimer -= Time.deltaTime;

            if (restartTimer <= 0)
            {
                pressInteractButtonText.gameObject.SetActive(true);
                canReset = true;
            }
        }
    }
}
