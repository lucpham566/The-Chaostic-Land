﻿using Spine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerCharacter playerCharacter;
    public PlayerRangeTarget playerRangeTarget;
    public PlayerRangeInteract playerRangeInteract;
    public InventoryManager inventoryManager;
    public CharacterAnimator characterAnimator;

    public GameObject targetIcon;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float slideSpeed = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool controlEnable = true;

    public int combo;
    public int comboNumber;
    public bool attacking;

    public float comboTiming;
    public float comboTempo;

    public GameObject attackPrefab; // vung tan cong duocc tao ra
    public List<GameObject> attackPrefabs; // vung tan cong duocc tao ra
    public float attackDuration = 0.5f; // thoi gian ton tai vung tan cong
    public float attackDeley = 0.5f; // thoi gian hồi chiêu
    public float TocDoRaDon = 0.2f; // thoi gian hồi chiêu
    public float bulletSpeed = 10f; // thoi gian hồi chiêu

    public GameObject bulletPrefab;
    public GameObject bulletMagicPrefab;

    private void Start()
    {

        playerCharacter = GetComponent<PlayerCharacter>();
        characterAnimator = GetComponent<CharacterAnimator>();

        combo = 1;
        comboTiming = 0.5f;
        comboTempo = comboTiming;
        comboNumber = 2;
    }

    private void Update()
    {
        if (controlEnable)
        {
            // tấn công

            Combo();
            // Kiểm tra xem có đang target hay không
            if (playerRangeTarget.currentTarget != null)
            {
                targetIcon.SetActive(true);
                Collider2D objectCollider = playerRangeTarget.currentTarget.GetComponent<Collider2D>();
                Transform objectTransform = playerRangeTarget.currentTarget.GetComponent<Transform>();
                float objectHeight = objectCollider.bounds.size.y;
                targetIcon.transform.position = new Vector3(objectTransform.position.x, objectTransform.position.y + objectHeight / 2 + 0.5f, objectTransform.position.z);
            }
            else
            {
                targetIcon.SetActive(false);
            }

            // Kiểm tra xem nhân vật có đứng trên mặt đất hay không

            if (!playerCharacter.isDashing)
            {
                // Di chuyển trái phải
                float moveInput = 0;
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    moveInput = 1;
                }
                else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    moveInput = -1;
                }
                else
                {
                    moveInput = 0;
                }

                playerCharacter.Move(moveInput);


            }

            // Nhảy
            if (Input.GetButtonDown("Jump"))
            {
                if (playerCharacter.isGrounded)
                {
                    //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    playerCharacter.Jump();
                }
            }

            // Tấn công
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }

            // Đổi mục tiêu
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("nhấn chuốt phải nè đm");
                playerCharacter.Defend();
            }

            if (Input.GetMouseButtonUp(1))
            {
                playerCharacter.DefendCancel();
            }

            // lướt
            if (Input.GetKeyDown(KeyCode.LeftShift) && playerCharacter.canDash && !playerCharacter.isAttacking)
            {
                StartCoroutine(playerCharacter.Dash());
            }

            // Đổi mục tiêu
            if (Input.GetKeyDown(KeyCode.X))
            {
                playerRangeTarget.SwipeTarget();
            }

            // Tương tác
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
    }

    private IEnumerator Slide()
    {
        playerCharacter.isSliding = true;
        playerCharacter.slide();

        yield return new WaitForSeconds(1f);
        playerCharacter.isSliding = false;

    }

    public void Attack()
    {
        if (playerRangeTarget.currentTarget)
        {
            playerCharacter.setFaceToTarget(playerRangeTarget.currentTarget.transform.position);
        }
    }

    public void Interact()
    {
        if (playerRangeTarget.currentTarget != null)
        {
            Debug.Log("zo item nè " + playerRangeInteract.CheckIntertactable());
            if (playerRangeInteract.CheckIntertactable())
            {
                Intertactive();
            }
            else
            {
                Debug.Log("không trong tầm tương tác");
            }
        }
        else
        {
            Debug.Log("không có mục tiêu nào");
        }
    }

    public void Intertactive()
    {

        if (playerRangeTarget.currentTarget.GetComponent<EnemyCharacter>() != null)
        {

        }
        else if (playerRangeTarget.currentTarget.GetComponent<Item>() != null)
        {
            Debug.Log("Xử lý item nè");

            Item item = playerRangeTarget.currentTarget.GetComponent<Item>();
            item.PickUpItem(transform.position);

            inventoryManager.AddItem(item.item, 1);
        }
    }

    public void Combo()
    {
        if (!playerCharacter.isAttacking)
        {
            comboTempo -= Time.deltaTime;
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                StartCoroutine(AttackCombo());
            }
        }
    }


    private IEnumerator AttackCombo()
    {
        attacking = true;
        checkJobAttackProperties();

        if (comboTempo < 0)
        {
            playerCharacter.Attack();
            playerCharacter.isAttacking = true;
            combo = 1;
            characterAnimator.ChangeAnimation("Attack" + combo);
            comboTempo = comboTiming;
            yield return new WaitForSeconds(TocDoRaDon);
            GenAttackByJob();

        }
        else if (comboTempo > 0 && comboTempo < 1)
        {
            playerCharacter.Attack();

            playerCharacter.isAttacking = true;
            combo++;
            if (combo > comboNumber)
            {
                combo = 1;
            }

            characterAnimator.ChangeAnimation("Attack" + combo);
            comboTempo = comboTiming;
            yield return new WaitForSeconds(TocDoRaDon);
            GenAttackByJob();
        }

        yield return new WaitForSeconds(attackDeley);
        playerCharacter.isAttacking = false;
    }

    private void GenAttackByJob()
    {
        switch (characterAnimator.AccGE.Job)
        {
            case Jobs.Warrior:
                GenAttackChienBinh();
                break;
            case Jobs.Archer:
                GenAttackCung();
                break;
            case Jobs.Elementalist:
                GenAttackPhep();
                break;
            default:
                GenAttackChienBinh();
                break;
        }
    }

    private void checkJobAttackProperties()
    {
        switch (characterAnimator.AccGE.Job)
        {
            case Jobs.Warrior:
                attackDeley = 0.3f; // thoi gian hồi chiêu
                TocDoRaDon = 0.2f; // thoi gian hồi chiêu
                break;
            case Jobs.Archer:
                attackDeley = 0.5f; // thoi gian hồi chiêu
                TocDoRaDon = 0.5f; // thoi gian hồi chiêu
                bulletSpeed = 15;
                break;
            case Jobs.Elementalist:
                attackDeley = 0.4f; // thoi gian hồi chiêu
                TocDoRaDon = 0.3f; // thoi gian hồi chiêu
                bulletSpeed = 5;
                break;
            default:
                attackDeley = 0.5f; // thoi gian hồi chiêu
                TocDoRaDon = 0.2f; // thoi gian hồi chiêu
                break;
        }
    }

    private void GenAttackCung()
    {
        Transform ArrowStartingTransform = transform.Find("AttackSpawnPoint").Find("FirePoint_Shoot1");
        GameObject newBullet = Instantiate(bulletPrefab, ArrowStartingTransform.position, ArrowStartingTransform.rotation);

        // Lấy Rigidbody2D của viên đạn để thiết lập tốc độ
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        PlayerDamage damage = newBullet.GetComponent<PlayerDamage>();
        damage.damge = playerCharacter.Damage;
        damage.destroyWhenCollide = true;

        if (rb != null)
        {
            if (playerCharacter.isFaceToRight())
            {
                rb.velocity = transform.right * bulletSpeed;
            }
            else
            {
                rb.velocity = transform.right * -bulletSpeed;
            }
        }

        // Hủy viên đạn sau một khoảng thời gian
        Destroy(newBullet, 3);
    }

    private void GenAttackPhep()
    {
        Transform ArrowStartingTransform = transform.Find("AttackSpawnPoint").Find("FirePoint_Shoot1");
        GameObject newBullet = Instantiate(bulletMagicPrefab, ArrowStartingTransform.position, ArrowStartingTransform.rotation);

        // Lấy Rigidbody2D của viên đạn để thiết lập tốc độ
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        PlayerDamage damage = newBullet.GetComponent<PlayerDamage>();
        damage.damge = playerCharacter.Damage;
        damage.destroyWhenCollide = true;

        if (rb != null)
        {
            if (playerCharacter.isFaceToRight())
            {
                rb.velocity = transform.right * bulletSpeed;
            }
            else
            {
                rb.velocity = transform.right * -bulletSpeed;
            }
        }

        // Hủy viên đạn sau một khoảng thời gian
        Destroy(newBullet, 3);
    }

    private void GenAttackChienBinh()
    {
        Collider2D myCollider = GetComponent<Collider2D>(); // Thay thế Collider2D bằng loại Collider bạn đang sử dụng
        Vector2 colliderExtents = myCollider.bounds.extents;

        // nếu quay trái thì trừ
        Vector3 spawnPosition = new Vector3(transform.position.x - colliderExtents.x, transform.position.y, transform.position.z);

        // nếu quay phải thì cộng
        if (playerCharacter.isFaceToRight())
        {
            spawnPosition = new Vector3(transform.position.x + colliderExtents.x, transform.position.y, transform.position.z);
        }
        attackPrefab = attackPrefabs[combo - 1];
        if (combo == 1)
        {

        }
        if (combo == 2)
        {

        }
        if (combo == 3)
        {

        }
        else
        {

        }
        GameObject attackObject = Instantiate(attackPrefab, spawnPosition, transform.rotation);
        PlayerDamage damage = attackObject.GetComponent<PlayerDamage>();
        damage.damge = playerCharacter.Damage;
        Destroy(attackObject, attackDuration);
    }
}
