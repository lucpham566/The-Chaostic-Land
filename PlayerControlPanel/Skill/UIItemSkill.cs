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
        if (skillControl)
        {
            //if (skillControl.icon)
            //{
            //    Debug.Log(skillControl.icon + "skillControl.icon");
            //}
            //GameObject cooldownUI = transform.Find("Cooldown").gameObject;
            //GameObject icon = transform.Find("Image").gameObject;
            //cooldownUI.GetComponent<Image>().fillAmount = skillControl.cooldownTimer / skillControl.skillClass.cooldownTime;
            //icon.GetComponent<Image>().sprite = skillControl.icon;
        }
       
    }


}
