using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Панелька паузи")]
    public GameObject pauseMenuPanel;

    private bool paused = false;

    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void TogglePause()
    {
        if (paused) Resume();
        else Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        paused = true;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        paused = false;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }
}
