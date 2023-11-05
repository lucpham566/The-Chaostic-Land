using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : NetworkBehaviour
{
    public float cooldownTimer;    // Biến thời gian đếm ngược
    protected bool isCooldown;
    public float cooldownTime=5;

    public GameObject bulletPrefab;  // Prefab của viên đạn
    public Transform firePoint;      // Vị trí bắn viên đạn
    public Transform playerTransform;      // Vị trí nhân vật
    public float bulletSpeed = 10f;  // Tốc độ viên đạn
    public float bulletLifetime = 5f; // Thời gian tồn tại của viên đạn (5 giây)

    public GameObject useSkillEffectPrefab;

    public PlayerCharacter playerCharacter;
    public PlayerSkillController playerSkillController;


    private void Start()
    {
    }
    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime; // Giảm cooldownTimer xuống theo thời gian thực

            // Đảm bảo cooldownTimer không nhỏ hơn 0
            cooldownTimer = Mathf.Max(cooldownTimer, 0f);
        }
    }
    public virtual void UseSkill()
    {
        if (!isCooldown)
        {
            // Thực hiện logic của kỹ năng ở đây
            cooldownTimer = cooldownTime;
            // Bắt đầu thời gian hồi chiêu
            StartCoroutine(Cooldown());
        }
        else
        {
            Debug.Log("Skill on cooldown.");
        }
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }

    public virtual IEnumerator SkillOvertime()
    {
        yield return new WaitForSeconds(0);
    }

}