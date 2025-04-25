using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CooldownSlot : MonoBehaviour
{
    [Header("Об'єкти префаба")]
    public Image iconImage;
    public Image fillMask;
    public TMP_Text timerText;

    private float duration;

    public void Initialize(Sprite icon, float cdDuration)
    {
        iconImage.sprite = icon;
        fillMask.fillAmount = 0f;
        timerText.text = "";
        duration = cdDuration;

        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float remaining = duration - elapsed;
            fillMask.fillAmount = elapsed / duration;
            timerText.text = remaining.ToString("F2");
            yield return null;
        }

        Destroy(gameObject);
    }
}
