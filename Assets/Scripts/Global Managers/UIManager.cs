public class UIManager : PersistentMonoBehaviourSingleton<UIManager>
{
    private bool openingCutscenePlayed = false;
    public bool OpeningCutscenePlayed { get => openingCutscenePlayed; set => openingCutscenePlayed = value; }
}