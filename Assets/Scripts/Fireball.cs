using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("Швидкість фаерболу")]
    public float speed;

    [Header("Рух фаерболу по осі X")]
    public float moveSide;

    [Header("Рух фаерболу по осі Y")]
    public float moveUp;

    [Header("Здібності мага")]
    public CharacterAbilities abilities;

    [Header("Назва звуків")]
    public string FireballShoot;
    public string FireballExplode;

    private string[] soundNames = {"TakeDamage1", "TakeDamage2", "TakeDamage3",
        "TakeDamage4", "TakeDamage5", "TakeDamage6"};
    private float fireballDamage, fireballCritChance, critAdded;
    private bool isPlayer1, hasDealtDamage;
    private Vector3 targetPos;
    private Animator animator;
    private AudioManager am;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        am = AudioManager.Instance;
        am.PlaySFX(FireballShoot);
    }

    private void Start()
    {
        fireballDamage = abilities.damage * 1.5f;
        fireballCritChance = abilities.critChanse;
    }

    public void Initialize(bool isP1, float facingDirection)
    {
        isPlayer1 = isP1;

        if (facingDirection < 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }

        var start = transform.position;
        targetPos  = start + new Vector3(moveSide * facingDirection, moveUp, 0f);

        animator.Play("Start");
        StartCoroutine(PlayFlyAfterDelay());

        StartCoroutine(MoveToTarget());
    }

    private IEnumerator PlayFlyAfterDelay()
    {
        yield return new WaitForSeconds(0.35f);
        if (!hasDealtDamage)  animator.Play("Fly");
    }

    private IEnumerator MoveToTarget()
    {

        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(PlayDestroyAndDie());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasDealtDamage) return;

        if (isPlayer1)
        {
            if (collision.CompareTag("HitboxP2"))
            {
                float damageForP1 = fireballDamage;
                critAdded = Random.Range(2, 10);
                if (Random.Range(0f, 1f) > fireballCritChance) damageForP1 += critAdded;
                HealthDefenseUpdater.Instance.ChangerHP2(damageForP1);

                hasDealtDamage = true;

                targetPos = transform.position;

                am.PlaySFX(soundNames[Random.Range(0, soundNames.Length)]);
            }
        }
        else
        {
            if (collision.CompareTag("HitboxP1"))
            {
                float damageForP2 = fireballDamage;
                critAdded = Random.Range(2, 10);
                if (Random.Range(0f, 1f) > fireballCritChance) damageForP2 += critAdded;
                HealthDefenseUpdater.Instance.ChangerHP1(damageForP2);

                hasDealtDamage = true;

                targetPos = transform.position;

                am.PlaySFX(soundNames[Random.Range(0, soundNames.Length)]);
            }
        }
    }

    private IEnumerator PlayDestroyAndDie()
    {
        am.PlaySFX(FireballExplode);

        animator.Play("Destroy");
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }
}
