using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Selector selector;

    public void PlayGame()
    {
        if (GameManager.Instance != null && selector != null)
        {
            GameManager.Instance.selectedCharacter1 = selector.charIndex1;
            GameManager.Instance.selectedCharacter2 = selector.charIndex2;
            GameManager.Instance.selectedArena = selector.arenaImage.sprite;
        }
        SceneManager.LoadScene("BattleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
