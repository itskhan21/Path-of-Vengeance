using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Defines the different states of the game
    public enum GameState
    {
        Gameplay,
        Story,
        Upgrade,
        Paused,
        GameOver
    }

    // Store the current state of the game
    public GameState currentState;
    // Store the previous state of the game
    public GameState previousState;

    [Header("UI")]
    public GameObject pauseScreen;

    private void Awake()
    {
        DisableScreens();
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                break;
            case GameState.Paused:
                CheckForPauseAndResume();
                break;
            case GameState.Story:

                break;
            case GameState.Upgrade:

                break;
            case GameState.GameOver:

                break;
            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        }
    }

    public void ChangeState(GameState state)
    {
        currentState = state;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            Debug.Log("Game is paused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused) 
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            Debug.Log("Game is resumed");
        }
        else if (currentState == GameState.Story || currentState == GameState.Upgrade)
        {
            ChangeState(GameState.Gameplay);
            Time.timeScale = 1f;
        }
    }

    public void StoryPause()
    {
        if (currentState != GameState.Story)
        {
            currentState = GameState.Story;
            Time.timeScale = 0f;
        }
    }

    public void UpgradePause()
    {
        if (currentState != GameState.Upgrade)
        {
            currentState = GameState.Upgrade;
            Time.timeScale = 0f;
        }
    }

    void CheckForPauseAndResume()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
                pauseScreen.SetActive(false);
            }
            else if (currentState == GameState.Gameplay)
            {
                pauseScreen.SetActive(true);
                PauseGame();
            }
        }
    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
    }

    public void ResumeButton()
    {
        ResumeGame();
        pauseScreen.SetActive(false);
    }

    public bool IsGameplay()
    {
        return currentState == GameState.Gameplay;
    }
}
