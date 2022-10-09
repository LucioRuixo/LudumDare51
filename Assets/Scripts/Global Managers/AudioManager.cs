using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : PersistentMonoBehaviourSingleton<AudioManager>
{
    public enum UISFXs
    {
    }

    public enum GameplaySFXs
    {
        Explosion,
        BrokenGlass,
        Jump,
        Spawn,
        Step,
        Clock,
        Huh,
        Laughter,
        Spin,
        Boo,
        Cheer,
        Courtain,
        Door,
        Slab
    }

    public enum Songs
    {
        MainMenu,
        Gameplay
    }

    [Serializable]
    private struct RandomizableSFX
    {
        public AudioClip[] clips;
    }

    [Serializable]
    private struct SourceList
    {
        public Transform container;
        public List<AudioSource> sources;
    }

    [SerializeField] private GameObject audioSourcePrefab;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private SourceList uiAudioSources;
    [SerializeField] private SourceList gameplayAudioSources;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] music;
    [SerializeField] private RandomizableSFX[] uiSFXs;
    [SerializeField] private RandomizableSFX[] gameplaySFXs;

    [Header("Sound Options")]
    [SerializeField] private bool soundOn = true;
    [SerializeField] private bool musicOn = true;
    [Space]
    [SerializeField, Range(0.0f, 1.0f)] private float soundBaseVolume = 1.0f;
    [SerializeField, Range(0.0f, 1.0f)] private float musicBaseVolume = 1.0f;

    private string lastScene;

    private bool playMusicOnEnterMainMenu = false;

    public bool SoundOn { set { soundOn = value; } get { return soundOn; } }
    public bool MusicOn { set { musicOn = value; } get { return musicOn; } }

    public Songs CurrentSong { private set; get; }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UIManager_MainMenu.OnOpeningCutscenePlayed += OnOpeningCutscenePlayed;
    }

    void Start()
    {
        musicAudioSource.volume = musicBaseVolume;
        foreach (AudioSource source in uiAudioSources.sources) source.volume = soundBaseVolume;
        foreach (AudioSource source in gameplayAudioSources.sources) source.volume = soundBaseVolume;

        if (SceneManager.GetActiveScene().name != "MainMenu") playMusicOnEnterMainMenu = true;
    }

    private void Update()
    {
        if (!musicAudioSource.isPlaying) PlayMusicOnScene(SceneManager.GetActiveScene().name);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        UIManager_MainMenu.OnOpeningCutscenePlayed -= OnOpeningCutscenePlayed;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        PlayMusicOnScene(scene.name);

        lastScene = scene.name;
    }

    private void OnOpeningCutscenePlayed()
    {
        PlayMusic(Songs.MainMenu);
        playMusicOnEnterMainMenu = true;
    }

    #region SFX
    private void UpdateSoundVolume(float volume)
    {
        foreach (AudioSource source in uiAudioSources.sources) source.volume = volume * soundBaseVolume;
        foreach (AudioSource source in gameplayAudioSources.sources) source.volume = volume * soundBaseVolume;
    }

    private void StopSFXOnNewScene(string sceneName)
    {
        foreach (AudioSource source in uiAudioSources.sources)
        {
            do source.Stop();
            while (source.isPlaying);
        }

        foreach (AudioSource source in gameplayAudioSources.sources)
        {
            do source.Stop();
            while (source.isPlaying);
        }
    }

    private AudioSource GetAvailableAudioSource(SourceList sourceList)
    {
        foreach (AudioSource source in sourceList.sources) if (!source.isPlaying) return source;

        AudioSource newSource = Instantiate(audioSourcePrefab, sourceList.container).GetComponent<AudioSource>();
        sourceList.sources.Add(newSource);

        return newSource;
    }

    private AudioClip GetRandomClip(RandomizableSFX sfx)
    {
        return sfx.clips[UnityEngine.Random.Range(0, sfx.clips.Length)];
    }

    public void ToggleSound()
    {
        soundOn = !soundOn;
    }

    public void PlayUISFX(UISFXs sfx)
    {
        if (!soundOn) return;

        AudioSource source = GetAvailableAudioSource(uiAudioSources);

        source.clip = GetRandomClip(uiSFXs[(int)sfx]);
        source.Play();
    }

    public void PlayGameplaySFX(GameplaySFXs sfx)
    {
        if (!soundOn) return;

        AudioSource source = GetAvailableAudioSource(gameplayAudioSources);

        source.clip = GetRandomClip(gameplaySFXs[(int)sfx]);
        source.Play();
    }
    #endregion

    #region Music
    private void UpdateMusicVolume(float volume)
    {
        musicAudioSource.volume = volume * musicBaseVolume;
    }

    private void PlayMusicOnScene(string sceneName)
    {
        bool playNewSong = false;
        Songs song = 0;

        switch (sceneName)
        {
            case "MainMenu":
                if (playMusicOnEnterMainMenu)
                {
                    song = Songs.MainMenu;
                    playNewSong = true;
                }
                break;
            case "Gameplay":
                if (lastScene != "Gameplay")
                {
                    song = Songs.Gameplay;
                    playNewSong = true;
                }
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