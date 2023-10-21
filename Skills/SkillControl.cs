using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : MonoBehaviour
{
    public float cooldownTimer;    // Biến thời gian đếm ngược
    protected bool isCooldown;
    public float cooldownTime=5;
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
}