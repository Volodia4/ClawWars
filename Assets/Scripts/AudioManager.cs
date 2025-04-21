using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Джерела")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Усі кліпи")]
    public AudioClip[] musicClips;
    public AudioClip[] sfxClips;

    private Dictionary<string, AudioClip> sfxMap;
    private Dictionary<string, AudioClip> musicMap;
    private Dictionary<string, AudioClip[]> sceneMusicMap;
    private Coroutine musicLoopRoutine;
    private string currentLoopSFX;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildDictionaries();
            BuildSceneMusicMap();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusicLoop();
    }

    private void BuildDictionaries()
    {
        sfxMap = new Dictionary<string, AudioClip>();
        foreach (var clip in sfxClips) sfxMap[clip.name] = clip;

        musicMap = new Dictionary<string, AudioClip>();
        foreach (var clip in musicClips) musicMap[clip.name] = clip;
    }

    private void BuildSceneMusicMap()
    {
        sceneMusicMap = new Dictionary<string, AudioClip[]>
    { { "MainMenu", new AudioClip[] { musicMap["track1"], musicMap["track2"],
            musicMap["track3"], musicMap["track4"], musicMap["track5"], musicMap["track6"] } },
        { "BattleScene", new AudioClip[] { musicMap["battle1"], musicMap["battle2"],
            musicMap["battle3"], musicMap["battle4"], musicMap["battle5"], musicMap["battle6"] } } };
    }

    public void PlaySceneMusicLoop()
    {
        if (musicLoopRoutine != null) StopCoroutine(musicLoopRoutine);
        musicLoopRoutine = StartCoroutine(PlayMusicForCurrentScene());
    }

    private IEnumerator<WaitForSeconds> PlayMusicForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (!sceneMusicMap.ContainsKey(sceneName))
        {
            Debug.LogWarning("Нема музики для сцени: " + sceneName);
            yield break;
        }

        var clips = sceneMusicMap[sceneName];

        while (true)
        {
            var clip = clips[Random.Range(0, clips.Length)];
            musicSource.clip = clip;
            musicSource.Play();
            yield return new WaitForSeconds(clip.length);
        }
    }

    public void StopMusic()
    {
        if (musicLoopRoutine != null)
        {
            StopCoroutine(musicLoopRoutine);
            musicLoopRoutine = null;
        }
        musicSource.Stop();
    }

    public void PlaySFX(string name, float volume = 1f)
    {
        if (!sfxMap.ContainsKey(name)) return;
        sfxSource.PlayOneShot(sfxMap[name], volume);
    }

    public void PlayLoopingSFX(string name, AudioSource loopSfxSource)
    {
        if (!sfxMap.ContainsKey(name)) return;
        if (loopSfxSource.isPlaying && currentLoopSFX == name) return;

        loopSfxSource.clip = sfxMap[name];
        loopSfxSource.loop = true;
        loopSfxSource.Play();
        currentLoopSFX = name;
    }

    public void StopLoopingSFX(AudioSource loopSfxSource)
    {
        if (loopSfxSource.isPlaying)
        {
            loopSfxSource.Stop();
            loopSfxSource.loop = false;
            currentLoopSFX = null;
            loopSfxSource.clip = null;
        }
    }
}
