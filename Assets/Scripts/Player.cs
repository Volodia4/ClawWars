using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterAbilities abilities;
    public Rigidbody2D rb;
    public Transform otherPlayer;
    public bool isPlayer1;
    public KeyCode leftKey, rightKey, upKey, downKey, longAtkKey, shortAtkKey, shieldKey;

    private float jumpTime, facingDirection = 1f, speed, jumpForce, maxJumpHoldTime, maxHP, damage, defense;
    private bool isGrounded, isJumping, cantMove, canShield;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        BattleSceneManager manager = FindObjectOfType<BattleSceneManager>();
        if (manager != null)
        {
            abilities = isPlayer1 ? manager.characterAbilities[GameManager.Instance.selectedCharacter1]
                                  : manager.characterAbilities[GameManager.Instance.selectedCharacter2];
        }

        if (abilities != null)
        {
            speed = abilities.speed;
            jumpForce = abilities.jumpForce;
            maxJumpHoldTime = abilities.maxJumpHoldTime;
            maxHP = abilities.maxHP;
            damage = abilities.damage;
            defense = abilities.defense;
            canShield = abilities.canShield;
        }
    }

    void Update()
    {
        float moveInput = 0f;
        if (Input.GetKey(leftKey) && !cantMove) moveInput = -1f;
        if (Input.GetKey(rightKey) && !cantMove) moveInput = 1f;

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (otherPlayer != null)
        {
            float desiredFacing = (otherPlayer.position.x > transform.position.x) ? 1f : -1f;
            if (!isPlayer1) desiredFacing = -desiredFacing;
            if (desiredFacing != facingDirection) Flip(desiredFacing);
        }

        if (animator != null && isGrounded)
        {
            float effectiveFacing = isPlayer1 ? facingDirection : -facingDirection;
            bool runningFront = false, runningBack = false;
            if (moveInput != 0f)
            {
                if (moveInput * effectiveFacing > 0f) runningFront = true;
                else if (moveInput * effectiveFacing < 0f) runningBack = true;
            }
            animator.SetBool("isRunningFront", runningFront);
            animator.SetBool("isRunningBack", runningBack);
        }

        if (Input.GetKeyDown(upKey) && isGrounded && !cantMove)
        {
            isJumping = true;
            jumpTime = 0f;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }

        if (Input.GetKey(upKey) && isJumping && !cantMove)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime < maxJumpHoldTime)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * (1 + jumpTime));
            }
        }

        if (Input.GetKeyUp(upKey))
        {
            isJumping = false;
        }

        if (animator != null)
        {
            if (!isGrounded)
            {
                if (rb.velocity.y > 0f)
                {
                    animator.SetBool("isJumping", true);
                    animator.SetBool("isFalling", false);
                }
                else if (rb.velocity.y < 0f)
                {
                    animator.SetBool("isJumping", false);
                    animator.SetBool("isFalling", true);
                }
            }
            else
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", false);
            }
        }

        if (Input.GetKeyDown(downKey))
        {
            animator.SetBool("isCrouching", true);
            cantMove = true;
        }
        if (Input.GetKeyUp(downKey))
        {
            animator.SetBool("isCrouching", false);
            cantMove = false;
        }

        if (Input.GetKeyDown(longAtkKey)) animator.SetTrigger("longAttack");
        if (Input.GetKeyDown(shortAtkKey)) animator.SetTrigger("shortAttack");

        if (canShield && Input.GetKeyDown(shieldKey))
        {
            animator.SetBool("isShielding", true);
            cantMove = true;
        }
        if (canShield && Input.GetKeyUp(shieldKey))
        {
            animator.SetBool("isShielding", false);
            cantMove = false;
        }
    }

    public void FreezeAnimation()
    {
        GetComponent<Animator>().speed = 0f;
    }

    private void Flip(float desiredFacing)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * desiredFacing;
        transform.localScale = scale;
        facingDirection = desiredFacing;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
