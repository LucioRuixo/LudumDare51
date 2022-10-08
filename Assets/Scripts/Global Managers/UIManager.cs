using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    [Header("Main Menu")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private VideoPlayer openingCutscene;

    [Header("Gameplay")]
    [SerializeField] private GameObject pauseMenu;

    private bool openingCutscenePlayed = true;
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
        if (scene.name == "MainMenu")
        {
            if (!openingCutscenePlayed) StartCoroutine(PlayOpeningCutscene());
            else SetMenuPanelVisibility(true);
        }
    }

    public void SetMenuPanelVisibility(bool newVisibility)
    {
        menuPanel.SetActive(newVisibility);
    }

    public void SetCreditsVisibility(bool newVisibility)
    {
        credits.SetActive(newVisibility);
    }

    public void SetTutorialVisibility(bool newVisibility)
    {
        tutorial.SetActive(newVisibility);
    }

    public void SetOpeningCutsceneVisibility(bool newVisibility)
    {
        openingCutscene.gameObject.SetActive(newVisibility);
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
        SetOpeningCutsceneVisibility(true);
        openingCutscene.Prepare();

        yield return new WaitUntil(() => openingCutscene.isPrepared);

        openingCutscene.Play();

        yield return new WaitUntil(() => !openingCutscene.isPlaying);

        SetMenuPanelVisibility(true);
        SetOpeningCutsceneVisibility(false);
        openingCutscenePlayed = true;

        OnOpeningCutscenePlayed?.Invoke();
    }
    #endregion
}
