using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerGameplay : MonoBehaviourSingleton<UIManagerGameplay>
{
    [SerializeField] private GameObject pauseMenu;
    private bool pauseState = false;

    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
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
}
