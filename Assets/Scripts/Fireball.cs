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

    [Header("Відкидання")]
    public float knockbackForce;
    public float knockbackDuration;

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
    private Rigidbody2D player1Rb, player2Rb;
    private Player playerScriptP1, playerScriptP2;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        am = AudioManager.Instance;
        am.PlaySFX(FireballShoot);

        player1Rb = GameObject.FindWithTag("Player1").GetComponent<Rigidbody2D>();
        player2Rb = GameObject.FindWithTag("Player2").GetComponent<Rigidbody2D>();

        playerScriptP1 = GameObject.FindWithTag("Player1").GetComponent<Player>();
        playerScriptP2 = GameObject.FindWithTag("Player2").GetComponent<Player>();
    }

    private void Start()
    {
        fireballDamage = abilities.damage * 1.5f;
        fireballCritChance = abilities.critChanse;
    }

    public void Initialize(bool isP1, bool isLowerFireball, float facingDirection)
    {
        isPlayer1 = isP1;

        if (facingDirection < 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }

        var start = transform.position;
        if (isLowerFireball) moveUp = moveUp - 3;
        targetPos = start + new Vector3(moveSide * facingDirection, moveUp, 0f);

        animator.Play("Start");
        StartCoroutine(PlayFlyAfterDelay());

        StartCoroutine(MoveToTarget());
    }

    private IEnumerator PlayFlyAfterDelay()
    {
        yield return new WaitForSeconds(0.35f);
        if (!hasDealtDamage) animator.Play("Fly");
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
                targetPos = transform.position;

                player2Rb.velocity = Vector2.zero;
                Vector2 dir = ((Vector2)player2Rb.position - (Vector2)player1Rb.position).normalized;
                player2Rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

                if (playerScriptP2.isShielding)
                {
                    playerScriptP2.DisableTemporarily(knockbackDuration);
                    am.PlaySFX("ShieldHit");
                    return;
                }
                playerScriptP2.DisableTemporarily(knockbackDuration);

                float damageForP1 = fireballDamage;
                critAdded = Random.Range(2, 10);
                if (Random.Range(0f, 1f) > fireballCritChance) damageForP1 += critAdded;
                HealthDefenseUpdater.Instance.ChangerHP2(damageForP1);

                am.PlaySFX(soundNames[Random.Range(0, soundNames.Length)]);

                hasDealtDamage = true;
            }
        }
        else
        {
            if (collision.CompareTag("HitboxP1"))
            {
                targetPos = transform.position;

                player1Rb.velocity = Vector2.zero;
                Vector2 dir = ((Vector2)player1Rb.position - (Vector2)player2Rb.position).normalized;
                player1Rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

                if (playerScriptP1.isShielding)
                {
                    playerScriptP1.DisableTemporarily(knockbackDuration);
                    am.PlaySFX("ShieldHit");
                    return;
                }
                playerScriptP1.DisableTemporarily(knockbackDuration);

                float damageForP2 = fireballDamage;
                critAdded = Random.Range(2, 10);
                if (Random.Range(0f, 1f) > fireballCritChance) damageForP2 += critAdded;
                HealthDefenseUpdater.Instance.ChangerHP1(damageForP2);

                am.PlaySFX(soundNames[Random.Range(0, soundNames.Length)]);

                hasDealtDamage = true;
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
