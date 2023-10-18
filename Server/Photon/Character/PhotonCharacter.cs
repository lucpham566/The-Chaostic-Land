using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChanges
{
    public int hpChange;
    public int maxHpChange;
    public int manaChange;
    public int maxManaChange;
    public int damageChange;
    public int ArmorChange;

    public StatChanges()
    {
        hpChange = 0;
        maxHpChange = 0;
        manaChange = 0;
        maxManaChange = 0;
        damageChange = 0;
    }
}


public class PhotonCharacter : MonoBehaviour
{
    [Networked] public string name = "name";
    [Networked] public int MaxHealth;
    [Networked] public int initMaxHealth;
    [Networked] public int Health;
    [Networked] public int MaxMana;
    [Networked] public int Mana;

    [Networked] public int Armor;
    [Networked] public int Damage;

    [Networked] public int initArmor;
    [Networked] public int initDamage;
    [Networked] public int baseArmor;

    [Networked] public float moveSpeed = 2.0f; // Tốc độ di chuyển của quái
    [Networked] public float movementThreshold = 0.1f; // Ngưỡng vận tốc để xem quái vật có đang di chuyển
    [Networked] public float wallDistanceThreshold; // Khoảng cách tối thiểu giữa nhân vật và tường
    [Networked] public float jumpForce = 5;
    [Networked] public float slideForce = 5;
    [Networked] public float moveX; // Tốc độ di chuyển của quái


    // các trạng thái
    [Networked] public bool dameable = true;

    [Networked] public bool isMove;

    [Networked] public bool isFalling;
    [Networked] public bool isJumping;
    [Networked] public bool isGrounded;
    [Networked] public bool isSliding;
    [Networked] public bool isAttacking;
    [Networked] public bool isDefend;
    [Networked] public bool moveEnable = true;

    [Networked] public bool canDash = true;
    [Networked] public bool isDashing = false;
    [Networked] public float dashingPower = 24f;
    [Networked] public float dashingTime = 0.2f;
    [Networked] public float dashingCooldown = 1f;
    [Networked] public float groundCheckDistance = 1f; // Khoảng cách kiểm tra từ chân nhân vật xuống mặt đất
    [Networked] public LayerMask groundLayer; // Layer của mặt đất
    [Networked] public float groundCheckRadius = 0.1f;
    [Networked] public float maxFallHeight = 0.01f; // Mức cao tối đa trước khi đặt lại vị trí

    // animation
    public CharacterAnimator characterAnimator;

    private Rigidbody2D rb2d;
    private Animator animator;
    private Transform enemyTransform;
    public SpawnDecreaseText spawnDecreaseText;
    public SpriteRenderer spriteRenderer;
    public PlayerAudio playerAudio;
    public EquipmentManager equipmentManager;
    public GearEquipper gearEquipper;
    public Transform transformCharacterGFX;
    [SerializeField] private TrailRenderer tr; // hiệu ứng lướt

    // ===== các hiệu ứng =============
    public int staminaMax; // choáng
    public int burnStaminaMax; //bỏng
    public int freezeStaminaMax; //tê cứng
    public int poisonStaminaMax; //trúng độc
    public int diseasesStaminaMax; // suy nhược

    public int stamina; // choáng
    public int burnStamina; //bỏng
    public int freezeStamina; //tê cứng
    public int poisonStamina; //trúng độc
    public int diseasesStamina; // suy nhược

    public bool isStune;
    public bool isBurn;
    public bool isFreeze;
    public bool isPoison;
    public bool isDiseases;

    public float staminaRecoveryRate = 10.0f;
    public bool isRecoveringStamina = false;


    public float takeDameTimer = 0;

    // ===== các hiệu ứng =============

    private void Awake()
    {
        initMaxHealth = MaxHealth;
        initArmor = Armor;
        initDamage = Damage;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAudio = GetComponent<PlayerAudio>();
        enemyTransform = transform;
        tr = GetComponent<TrailRenderer>();
        equipmentManager = GetComponent<EquipmentManager>();
        characterAnimator = GetComponent<CharacterAnimator>();

    }

