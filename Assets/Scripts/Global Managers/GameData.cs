using UnityEngine.SceneManagement;

public class GameData : PersistentMonoBehaviourSingleton <GameData>
{
    private bool win = false;

    public void SetWinState(bool newWinState)
    {
        win = newWinState;
        SceneManager.LoadScene(2);
    }

    public bool GetWinState()
    {
        return win;
    }
}