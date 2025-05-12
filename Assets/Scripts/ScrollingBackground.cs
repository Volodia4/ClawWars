using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    [Header("Ўвидк≥сть прокрутки картинки")]
    public Vector2 scrollSpeed;

    private RawImage rawImage;
    private Vector2 currentOffset;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    void Update()
    {
        currentOffset += scrollSpeed * Time.deltaTime;
        rawImage.uvRect = new Rect(currentOffset, rawImage.uvRect.size);
    }
}
