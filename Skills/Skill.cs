using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string skillName;        // Tên của kỹ năng
    public Sprite icon;             // Biểu tượng của kỹ năng (hình ảnh)
    public float cooldownTime;      // Thời gian cooldown của kỹ năng
    public float cooldownTimer;    // Biến thời gian đếm ngược
    public AnimationClip animation; // Animation của kỹ năng

    private Control skillControl;

    private void Start()
    {
        // Truy cập script FireballSkill từ cùng một GameObject
        skillControl = GetComponent<Control>();
    }

    private void Update()
    {
        // Kiểm tra nếu cooldownTimer đang lớn hơn 0

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime; // Giảm cooldownTimer xuống theo thời gian thực

            // Đảm bảo cooldownTimer không nhỏ hơn 0
            cooldownTimer = Mathf.Max(cooldownTimer, 0f);
        }
    }

    // Hàm để sử dụng kỹ năng
    public void UseSkill()
    {
        skillControl = GetComponent<Control>();

        if (cooldownTimer > 0f)
        {
            Debug.LogWarning("Kỹ năng chưa được hồi");
        }
        else
        {
            if (skillControl != null)
            {
                // Gọi phương thức UseSkill của Control
                skillControl.UseSkill();

                // Đặt lại cooldownTimer sau khi sử dụng kỹ năng
                cooldownTimer = cooldownTime;

            }
            else
            {
                Debug.LogWarning("Không tìm thấy kỹ năng skillControl");
            }
        }
    }

}
