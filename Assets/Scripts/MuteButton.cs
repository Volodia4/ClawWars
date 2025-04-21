using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [Header("Іконки")]
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private Image image;
    static private bool muted;

    void Awake()
    {
        image  = GetComponent<Image>();
        image.sprite = muted ? soundOffSprite : soundOnSprite;
    }

    public void Mute()
    {
        muted = !muted;
        AudioListener.volume = muted ? 0f : 1f;
        image.sprite = muted ? soundOffSprite : soundOnSprite;
    }
}
