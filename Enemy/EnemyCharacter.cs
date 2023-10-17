using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour, ITargetable
{
    public string name = "enemy";
    public int MaxHealth;
    public int Health;
    public int Armor;
    public int ArmorMagic;
    public int Damage;
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

    public float moveSpeed = 2.0f; // Tốc độ di chuyển của quái
    public float movementThreshold = 0.1f; // Ngưỡng vận tốc để xem quái vật có đang di chuyển
    public float maxFallHeight = 0.01f; // Mức cao tối đa trước khi đặt lại vị trí
    public float groundCheckDistance = 1f; // Khoảng cách kiểm tra từ chân nhân vật xuống mặt đất
    public float delayInSecondsDie;
    public float jumpForce = 5;
    public float slideForce = 5;

    // các trạng thái
    public bool isAttack;
    public bool isMove;
    public bool isJumping;
    public bool isFalling;
    public bool isGrounded;
    public bool isDeath;

    public Rigidbody2D rb;
    public Animator animator;
    public Transform enemyTransform;
    public SpriteRenderer spriteRenderer;
    public SpawnDecreaseText spawnDecreaseText;
    public EnemyAudio enemyAudio;
    public ItemClass[] possibleDrops; // Danh sách các vật phẩm có thể rơi từ quái.
    public GameObject itemPrefab; // Danh sách các vật phẩm có thể rơi từ quái.
    public EnemyClass enemyClass; // Danh sách các vật phẩm có thể rơi từ quái.


    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = enemyClass.animationController;
        MaxHealth = enemyClass.maxHP;
        Health = enemyClass.maxHP;
        Armor = enemyClass.armor;
        Damage = enemyClass.damage;

        staminaRecoveryRate = staminaMax / 100;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyAudio = GetComponent<EnemyAudio>();
        enemyTransform = transform;
        isGrounded = false;
        isDeath = false;
    }


    private void FixedUpdate()
    {
        bool check_grounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));
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
    }

    private void Update()
    {
        CheckJumpingAndFalling();
        if (takeDameTimer > 0)
        {
            takeDameTimer -= Time.deltaTime;
            if (stamina < staminaMax)
            {
                StartStaminaRecovery();
            }
        }
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

    public string GetName()
    {
        return name;
    }


    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public virtual void TakeDamage(int damage)
    {
        if (!isDeath)
        {
            int damageTaken = Mathf.Max(1, damage - Armor);
            Health -= damageTaken;

            if (spawnDecreaseText != null)
            {
                string decreaseText = "-" + damageTaken;
                spawnDecreaseText.SpawnText(decreaseText);
            }


            if (Health <= 0)
            {
                Die();
            }
            else
            {
                animator.SetTrigger("Hurt");
            }
        }
    }

    protected virtual void Die()
    {
        isDeath = true;
        animator.SetBool("Die", true);
        Destroy(gameObject, delayInSecondsDie);
        DropItem();
    }


    public void Move(Vector3 direction)
    {
        if (isGrounded)
        {
            // Di chuyển quái vật theo hướng direction với tốc độ moveSpeed
            rb.velocity = direction.normalized * 2;
        }
    }
    public bool IsMoving()
    {
        return rb.velocity.magnitude > movementThreshold;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            //rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            rb.velocity = new Vector2(rb.velocity.x, 0f); // Đặt tốc độ y về 0 để tránh nhảy nhanh hơn khi đã ở trong không trung.
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    bool IsGrounded()
    {
        float raycastDistance = 0.1f; // Độ dài của raycast từ quái vật xuống
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f; // Vị trí bắt đầu raycast
        Vector3 raycastDirection = Vector3.down; // Hướng raycast (xuống)

        if (Physics.Raycast(raycastOrigin, raycastDirection, raycastDistance))
        {
            return true; // Quái vật đang ở mặt đất
        }

        return false; // Quái vật không ở mặt đất
    }

    public void CheckJumpingAndFalling()
    {
        // Kiểm tra nếu quái vật đang nhảy (Jumping)
        if (rb.velocity.y > 0)
        {
            isJumping = true;
            isFalling = false;
            animator.SetBool("Jumping", true);
            animator.SetBool("Falling", false);
        }
        else if (rb.velocity.y < 0)
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

    public void Slide()
    {
        animator.SetTrigger("Sliding");
        if (!spriteRenderer.flipX) // Nếu vận tốc dương, đang di chuyển sang phải
        {
            rb.velocity = new Vector2(slideForce, rb.velocity.y);
        }
        else // Nếu vận tốc âm, đang di chuyển sang trái
        {
            rb.velocity = new Vector2(-slideForce, rb.velocity.y);
        }
    }

    private void setFaceTarget()
    {
        // Kiểm tra hướng vận tốc và lật lại hình ảnh
        if (rb.velocity.x > 0) // Nếu vận tốc dương, đang di chuyển sang phải
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.velocity.x < 0) // Nếu vận tốc âm, đang di chuyển sang trái
        {
            spriteRenderer.flipX = true;

        }
    }

    public void setFaceRight()
    {
        spriteRenderer.flipX = false;
    }

    public void setFaceLeft()
    {
        spriteRenderer.flipX = true;
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

    // các trạng thái hiệu ứng
    public void Stune(float stuneTime)
    {
        isStune = true;
        StartCoroutine(RecoverStaminaOverTime(stuneTime));
    }

    private IEnumerator StuneOverTime(float stuneTime)
    {
        yield return new WaitForSeconds(stuneTime);

        isStune = false;
    }

    public void ReducedArmor(float time, int amountRate)
    {
        StartCoroutine(ReducedArmorOverTime(time, amountRate));
    }

    private IEnumerator ReducedArmorOverTime(float time, int amountRate)
    {
        int ArmorGiam = Armor/100*amountRate;
        Armor -= ArmorGiam;
        yield return new WaitForSeconds(time);
        Armor += ArmorGiam;
    }

    private void DropItem()
    {
        if (possibleDrops.Length > 0)
        {
            int randomIndex = Random.Range(0, possibleDrops.Length);
            ItemClass droppedItem = possibleDrops[randomIndex];

            // Tạo ra một thể hiện của vật phẩm và đặt nó tại vị trí của quái.
            GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            Item item = newItem.GetComponent<Item>();
            item.item = droppedItem;

            Debug.Log("rơi đồ nè");
        }
    }
}