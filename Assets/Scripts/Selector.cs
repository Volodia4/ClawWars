using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    [Header("Вибір персонажа")]
    public Image characterImage1;
    public Image characterImage2;
    public Sprite[] characterImages;

    [Header("Вибір арени")]
    public Image arenaImage;
    public Sprite[] arenaImages;

    [HideInInspector]
    public int charIndex1 = 0, charIndex2 = 0, arenaIndex = 0;

    private AudioManager am;

    private void Awake()
    {
        am = AudioManager.Instance;
    }

    void Start()
    {
        characterImage1.sprite = characterImages[0];
        characterImage2.sprite = characterImages[0];
        arenaImage.sprite = arenaImages[0];
    }

    public void NextCharacter1()
    {
        am.PlaySFX("Click");

        charIndex1 = (charIndex1 + 1) % characterImages.Length;
        characterImage1.sprite = characterImages[charIndex1];
    }

    public void PreviousCharacter1()
    {
        am.PlaySFX("Click");

        charIndex1 = (charIndex1 - 1 + characterImages.Length) % characterImages.Length;
        characterImage1.sprite = characterImages[charIndex1];
    }

    public void NextCharacter2()
    {
        am.PlaySFX("Click");

        charIndex2 = (charIndex2 + 1) % characterImages.Length;
        characterImage2.sprite = characterImages[charIndex2];
    }

    public void PreviousCharacter2()
    {
        am.PlaySFX("Click");

        charIndex2 = (charIndex2 - 1 + characterImages.Length) % characterImages.Length;
        characterImage2.sprite = characterImages[charIndex2];
    }

    public void NextArena()
    {
        am.PlaySFX("Click");

        arenaIndex = (arenaIndex + 1) % arenaImages.Length;
        arenaImage.sprite = arenaImages[arenaIndex];
    }

    public void PreviousArena()
    {
        am.PlaySFX("Click");

        arenaIndex = (arenaIndex - 1 + arenaImages.Length) % arenaImages.Length;
        arenaImage.sprite = arenaImages[arenaIndex];
    }
}
