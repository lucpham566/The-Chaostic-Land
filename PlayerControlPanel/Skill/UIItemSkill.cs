using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSkill : MonoBehaviour
{
    // Start is called before the first frame update
    public SkillControl skillControl;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject cooldownUI = transform.Find("Cooldown").gameObject;
        cooldownUI.GetComponent<Image>().fillAmount = skillControl.cooldownTimer/skillControl.skillClass.cooldownTime;
    }


}
