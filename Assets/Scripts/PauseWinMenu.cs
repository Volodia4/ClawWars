using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseWinMenu : MonoBehaviour
{
    [Header("Лейбли хелсбарів")]
    public TextMeshProUGUI LabelHealth1;
    public TextMeshProUGUI LabelHealth2;

    [Header("Лейбл результату")]
    public TextMeshProUGUI LabelResult;

    [Header("Панелька паузи і панелька результату")]
    public GameObject pauseMenuPanel;
    public GameObject resultPanel;

    private AudioManager am;
    private AudioSource[] allAudioSources;
    private bool isPaused, isBattleEnded;

    private void Awake()
    {
        am = AudioManager.Instance;

        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();

        if (isBattleEnded) return;

        int health1 = int.Parse(LabelHealth1.text);
        int health2 = int.Parse(LabelHealth2.text);

        bool p1Dead = health1 <= 0;
        bool p2Dead = health2 <= 0;

        if (p1Dead && p2Dead) ShowResult("Draw!");
        else if (p2Dead) ShowResult("Player 1 wins!");
        else if (p1Dead) ShowResult("Player 2 wins!");
    }

    private void ShowResult(string message)
    {
        allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (var src in allAudioSources) if (src.isPlaying) src.Pause();

        if (isBattleEnded) return;
        am.PlaySFX("Win", 3);

        Time.timeScale = 0f;
        isBattleEnded = true;
        resultPanel.SetActive(true);
        LabelResult.text = message;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TogglePause()
    {
        if (isBattleEnded) return;
        if (isPaused) Resume();
        else Pause();
    }

    private void Pause()
    {
        allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (var src in allAudioSources)
            if (src.isPlaying && src.gameObject.name != "MusicSource") src.Pause();

        am.PlaySFX("Click");

        Time.timeScale = 0f;
        isPaused = true;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    private void Resume()
    {
        am.PlaySFX("Click");

        Time.timeScale = 1f;
        isPaused = false;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(false);
        isBattleEnded = false;
    }

    public void GoToMainMenu()
    {
        am.PlaySFX("Click");

        SceneManager.LoadScene("MainMenu");
    }
}
