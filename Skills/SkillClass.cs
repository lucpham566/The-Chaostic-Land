using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/SkillClass")]
public class SkillClass : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public int damage;
    public int manaCost;
    public int staminaCost;
    public int magicCost;
    public float cooldownTime;
    public SkillControl skillControl;
}