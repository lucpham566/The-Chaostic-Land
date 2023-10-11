using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "New Enemy")]
public class EnemyClass : ScriptableObject
{
    public string enemyName;
    public int maxHP;
    public int damage;
    public int armor;
    public RuntimeAnimatorController animationController;
}