    private void FixedUpdate()
    {
        // Kiểm tra va chạm với nền đất
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity);
        Boolean check_grounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));
        // Nếu không có va chạm hoặc va chạm nằm quá thấp, đặt lại vị trí nhân vật
        if (check_grounded)
        {
            animator.SetBool("Grounded", true);
            isGrounded = true;
        }
        else
        {
            animator.SetBool("Grounded", false);
            isGrounded = false;
        }

    }

    private void Update()
    {
        CheckJumpingAndFalling();
        checkMoveEnable();
        CheckDefend();
        if (IsMoving())
        {
            animator.SetBool("Move", true);
            setFaceTarget();
            isMove = true;
        }
        else
        {
            animator.SetBool("Move", false);
            isMove = false;
        }

    }

    public void Attack()
    {
       
    }

    public void CheckDefend()
    {
        if (isDefend)
        {
            Armor = baseArmor * 3;
        }
        else
        {
            Armor = baseArmor;
        }
    }
    public void Defend()
    {
        isDefend = true;
        characterAnimator.ChangeAnimation("Defence");
    }

    public void DefendCancel()
    {
        isDefend = false;
    }


    public void Jump()
    {
        if (isGrounded)
        {
            playerAudio.PlayClipOneShot(playerAudio.jumpSound);
            //rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f); // Đặt tốc độ y về 0 để tránh nhảy nhanh hơn khi đã ở trong không trung.
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (dameable)
        {
            int damageTaken = Mathf.Max(1, damage - Armor);
            Health -= damageTaken;
            animator.SetTrigger("Hurt");

            if (spawnDecreaseText != null)
            {
                string decreaseText = "-" + damageTaken;
                spawnDecreaseText.SpawnText(decreaseText);
            }

            if (Health <= 0)
            {
                Die();
            }
        }
    }

    protected void checkMoveEnable()
    {
        if (isAttacking || isDefend)
        {
            moveEnable = false;
        }
        else
        {
            moveEnable = true;
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{GetType().Name} has been defeated.");
        animator.SetTrigger("Die");
    }

    public void Move(float moveInput)
    {
        if (moveEnable)
        {
            // Di chuyển quái vật theo hướng direction với tốc độ moveSpeed
            moveX = moveInput;
            rb2d.velocity = new Vector2(moveInput * moveSpeed, rb2d.velocity.y);
        }
    }
    public bool IsMoving()
    {
        float absoluteVelocityX = Mathf.Abs(rb2d.velocity.x);
        animator.SetFloat("MoveVelocity", absoluteVelocityX);
        return absoluteVelocityX > movementThreshold;
    }

    bool IsGrounded()
    {
        Vector2 groundCheckPosition = transform.position;
        groundCheckPosition.y -= GetComponent<Collider2D>().bounds.extents.y;

        // Kiểm tra xem có collider nào đang nằm ở dưới nhân vật
        isGrounded = Physics2D.OverlapCircle(groundCheckPosition, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            // Nhân vật đang ở mặt đất
        }
        else
        {
            // Nhân vật không ở mặt đất
        }

        return isGrounded;
    }

    public void slide()
    {
        animator.SetTrigger("Sliding");
        playerAudio.PlayClipOneShot(playerAudio.swordSlashSound);
        if (!spriteRenderer.flipX) // Nếu vận tốc dương, đang di chuyển sang phải
        {
            //rb2d.velocity = new Vector2(slideForce, rb2d.velocity.y);
            rb2d.velocity = new Vector2(0f, rb2d.velocity.x); // Đặt tốc độ y về 0 để tránh nhảy nhanh hơn khi đã ở trong không trung.
            rb2d.AddForce(Vector2.right * slideForce, ForceMode2D.Impulse);
        }
        else // Nếu vận tốc âm, đang di chuyển sang trái
        {
            //rb2d.velocity = new Vector2(-slideForce, rb2d.velocity.y);
            rb2d.velocity = new Vector2(0f, rb2d.velocity.x); // Đặt tốc độ y về 0 để tránh nhảy nhanh hơn khi đã ở trong không trung.
            rb2d.AddForce(Vector2.right * -slideForce, ForceMode2D.Impulse);
        }
    }

    public void CheckJumpingAndFalling()
    {
        // Kiểm tra nếu quái vật đang nhảy (Jumping)
        if (rb2d.velocity.y > 0)
        {
            isJumping = true;
            isFalling = false;
            animator.SetBool("Jumping", true);
            animator.SetBool("Falling", false);
        }
        else if (rb2d.velocity.y < 0)
        {
            isJumping = false;
            isFalling = true;
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", true);
        }
        else
        {
            isJumping = false;
            isFalling = false;
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
        }

    }

    private void setFaceTarget()
    {
        // Kiểm tra hướng vận tốc và lật lại hình ảnh
        
    }

    public void setFaceToTarget(Vector3 targetPosition)
    {
       
    }

    public bool isFaceToRight()
    {
       
    }

    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        dameable = false;
        animator.SetBool("Dashing", true);
        float originalGravity = rb2d.gravityScale;
        rb2d.gravityScale = 0f;
        rb2d.velocity = new Vector2(dashingPower, 0f);

        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb2d.gravityScale = originalGravity;
        isDashing = false;
        dameable = true;
        animator.SetBool("Dashing", false);
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }


    public string GetName()
    {
        return name;
    }

    // Hồi phục thể lực 
    private void StartStaminaRecovery()
    {
        isRecoveringStamina = true;
        StartCoroutine(RecoverStaminaOverTime());
    }

    private IEnumerator RecoverStaminaOverTime()
    {
        while (stamina < staminaMax)
        {
            stamina += staminaRecoveryRate * Time.deltaTime;
            yield return null;
        }

        stamina = staminaMax;
        isRecoveringStamina = false;
    }

    private IEnumerator ModifyStatsOverTime(StatChanges changes, float time)
    {
        MaxHealth += changes.hpChange;
        MaxMana += changes.manaChange;
        Health += changes.hpChange;
        Mana += changes.manaChange;
        Damage += changes.damageChange;

        yield return new WaitForSeconds(time);
        MaxHealth -= changes.hpChange;
        MaxMana -= changes.manaChange;
        Damage -= changes.damageChange;
        if (Health >= MaxHealth)
        {
            Health = MaxHealth;
        }
        if (Mana <= MaxMana)
        {
            Mana = MaxMana;
        }
    }
    
    private IEnumerator StuneOverTime(float stuneTime)
    {
        isStune = true;
        yield return new WaitForSeconds(stuneTime);
        isStune = false;
    }

    
}
