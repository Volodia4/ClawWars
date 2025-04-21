using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Атаки")]
    public PlayerAttack playerAttackShort;
    public PlayerAttack playerAttackLong;

    [Header("Здібності персонажа")]
    public CharacterAbilities abilities;

    [Header("Посилання на префаб фаербола")]
    public GameObject fireball;

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

    [Header("Назви звуків")]
    public string shortAtkSound;
    public string longAtkSound;

    [Header("Затримка перед звуками")]
    public float shortAtkSoundDelay;
    public float longAtkSoundDelay;

    [HideInInspector]
    public Transform otherPlayer;

    [HideInInspector]
    public KeyCode leftKey, rightKey, upKey, downKey, longAtkKey, shortAtkKey, shieldKey;

    [HideInInspector]
    public float facingDirection = 1f;

    private float currentShortAttackCooldown, currentLongAttackCooldown, currentShieldCooldown;
    private float speed, jumpForce, maxJumpHoldTime, jumpTime, delayBetweenActions;
    private bool canShield, hasFireball, isGrounded, isJumping, cantMove, isP1;
    private Animator animator;
    private AudioManager am;
    private AudioSource audioSource;
    private PolygonCollider2D polyCollider;
    private Vector2[] originalPoints;

    void Awake()
    {
        Transform hitboxTransform = transform.Find("Hitbox");
        polyCollider = hitboxTransform.GetComponent<PolygonCollider2D>();
        originalPoints = polyCollider.points;

        am = AudioManager.Instance;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        speed = abilities.speed;
        jumpForce = abilities.jumpForce;
        maxJumpHoldTime = abilities.maxJumpHoldTime;
        canShield = abilities.canShield;
        hasFireball = abilities.hasFireball;
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

        if (isGrounded && Mathf.Abs(moveInput) > 0.1f) am.PlayLoopingSFX("Running", audioSource);
        else am.StopLoopingSFX(audioSource);

        if (Input.GetKeyDown(upKey) && isGrounded && !cantMove) Jump();
        else if (Input.GetKey(upKey) && isJumping && !cantMove)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime < maxJumpHoldTime)
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * (1 + jumpTime));
        }
        else if (Input.GetKeyUp(upKey)) isJumping = false;

        if (animator != null)
        {
            if (!isGrounded)
            {
                animator.SetBool("isJumping", rb.velocity.y > 0f);
                animator.SetBool("isFalling", rb.velocity.y < 0f);
            }
            else
            {
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", false);
            }
        }

        if (Input.GetKeyDown(downKey) && delayBetweenActions <= 0f && isGrounded) Crouch();
        else if (Input.GetKeyUp(downKey)) Uncrouch();

        if (Input.GetKeyDown(shortAtkKey) && currentShortAttackCooldown <= 0f && delayBetweenActions <= 0f && !cantMove)
        {
            StartCoroutine(PlayAtkSound(shortAtkSound, shortAtkSoundDelay));

            animator.SetTrigger("shortAttack");
            StartCoroutine(playerAttackShort.AttackSequence());
            currentShortAttackCooldown = shortAttackCooldown;
            delayBetweenActions = shortAtkHoldTime;
        }

        if (Input.GetKeyDown(longAtkKey) && currentLongAttackCooldown <= 0f && delayBetweenActions <= 0f && !cantMove)
        {
            StartCoroutine(PlayAtkSound(longAtkSound, longAtkSoundDelay));

            animator.SetTrigger("longAttack");
            if (hasFireball) StartCoroutine(SpawnFireball());
            else StartCoroutine(playerAttackLong.AttackSequence());
            currentLongAttackCooldown = longAttackCooldown;
            delayBetweenActions = longAtkHoldTime;
        }

        if (canShield)
        {
            if (Input.GetKeyDown(shieldKey) && currentShieldCooldown <= 0f && delayBetweenActions <= 0f && !cantMove)
            {
                animator.SetBool("isShielding", true);
                cantMove = true;
                currentShieldCooldown = shieldCooldown;
                delayBetweenActions = shieldHoldTime;
            }
            if (Input.GetKey(shieldKey) && delayBetweenActions <= 0f)
            {
                animator.SetBool("isShielding", false);
                cantMove = false;
            }
            if (Input.GetKeyUp(shieldKey))
            {
                animator.SetBool("isShielding", false);
                cantMove = false;
                delayBetweenActions = 0.2f;
            }
        }
    }

    private void Jump()
    {
        am.PlaySFX("Jump");

        isJumping = true;
        jumpTime = 0f;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false;
    }

    private void Crouch()
    {
        animator.SetBool("isCrouching", true);
        cantMove = true;

        float minY = originalPoints[0].y;
        foreach (var p in originalPoints) if (p.y < minY) minY = p.y;

        Vector2[] newPoints = new Vector2[originalPoints.Length];
        for (int i = 0; i < originalPoints.Length; i++)
        {
            float offsetY = originalPoints[i].y - minY;
            newPoints[i] = new Vector2(originalPoints[i].x, minY + offsetY * 0.65f);
        }
        polyCollider.points = newPoints;
    }

    private void Uncrouch()
    {
        animator.SetBool("isCrouching", false);
        cantMove = false;
        polyCollider.points = originalPoints;
    }

    private void Flip(float desiredFacing)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * desiredFacing;
        transform.localScale = scale;
        facingDirection = desiredFacing;
    }

    private IEnumerator PlayAtkSound(string nameOfSound, float delay)
    {
        yield return new WaitForSeconds(delay);

        am.PlaySFX(nameOfSound);
    }

    private IEnumerator SpawnFireball()
    {
        yield return new WaitForSeconds(1);

        var start = transform.position + new Vector3(1f * facingDirection, -0.5f, 0f);
        var go = Instantiate(fireball, start, Quaternion.identity);

        Transform bodyTransform = transform.Find("Hitbox");
        GameObject bodyObj = bodyTransform.gameObject;
        if (bodyObj.CompareTag("HitboxP1")) isP1 = true;
        if (bodyObj.CompareTag("HitboxP2")) isP1 = false;

        var fb = go.GetComponent<Fireball>();
        fb.Initialize(isP1, facingDirection);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            am.PlaySFX("Fall");

            isGrounded = true;
            isJumping = false;

            if (Input.GetKey(upKey)) Jump();
            if (Input.GetKey(downKey) && delayBetweenActions <= 0f) Crouch();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }
}
