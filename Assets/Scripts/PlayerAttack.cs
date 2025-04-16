using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Кут нахилу")]
    public float rotationAngle;

    [Header("Рух по осі Y")]
    public float moveUp;

    [Header("Рух по осі X")]
    public float moveSide;

    [Header("Час виконання")]
    public float moveDuration;

    [Header("Затримка перед атакою")]
    public float delayBeforeAttack;

    [Header("Хітбокс атаки")]
    public GameObject attackHitbox;

    [Header("Час активності хітбокса атаки після завершення атаки")]
    public float activeDurationAfterAttack;

    [Header("Посилання на гравця")]
    public Player player;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    public IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(delayBeforeAttack);

        if (!gameObject.activeSelf) gameObject.SetActive(true);

        float facingDirection = player.facingDirection;

        Vector3 initialPos = transform.localPosition;
        Quaternion initialRot = transform.localRotation;

        Vector3 targetPos = initialPos + new Vector3(moveSide, moveUp, 0);
        Quaternion targetRot = Quaternion.Euler(0, 0, rotationAngle);

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            transform.localPosition  = Vector3.Lerp(initialPos, targetPos, elapsed / moveDuration);
            transform.localRotation  = Quaternion.Lerp(initialRot, targetRot, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = initialPos;
        transform.localRotation = initialRot;

        yield return new WaitForSeconds(activeDurationAfterAttack);
        attackHitbox.SetActive(false);
    }
}
