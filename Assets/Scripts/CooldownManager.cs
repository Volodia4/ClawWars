using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    public static CooldownManager Instance;

    [Header("Посилання на контейнер і префаб кд")]
    public Transform cooldownContainerP1;
    public Transform cooldownContainerP2;
    public GameObject cooldownSlotPrefab;

    [Header("Іконки кд")]
    public Sprite iconSwordShort;
    public Sprite iconSwordLong;
    public Sprite iconShield;
    public Sprite iconMageShort;
    public Sprite iconFireball;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnCooldown(bool isP1, bool isWarrior, int iconChoose, float duration)
    {
        Sprite icon = null;
        if (isWarrior)
        {
            if (iconChoose == 1) icon = iconSwordShort;
            if (iconChoose == 2) icon = iconSwordLong;
            if (iconChoose == 3) icon = iconShield;
        }
        else
        {
            if (iconChoose == 1) icon = iconMageShort;
            if (iconChoose == 2) icon = iconFireball;
        }

        GameObject go = Instantiate(cooldownSlotPrefab,
            isP1 ? cooldownContainerP1 : cooldownContainerP2);
        var slot = go.GetComponent<CooldownSlot>();
        slot.Initialize(icon, duration);
    }
}
