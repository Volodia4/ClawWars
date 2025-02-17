using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int selectedCharacter1, selectedCharacter2;
    public Sprite selectedArena;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
