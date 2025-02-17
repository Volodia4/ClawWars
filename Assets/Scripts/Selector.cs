using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    [Header("Character Selection")]
    public Image characterImage1, characterImage2;
    public Sprite[] characterSprites;
    [HideInInspector]
    public int charIndex1 = 0, charIndex2 = 0;

    [Header("Arena Selection")]
    public Image arenaImage;
    public Sprite[] arenaSprites;
    [HideInInspector]
    public int arenaIndex = 0;

    void Start()
    {
        characterImage1.sprite = characterSprites[0];
        characterImage2.sprite = characterSprites[0];

        arenaImage.sprite = arenaSprites[0];
    }

    public void NextCharacter1()
    {
        charIndex1 = (charIndex1 + 1) % characterSprites.Length;
        characterImage1.sprite = characterSprites[charIndex1];
    }

    public void PreviousCharacter1()
    {
        charIndex1 = (charIndex1 - 1 + characterSprites.Length) % characterSprites.Length;
        characterImage1.sprite = characterSprites[charIndex1];
    }

    public void NextCharacter2()
    {
        charIndex2 = (charIndex2 + 1) % characterSprites.Length;
        characterImage2.sprite = characterSprites[charIndex2];
    }

    public void PreviousCharacter2()
    {
        charIndex2 = (charIndex2 - 1 + characterSprites.Length) % characterSprites.Length;
        characterImage2.sprite = characterSprites[charIndex2];
    }

    public void NextArena()
    {
        arenaIndex = (arenaIndex + 1) % arenaSprites.Length;
        arenaImage.sprite = arenaSprites[arenaIndex];
    }

    public void PreviousArena()
    {
        arenaIndex = (arenaIndex - 1 + arenaSprites.Length) % arenaSprites.Length;
        arenaImage.sprite = arenaSprites[arenaIndex];
    }
}
