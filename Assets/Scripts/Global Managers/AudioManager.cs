﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : PersistentMonoBehaviourSingleton<AudioManager>
{
    public enum SFXAudioSources
    {
        UI,
        SFX
    }

    public enum UISFXs
    {
        Click,
        CountdownTick,
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
        foreach (AudioSource source in sfxAudioSources) source.volume = soundBaseVolume;

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