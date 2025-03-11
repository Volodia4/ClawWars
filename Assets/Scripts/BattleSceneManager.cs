using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    [Header("Персонажі та арена")]
    public GameObject player1, player2, background;

    [Header("Аніматори персонажів")]
    public RuntimeAnimatorController[] animatorControllers;

    [Header("Параметри та особливості персонажів")]
    public CharacterAbilities[] characterAbilities;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            int charIndex1 = GameManager.Instance.selectedCharacter1;
            int charIndex2 = GameManager.Instance.selectedCharacter2;

            if (player1 != null) SetupCharacter(player1, charIndex1);
            if (player2 != null) SetupCharacter(player2, charIndex2);

            if (background != null)
            {
                SpriteRenderer bgSR = background.GetComponent<SpriteRenderer>();
                if (bgSR != null && GameManager.Instance.selectedArena != null)
                {
                    bgSR.sprite = GameManager.Instance.selectedArena;
                }
            }
        }
    }

    private void SetupCharacter(GameObject player, int charIndex)
    {
        if (player != null)
        {
            Animator anim = player.GetComponent<Animator>();
            if (anim != null && charIndex >= 0 && charIndex < animatorControllers.Length)
            {
                anim.runtimeAnimatorController = animatorControllers[charIndex];
            }

            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null && charIndex >= 0 && charIndex < characterAbilities.Length)
            {
                playerScript.abilities = characterAbilities[charIndex];
            }
        }
    }
}
