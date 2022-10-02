using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject pauseMenu;
    bool pauseState = false;

    public void GoToGameplay()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void SetCreditsVisibility(bool newVisibility)
    {
        credits.SetActive(newVisibility);
    }

    public void SetTutorialVisibility(bool newVisibility)
    {
        tutorial.SetActive(newVisibility);
    }

    public void TogglePauseState()
    {
        SetPauseState(!pauseState);
    }

    public void SetPauseState(bool newPauseState)
    {
        pauseState = newPauseState;
        pauseMenu.SetActive(pauseState);
        if (pauseState)
        { 
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
