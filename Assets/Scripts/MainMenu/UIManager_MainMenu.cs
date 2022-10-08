using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UIManager_MainMenu : MonoBehaviourSingleton<UIManager_MainMenu>
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private VideoPlayer openingCutscene;

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
            if (!UIManager.Get().OpeningCutscenePlayed) StartCoroutine(PlayOpeningCutscene());
            else SetMenuPanelVisibility(true);
        }
    }

    public void SkipOpeningCutscene()
    {
        StopCoroutine(PlayOpeningCutscene());
        openingCutscene.Stop();

        SetMenuPanelVisibility(true);
        SetOpeningCutsceneVisibility(false);
        UIManager.Get().OpeningCutscenePlayed = true;
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

    #region Element Visibility
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
        openingCutscene.transform.parent.gameObject.SetActive(newVisibility);
    }
    #endregion

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
        UIManager.Get().OpeningCutscenePlayed = true;

        OnOpeningCutscenePlayed?.Invoke();
    }
    #endregion
}
