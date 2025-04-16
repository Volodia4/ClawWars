using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Атаки")]
    public PlayerAttack playerAttackShort;
    public PlayerAttack playerAttackLong;

    [Header("Здібності персонажа")]
    public CharacterAbilities abilities;

    [Header("Фізика")]
    public Rigidbody2D rb;

    [Header("Cooldown для атак")]
    public float shortAttackCooldown;
    public float longAttackCooldown;
    public float shieldCooldown;

    [Header("Час тривання атак")]
    public float shortAtkHoldTime;
    public float longAtkHoldTime;
    public float shieldHoldTime;

    [HideInInspector]
    public Transform otherPlayer;

    [HideInInspector]
    public KeyCode leftKey, rightKey, upKey, downKey, longAtkKey, shortAtkKey, shieldKey;

    [HideInInspector]
    public float facingDirection = 1f;

    private float currentShortAttackCooldown, currentLongAttackCooldown, currentShieldCooldown;
    private float speed, jumpForce, maxJumpHoldTime, jumpTime, delayBetweenActions;
    private bool canShield, isGrounded, isJumping, cantMove;
    private Animator animator;
    private PolygonCollider2D polyCollider;
    private Vector2[] originalPoints;

    void Awake()
    {
        Transform hitboxTransform = transform.Find("Hitbox");

        polyCollider = hitboxTransform.GetComponent<PolygonCollider2D>();
        originalPoints = polyCollider.points;
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        if (abilities != null)
        {
            speed = abilities.speed;
            jumpForce = abilities.jumpForce;
            maxJumpHoldTime = abilities.maxJumpHoldTime;
            canShield = abilities.canShield;
        }
    }

    void Update()
    {
        if (Mathf.Approximately(Time.timeScale, 0f)) return;

        if (currentShortAttackCooldown > 0f) currentShortAttackCooldown -= Time.deltaTime;
        if (currentLongAttackCooldown > 0f) currentLongAttackCooldown -= Time.deltaTime;
        if (currentShieldCooldown > 0f) currentShieldCooldown -= Time.deltaTime;
        if (delayBetweenActions > 0f) delayBetweenActions -= Time.deltaTime;

        float moveInput = 0f;
        if (!cantMove)
        {
            if (Input.GetKey(leftKey)) moveInput = -1f;
            if (Input.GetKey(rightKey)) moveInput = 1f;
        }
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (otherPlayer != null)
        {
            float desiredFacing = (otherPlayer.position.x > transform.position.x) ? 1f : -1f;
            if (desiredFacing != facingDirection) Flip(desiredFacing);
        }

        if (animator != null && isGrounded)
        {
            bool runningFront = (moveInput != 0f && moveInput * facingDirection > 0f);
            bool runningBack = (moveInput != 0f && moveInput * facingDirection < 0f);
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
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * (1 + jumpTime));
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

        if (Input.GetKeyDown(downKey) && delayBetweenActions <= 0f && isGrounded)
        {
            animator.SetBool("isCrouching", true);
            cantMove = true;

            float minY = originalPoints[0].y;
            for (int i = 1; i < originalPoints.Length; i++)
            {
                if (originalPoints[i].y < minY)
                    minY = originalPoints[i].y;
            }

            Vector2[] newPoints = new Vector2[originalPoints.Length];
            for (int i = 0; i < originalPoints.Length; i++)
            {
                float offsetY = originalPoints[i].y - minY;
                newPoints[i] = new Vector2(originalPoints[i].x, minY + offsetY * 0.65f);
            }
            polyCollider.points = newPoints;
        }
        if (Input.GetKeyUp(downKey))
        {
            animator.SetBool("isCrouching", false);
            cantMove = false;

            polyCollider.points = originalPoints;
        }

        if (Input.GetKeyDown(shortAtkKey) && currentShortAttackCooldown <= 0f
            && delayBetweenActions <= 0f && !cantMove)
        {
            animator.SetTrigger("shortAttack");
            StartCoroutine(playerAttackShort.AttackSequence());
            currentShortAttackCooldown = shortAttackCooldown;
            delayBetweenActions = shortAtkHoldTime;
        }

        if (Input.GetKeyDown(longAtkKey) && currentLongAttackCooldown <= 0f
            && delayBetweenActions <= 0f && !cantMove)
        {
            animator.SetTrigger("longAttack");
            StartCoroutine(playerAttackLong.AttackSequence());
            currentLongAttackCooldown = longAttackCooldown;
            delayBetweenActions = longAtkHoldTime;
        }

        if (canShield)
        {
            if (Input.GetKeyDown(shieldKey) && currentShieldCooldown <= 0f
                && delayBetweenActions <= 0f && !cantMove)
            {
                animator.SetBool("isShielding", true);
                cantMove = true;
                currentShieldCooldown = shieldCooldown;
                delayBetweenActions = shieldHoldTime;
            }
            if (Input.GetKey(shieldKey))
            {
                if (delayBetweenActions <= 0f)
                {
                    animator.SetBool("isShielding", false);
                    cantMove = false;
                }
            }
            if (Input.GetKeyUp(shieldKey))
            {
                delayBetweenActions = 0.2f;
                animator.SetBool("isShielding", false);
                cantMove = false;
            }
        }
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
            isGrounded = false;
    }
}
