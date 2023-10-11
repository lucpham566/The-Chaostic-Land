using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireballSkill : MonoBehaviour, ISkillUser
{
    public GameObject bulletPrefab;  // Prefab của viên đạn
    public Transform firePoint;      // Vị trí bắn viên đạn
    public float bulletSpeed = 10f;  // Tốc độ viên đạn
    public float bulletLifetime = 5f; // Thời gian tồn tại của viên đạn (5 giây)

    void Start()
    {

    }

    void Update()
    {
       
    }

    public void UseSkill()
    {
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Lấy Rigidbody2D của viên đạn để thiết lập tốc độ
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Thiết lập tốc độ của viên đạn
            rb.velocity = transform.right * bulletSpeed;
        }

        // Hủy viên đạn sau một khoảng thời gian
        Destroy(newBullet, bulletLifetime);
    }
}