using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [Header("Посилання на гравця")]
    public Player owner;

    [Header("Параметри персонажа")]
    public CharacterAbilities abilities;

    [Header("Довга атака?")]
    public bool isLongAtk;

    private int damage, health, longAttackMultiplier, critAdded;
    private float defense, critChanse;
    private double longDamage, damageForP1, damageForP2;
    private bool isP1, hasDealtDamage;

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
        longDamage = damage * 1.5;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasDealtDamage) return;

        Transform bodyTransform = owner.transform.Find("Hitbox");
        if (bodyTransform != null)
        {
            GameObject bodyObj = bodyTransform.gameObject;
            if (bodyObj.CompareTag("HitboxP1")) isP1 = true;
            if (bodyObj.CompareTag("HitboxP2")) isP1 = false;
        }

        if (isP1)
        {
            if (collision.CompareTag("HitboxP2"))
            {
                if (isLongAtk)
                {
                    damageForP1 = longDamage;
                    critAdded = Random.Range(2, 10);
                    if (Random.Range(0f, 1f) > critChanse) damageForP1 += critAdded;
                    HealthDefenseInitializer.Instance.ChangerHP2(damageForP1);
                }
                else
                {
                    damageForP1 = damage;
                    critAdded = Random.Range(1, 8);
                    if (Random.Range(0f, 1f) <= critChanse) damageForP1 += critAdded;
                    HealthDefenseInitializer.Instance.ChangerHP2(damageForP1);
                }

                hasDealtDamage = true;
            }
        }
        else
        {
            if (collision.CompareTag("HitboxP1"))
            {
                if (isLongAtk)
                {
                    damageForP2 = longDamage;
                    critAdded = Random.Range(2, 10);
                    if (Random.Range(0f, 1f) > critChanse) damageForP2 += critAdded;
                    HealthDefenseInitializer.Instance.ChangerHP1(damageForP2);
                }
                else
                {
                    damageForP2 = damage;
                    critAdded = Random.Range(1, 8);
                    if (Random.Range(0f, 1f) > critChanse) damageForP2 += critAdded;
                    HealthDefenseInitializer.Instance.ChangerHP1(damageForP2);
                }

                hasDealtDamage = true;
            }
        }
    }
}
