using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerCharacter playerCharacter;
    public PlayerRangeTarget playerRangeTarget;
    public PlayerRangeInteract playerRangeInteract;
    public InventoryManager inventoryManager;

    public GameObject targetIcon;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float slideSpeed = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Collider2D col;
    private bool isGrounded;
    private bool isSliding;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        playerCharacter = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        // Kiểm tra xem có đang target hay không
        if (playerRangeTarget.currentTarget != null)
        {
            targetIcon.SetActive(true);
            Collider2D objectCollider = playerRangeTarget.currentTarget.GetComponent<Collider2D>();
            Transform objectTransform = playerRangeTarget.currentTarget.GetComponent<Transform>();
            float objectHeight = objectCollider.bounds.size.y;
            targetIcon.transform.position = new Vector3(objectTransform.position.x, objectTransform.position.y+objectHeight/2 + 0.5f, objectTransform.position.z) ;
        }
        else
        {
            targetIcon.SetActive(false);
        }

        // Kiểm tra xem nhân vật có đứng trên mặt đất hay không
        isGrounded = playerCharacter.isGrounded;

        if (!playerCharacter.isDashing)
        {
            // Di chuyển trái phải
            float moveInput = Input.GetAxis("Horizontal");
            if (moveInput>0)
            {
                playerCharacter.Move(1);
            }
            else if (moveInput < 0)
            {
                playerCharacter.Move(-1);
            }
            else
            {
                playerCharacter.Move(0);

            }

        }

        // Nhảy
        if ( Input.GetButtonDown("Jump"))
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

        // lướt
        if (Input.GetKeyDown(KeyCode.LeftShift) && playerCharacter.canDash)
        {
            StartCoroutine(playerCharacter.Dash());
        }

        // Đổi mục tiêu
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerRangeTarget.SwipeTarget();
        }

        // Đổi mục tiêu
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        playerCharacter.slide();

        yield return new WaitForSeconds(1f);
        isSliding = false;

    }

        private void Attack()
    {
        // Viết mã xử lý tấn công ở đây
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

            inventoryManager.AddItem(item.item,1);
        }
    }


}
