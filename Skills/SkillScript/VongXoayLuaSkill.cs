using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class VongXoayLuaSkill : SkillControl
{
    public NetworkObjectManager networkObjectManager;
    public void Start()
    {
        networkObjectManager = NetworkObjectManager.Instance;
    }
    public override void UseSkill()
    {
        Debug.Log("Sử dụng skill");
        if (!base.isCooldown)
        {
            StartCoroutine(SkillOvertime());
        }
        else
        {
            Debug.Log("Skill on cooldown.");
        }
    }
    public override IEnumerator SkillOvertime()
    {
        playerCharacter.Attack1();
        playerCharacter.isAttacking = true;
        Debug.Log("Runner.IsServer" + JsonUtility.ToJson(Runner));
        NetworkObject skillEffect = Runner.Spawn(useSkillEffectPrefab, playerTransform.position, playerTransform.rotation);
        networkObjectManager.DestroyNetworkObject(skillEffect, 1);

        base.UseSkill(); // Gọi hàm cơ sở trước khi thêm logic cụ thể
        yield return new WaitForSeconds(0.5f);

        // Thực hiện logic cụ thể cho FireballSkill ở đây
        Debug.Log("Casting fireball with damage: " + 10);
        NetworkObject newBullet = Runner.Spawn(bulletPrefab, firePoint.position, firePoint.rotation);

        // Lấy Rigidbody2D của viên đạn để thiết lập tốc độ
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (playerCharacter.isFaceToRight())
            {
                rb.velocity = transform.right * bulletSpeed;
            }
            else
            {
                rb.velocity = -transform.right * bulletSpeed;
            }
            // Thiết lập tốc độ của viên đạn
        }

        // Hủy viên đạn sau một khoảng thời gian
        networkObjectManager.DestroyNetworkObject(newBullet, bulletLifetime);
        playerCharacter.isAttacking = false;
        playerCharacter.isAttacking = false;
    }

}
