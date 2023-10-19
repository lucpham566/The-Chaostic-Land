using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillController : MonoBehaviour
{
    public List<SkillControl> skills;    

    // Hàm để chọn kỹ năng
    public void UseSelectedSkill(SkillControl selectedSkill)
    {
        if (selectedSkill != null)
        {
            selectedSkill.UseSkill();
            // Thực hiện logic sử dụng kỹ năng ở đây
            // Ví dụ: chạy animation, bắn đạn, áp dụng cooldown, vv.
            // Sau khi sử dụng xong, bạn có thể đặt selectedSkill về null.
        }
    }

}
