using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySpawnPosition
{
    public GameObject enemy;
    public Transform enemyTransform;

    public EnemyPosition()
    {
        enemy = null;
        enemyTransform = null;
    }

    public EnemyPosition(GameObject enemy,Transform enemyTransform)
    {
        this.enemy = enemy;
        this.enemyTransform = enemyTransform;
    }
}
