using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private VideoPlayer openingCutscene;

    private bool openingCutscenePlayed = false;
    private bool pauseState = false;

    public static event Action OnOpeningCutscenePlayed;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (scene.name == "MainMenu" && !openingCutscenePlayed) StartCoroutine(PlayOpeningCutscene());
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

    public void GoToGameplay()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    #region Coroutines
    private IEnumerator PlayOpeningCutscene()
    {
        openingCutscene.Prepare();

        yield return new WaitUntil(() => openingCutscene.isPrepared);

        openingCutscene.Play();

        yield return new WaitUntil(() => !openingCutscene.isPlaying);

        openingCutscene.gameObject.SetActive(false);
        openingCutscenePlayed = true;

        OnOpeningCutscenePlayed?.Invoke();
    }
    #endregion
}
