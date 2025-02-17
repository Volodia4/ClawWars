using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    [Header("Персонажі та арена")]
    public GameObject player1, player2, background;

    [Header("Аніматори персонажів")]
    public RuntimeAnimatorController[] playerAnimators;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            int char1Index = GameManager.Instance.selectedCharacter1;
            int char2Index = GameManager.Instance.selectedCharacter2;

            if (player1 != null)
            {
                Animator anim1 = player1.GetComponent<Animator>();
                if (anim1 != null && playerAnimators != null && playerAnimators.Length > char1Index)
                {
                    anim1.runtimeAnimatorController = playerAnimators[char1Index];
                }
            }

            if (player2 != null)
            {
                Animator anim2 = player2.GetComponent<Animator>();
                if (anim2 != null && playerAnimators != null && playerAnimators.Length > char2Index)
                {
                    anim2.runtimeAnimatorController = playerAnimators[char2Index];
                }
            }

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
}
