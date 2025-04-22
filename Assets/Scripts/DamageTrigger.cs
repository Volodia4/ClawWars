using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [Header("Посилання на гравця")]
    public Player owner;

    [Header("Параметри персонажа")]
    public CharacterAbilities abilities;

    [Header("Довга атака?")]
    public bool isLongAtk;

    [Header("Відкидання")]
    public float knockbackForce;
    public float knockbackDuration;

    private int damage, health, longAttackMultiplier, critAdded;
    private float defense, critChanse, longDamage, damageForP1, damageForP2;
    private bool isP1, hasDealtDamage;
    private AudioManager am;
    private Rigidbody2D player1Rb, player2Rb;
    private Player playerScriptP1, playerScriptP2;

    private string[] soundNames = {
        "TakeDamage1", "TakeDamage2", "TakeDamage3",
        "TakeDamage4", "TakeDamage5", "TakeDamage6"
    };

    private void Awake()
    {
        am = AudioManager.Instance;

        player1Rb = GameObject.FindWithTag("Player1").GetComponent<Rigidbody2D>();
        player2Rb = GameObject.FindWithTag("Player2").GetComponent<Rigidbody2D>();

        playerScriptP1 = GameObject.FindWithTag("Player1").GetComponent<Player>();
        playerScriptP2 = GameObject.FindWithTag("Player2").GetComponent<Player>();
    }

    private void OnEnable()
    {
        hasDealtDamage = false;
    }

    private void Start()
    {
        damage = abilities.damage;
        health = abilities.maxHP;
        defense = abilities.defense;
        critChanse = abilities.critChanse;
        longDamage = damage * 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasDealtDamage) return;

        Transform bodyTransform = owner.transform.Find("Hitbox");
        GameObject bodyObj = bodyTransform.gameObject;
        if (bodyObj.CompareTag("HitboxP1")) isP1 = true;
        if (bodyObj.CompareTag("HitboxP2")) isP1 = false;

        if (isP1)
        {
            if (collision.CompareTag("HitboxP2"))
            {
                if (playerScriptP2.isShielding)
                {
                    playerScriptP2.DisableTemporarily(knockbackDuration);
                    am.PlaySFX("ShieldHit");
                    hasDealtDamage = true;
                    return;
                }

                player2Rb.velocity = Vector2.zero;
                Vector2 dir = ((Vector2)player2Rb.position - (Vector2)owner.transform.position).normalized;
                player2Rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
                playerScriptP2.DisableTemporarily(knockbackDuration);

                if (isLongAtk)
                {
                    damageForP1 = longDamage;
                    critAdded = Random.Range(2, 10);
                    if (Random.Range(0f, 1f) > critChanse) damageForP1 += critAdded;
                    HealthDefenseUpdater.Instance.ChangerHP2(damageForP1);
                }
                else
                {
                    damageForP1 = damage;
                    critAdded = Random.Range(1, 8);
                    if (Random.Range(0f, 1f) <= critChanse) damageForP1 += critAdded;
                    HealthDefenseUpdater.Instance.ChangerHP2(damageForP1);
                }

                am.PlaySFX(soundNames[Random.Range(0, soundNames.Length)]);

                hasDealtDamage = true;
            }
        }
        else
        {
            if (collision.CompareTag("HitboxP1"))
            {
                if (playerScriptP1.isShielding)
                {
                    playerScriptP1.DisableTemporarily(knockbackDuration);
                    am.PlaySFX("ShieldHit");
                    hasDealtDamage = true;
                    return;
                }

                player1Rb.velocity = Vector2.zero;
                Vector2 dir2 = ((Vector2)player1Rb.position - (Vector2)owner.transform.position).normalized;
                player1Rb.AddForce(dir2 * knockbackForce, ForceMode2D.Impulse);
                playerScriptP1.DisableTemporarily(knockbackDuration);

                if (isLongAtk)
                {
                    damageForP2 = longDamage;
                    critAdded = Random.Range(2, 10);
                    if (Random.Range(0f, 1f) > critChanse) damageForP2 += critAdded;
                    HealthDefenseUpdater.Instance.ChangerHP1(damageForP2);
                }
                else
                {
                    damageForP2 = damage;
                    critAdded = Random.Range(1, 8);
                    if (Random.Range(0f, 1f) > critChanse) damageForP2 += critAdded;
                    HealthDefenseUpdater.Instance.ChangerHP1(damageForP2);
                }

                am.PlaySFX(soundNames[Random.Range(0, soundNames.Length)]);

                hasDealtDamage = true;
            }
        }
    }
}
