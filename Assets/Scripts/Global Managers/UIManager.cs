using UnityEngine.SceneManagement;

public class UIManager : PersistentMonoBehaviourSingleton<UIManager>
{
    private bool openingCutscenePlayed = false;
    public bool OpeningCutscenePlayed { get => openingCutscenePlayed; set => openingCutscenePlayed = value; }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu") openingCutscenePlayed = true;
    }
}