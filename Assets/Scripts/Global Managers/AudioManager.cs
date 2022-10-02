using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : PersistentMonoBehaviourSingleton<AudioManager>
{
    public enum SFXAudioSources
    {
        UI,
        SFX,
        Dialogue
    }

    public enum UISFXs
    {
        Click,
        CountdownTick,
    }

    public enum GameplaySFXs
    {
        Guess,
        Mistake
    }

    public enum Songs
    {
        MainMenu,
        Gameplay
    }

    [Header("Audio Sources")]
    [SerializeField] AudioSource[] sfxAudioSources;
    [SerializeField] AudioSource musicAudioSource;

    [Header("Audio Clips")]
    [SerializeField] AudioClip[] uiSFXs;
    [SerializeField] AudioClip[] gameplaySFXs;
    [SerializeField] AudioClip[] music;

    [Header("Sound Options")]
    [SerializeField] bool soundOn = true;
    [SerializeField] bool musicOn = true;
    [Space]
    [SerializeField, Range(0.0f, 1.0f)] float soundBaseVolume = 1.0f;
    [SerializeField, Range(0.0f, 1.0f)] float musicBaseVolume = 1.0f;

    public bool SoundOn { set { soundOn = value; } get { return soundOn; } }
    public bool MusicOn { set { musicOn = value; } get { return musicOn; } }

    public Songs CurrentSong { private set; get; }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        musicAudioSource.volume = musicBaseVolume;
        foreach (AudioSource source in sfxAudioSources) source.volume = soundBaseVolume;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        PlayMusicOnNewScene(scene.name);
    }

    #region Sound
    private void UpdateSoundVolume(float volume) { foreach (AudioSource source in sfxAudioSources) source.volume = volume * soundBaseVolume; }

    private void StopSFXOnNewScene(string sceneName)
    {
        foreach (AudioSource source in sfxAudioSources)
        {
            do source.Stop();
            while (source.isPlaying);
        }
    }

    public void ToggleSound()
    {
        soundOn = !soundOn;
    }

    public void PlayUISFX(UISFXs sfx)
    {
        if (!soundOn) return;

        AudioSource source = sfxAudioSources[(int)SFXAudioSources.UI];

        source.clip = uiSFXs[(int)sfx];
        source.Play();
    }

    public void PlayGameplaySFX(GameplaySFXs sfx)
    {
        if (!soundOn) return;

        AudioSource source = sfxAudioSources[(int)SFXAudioSources.SFX];

        source.clip = gameplaySFXs[(int)sfx];
        source.Play();
    }
    #endregion

    #region Music
    private void UpdateMusicVolume(float volume)
    {
        musicAudioSource.volume = volume * musicBaseVolume;
    }

    private void PlayMusicOnNewScene(string sceneName)
    {
        bool playNewSong = false;
        Songs song = 0;

        switch (sceneName)
        {
            case "MainMenu":
                song = Songs.MainMenu;
                playNewSong = true;
                break;
            case "Gameplay":
                song = Songs.Gameplay;
                playNewSong = true;
                break;
            default:
                break;
        }

        if (playNewSong) PlayMusic(song);
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;

        if (musicOn) musicAudioSource.Play();
        else musicAudioSource.Stop();
    }

    public void PlayMusic(Songs song)
    {
        musicAudioSource.clip = music[(int)song];
        CurrentSong = song;

        if (musicOn) musicAudioSource.Play();
    }
    #endregion
}