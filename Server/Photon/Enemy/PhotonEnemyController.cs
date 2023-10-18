using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonEnemyController : NetworkBehaviour
{
    private Vector3 targetPosition; // Vị trí mục tiêu để quái vật di chuyển đến
    protected EnemyCharacter enemyCharactor;
    public GameObject playerTarget;

    public GameObject bulletPrefab; //Prefab cho viên đạn
    public float bulletSpeed = 10f; // Tốc độ viên đạn

    public float fireInterval = 2f; // Thời gian giữa mỗi lần bắn
    public float timeSinceLastShot = 0f;

    public float idleInterval = 5f; // Thời gian giữa mỗi lần bắn
    public float timeSinceLastIdle = 0f;


    private Rigidbody2D rb;

    private bool checkMoveRandom;
    public bool isPlayerInRangeAttack;
    private float randomX;

    private void Start()
    {
        enemyCharactor = GetComponent<EnemyCharacter>();
        rb = GetComponent<Rigidbody2D>();
        // Khởi tạo vị trí mục tiêu ban đầu
        checkMoveRandom = false;
        UpdateTargetPosition();
    }

    protected void Update()
    {
        if (enemyCharactor.isStune)
        {
            return;
        }

        timeSinceLastShot += Time.deltaTime;
        timeSinceLastIdle -= Time.deltaTime;

        if (!enemyCharactor.isDeath)
        {
            if (playerTarget != null)
            {
                if (isPlayerInRangeAttack)
                {
                    setFaceTarget();
                    // Kiểm tra nếu đủ thời gian giữa các lần bắn
                    if (timeSinceLastShot >= fireInterval)
                    {
                        enemyCharactor.Attack();
                        // Bắn viên đạn
                        StartCoroutine(DelayAction());

                        // Đặt lại thời gian kể từ lần bắn trước
                        timeSinceLastShot = 0f;
                    }

                }
                else
                {
                    MoveToTarget();
                }

            }
            else
            {
                if (enemyCharactor.isGrounded && timeSinceLastIdle<0)
                {
                    MoveRandom();
                    
                }
            }
           
        }
    }

    protected IEnumerator DelayAction()
    {
        // Chờ 2 giây
        yield return new WaitForSeconds(0.2f);

        // Đoạn code sau đây sẽ được thực thi sau khi chờ 2 giây
        FireBullet();
    }

    protected void FireBullet()
    {
        // Tạo một viên đạn từ prefab
        GameObject bullet = Runner.Spawn(bulletPrefab, transform.position, Quaternion.identity);

        // Tính toán hướng từ viên đạn đến mục tiêu
        Vector3 direction = (playerTarget.transform.position - transform.position).normalized;

        Rigidbody2D rb2d_bullet = bullet.GetComponent<Rigidbody2D>();
        // Di chuyển viên đạn theo hướng này
        rb2d_bullet.velocity = direction*bulletSpeed;
    }

    protected void MoveToTarget()
    {

        // Di chuyển quái vật đến vị trí mục tiêu
        targetPosition = playerTarget.transform.position;

        Vector3 moveDirection = (new Vector3(targetPosition.x, transform.position.y, transform.position.z) - transform.position);

        enemyCharactor.Move(moveDirection);
       
    }


    protected void MoveRandom()
    {
        checkMoveRandom = true;

        // Di chuyển quái vật đến vị trí mục tiêu
        targetPosition = new Vector3(randomX, transform.position.y, transform.position.z);

        Vector3 moveDirection = (targetPosition - transform.position);

        enemyCharactor.Move(moveDirection);
        // Kiểm tra nếu quái vật đã đến gần đủ với vị trí mục tiêu
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            UpdateTargetPosition();
            checkMoveRandom = false;
            timeSinceLastIdle = 5f;
        }
    }

    protected void UpdateTargetPosition()
    {
        // Tạo một vị trí ngẫu nhiên xung quanh vị trí hiện tại
         randomX = Random.Range(transform.position.x - 5f, transform.position.x + 5f);
    }

    protected void setFaceTarget()
    {
        // Kiểm tra hướng vận tốc và lật lại hình ảnh
        if (playerTarget != null) // Nếu vận tốc dương, đang di chuyển sang phải
        {
            if (playerTarget.transform.position.x - transform.position.x > 0) // Nếu vận tốc dương, đang di chuyển sang phải
            {
                enemyCharactor.setFaceRight();
            }
            else if (playerTarget.transform.position.x - transform.position.x < 0) // Nếu vận tốc âm, đang di chuyển sang trái
            {
                enemyCharactor.setFaceLeft();
            }
        }
       
    }

}