using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAbilities", menuName = "Character/Abilities")]
public class CharacterAbilities : ScriptableObject
{
    [Header("Основні параметри")]
    public float speed;
    public float jumpForce;
    public float maxJumpHoldTime;
    public int maxHP;
    public int damage;
    public float defense;
    [Range(0, 1)] public float critChanse;

    [Header("Особливості персонажа")]
    public bool canShield;
}
