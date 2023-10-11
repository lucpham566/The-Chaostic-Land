using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public int MaxHealth;
    public int Health;
    public int Armor;
    public int Damage;

    public int initMaxHealth;
    public int initArmor;
    public int initDamage;

    public float moveSpeed = 2.0f; // Tốc độ di chuyển của quái
    public float movementThreshold = 0.1f; // Ngưỡng vận tốc để xem quái vật có đang di chuyển
    public float wallDistanceThreshold; // Khoảng cách tối thiểu giữa nhân vật và tường
    public float jumpForce = 5;
    public float slideForce = 5;
    public float moveX ; // Tốc độ di chuyển của quái


    // các trạng thái
    public bool dameable=true;

    public bool isMove;

    public bool isFalling;
    public bool isJumping;
    public bool isGrounded;
    public bool isSliding;
    public bool isAttacking;
    public bool moveEnable=true;

    public bool canDash = true;
    public bool isDashing = false;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    public float groundCheckDistance = 1f; // Khoảng cách kiểm tra từ chân nhân vật xuống mặt đất
    public LayerMask groundLayer; // Layer của mặt đất
    public float groundCheckRadius = 0.1f;
    public float maxFallHeight = 0.01f; // Mức cao tối đa trước khi đặt lại vị trí

    private Rigidbody2D rb2d;
    private Animator animator;
    private Transform enemyTransform;
    public SpawnDecreaseText spawnDecreaseText;
    public SpriteRenderer spriteRenderer;
    public PlayerAudio playerAudio;
    public EquipmentManager equipmentManager;
    [SerializeField] private TrailRenderer tr; // hiệu ứng lướt

    public PlayerCharacter(int health, int armor, int damage)
    {
        Health = health;
        Armor = armor;
        Damage = damage;
    }

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
        tr=GetComponent<TrailRenderer>();
        equipmentManager=GetComponent<EquipmentManager>();

       
        AddEquipmentStats();
    }

    private void FixedUpdate()
    {
        // Kiểm tra va chạm với nền đất
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity);
        Boolean check_grounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));
        // Nếu không có va chạm hoặc va chạm nằm quá thấp, đặt lại vị trí nhân vật
        if (check_grounded)
        {
            // Đặt lại vị trí nhân vật lên một mức cao cố định
            //transform.position = new Vector3(transform.position.x, maxFallHeight, transform.position.z);
            animator.SetBool("Grounded", true);
            isGrounded = true;
        }
        else
        {
            animator.SetBool("Grounded", false);
            isGrounded = false;
        }

        // Kiểm tra va chạm bên trái hoặc bên phải tường

        //RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallDistanceThreshold, LayerMask.GetMask("Ground"));
        //RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallDistanceThreshold, LayerMask.GetMask("Ground"));

        //if ((hitLeft.collider != null && moveX < 0) || (hitRight.collider != null && moveX > 0))
        //{
        // Nếu có va chạm ở bên trái và người chơi đang cố gắng đi sang trái, hoặc có va chạm ở bên phải và người chơi đang cố gắng đi sang phải
        // rb2d.velocity = new Vector2(0, rb2d.velocity.y); // Đặt vận tốc x về 0 để ngăn nhân vật di chuyển thêm về phía tường
        //}

    }

    private void Update()
    {
        CheckJumpingAndFalling();
        checkMoveEnable();
        if (IsMoving())
        {
            animator.SetBool("Move",true);
            setFaceTarget();
            isMove=true;
        }
        else
        {
            animator.SetBool("Move", false);
            isMove = false;
        }

    }

    public void Attack()
    {
        playerAudio.PlayClipOneShot(playerAudio.swordSlashSound);
        animator.SetTrigger("Attack");
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
        if (isAttacking)
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
        if (rb2d.velocity.x > 0) // Nếu vận tốc dương, đang di chuyển sang phải
        {
            spriteRenderer.flipX = false;
        }
        else if (rb2d.velocity.x < 0) // Nếu vận tốc âm, đang di chuyển sang trái
        {
            spriteRenderer.flipX = true;

        }
    }

    public bool isFaceToRight()
    {
        if (!spriteRenderer.flipX) // Nếu vận tốc dương, đang di chuyển sang phải
        {
            return true;
        }
        return false;
    }

    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        dameable = false;
        animator.SetBool("Dashing", true);
        float originalGravity = rb2d.gravityScale;
        rb2d.gravityScale = 0f;
        if (isFaceToRight())
        {
            rb2d.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        }
        else
        {
            rb2d.velocity = new Vector2(-transform.localScale.x * dashingPower, 0f);
        }

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
    public void AddEquipmentStats()
    {
        int HealthBonus = 0;
        int ArmorBonus = 0;
        int DamageBonus = 0;
        List<EquipmentClass> equipmentList = equipmentManager.equipmentList;
        foreach (var item in equipmentList)
        {
            HealthBonus += item.hp;
            ArmorBonus += item.physicsDefense;
            DamageBonus += item.physicsDamage;
        }

        Health = initMaxHealth+ HealthBonus;
        Armor = initArmor + ArmorBonus;
        Damage = initDamage + DamageBonus;
    }
}
