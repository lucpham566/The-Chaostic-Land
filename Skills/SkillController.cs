using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    public List<SkillControl> skills;       // Danh sách các kỹ năng
    public Image skillIcon;          // Hình ảnh biểu tượng kỹ năng hiện tại (gắn vào UI)

    public SkillControl selectedSkill;     // Kỹ năng đang được chọn
    private int currentSkillIndex;   // Chỉ số của kỹ năng hiện tại

    public GameObject skillSlotContainer;
    public GameObject[] skillSlots;




    // Hàm để chọn kỹ năng
    public void SelectSkill(int skillIndex)
    {
        if (skillIndex >= 0 && skillIndex < skills.Count)
        {
            currentSkillIndex = skillIndex; // Cập nhật chỉ số kỹ năng hiện tại
            selectedSkill = skills[currentSkillIndex];
            skillIcon.sprite = selectedSkill.icon;
        }
    }

    // Hàm để chuyển đổi sang kỹ năng tiếp theo
    public void SwitchToNextSkill()
    {
        int nextSkillIndex = currentSkillIndex + 1;
        if (nextSkillIndex >= skills.Count)
        {
            nextSkillIndex = 0; // Quay lại kỹ năng đầu tiên nếu không còn kỹ năng tiếp theo
        }
        SelectSkill(nextSkillIndex);
    }

    // Hàm để sử dụng kỹ năng đang được chọn
    public void UseSelectedSkill()
    {
        if (selectedSkill != null)
        {
            selectedSkill.UseSkill();
            // Thực hiện logic sử dụng kỹ năng ở đây
            // Ví dụ: chạy animation, bắn đạn, áp dụng cooldown, vv.
            // Sau khi sử dụng xong, bạn có thể đặt selectedSkill về null.
        }
    }
    void Start()
    {
       
        skillSlots = new GameObject[skillSlotContainer.transform.childCount];

        for (int i = 0; i < skillSlotContainer.transform.childCount; i++)
        {
            skillSlots[i] = skillSlotContainer.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < skills.Count; i++)
        {
            UIItemSkill uiItemSkill = skillSlots[i].GetComponent<UIItemSkill>();
            uiItemSkill.skillControl = skills[i];
        }

        currentSkillIndex = 0; // Bắt đầu với kỹ năng đầu tiên
        SelectSkill(currentSkillIndex);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            // Thực hiện các hành động khi người chơi nhấn nút "I" ở đây
            Debug.Log("Nút 'I' đã được nhấn.");
            // Gọi hàm xử lý sự kiện hoặc thực hiện các hành động khác ứng với sự kiện này.
            UseSelectedSkill();

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Chuyển đổi sang kỹ năng tiếp theo
            SwitchToNextSkill();
        }
    }
}
