using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnPosition
{
    public GameObject enemy;
    public Transform enemyTransform;

    public EnemySpawnPosition()
    {
        enemy = null;
        enemyTransform = null;
    }

    public EnemySpawnPosition(GameObject enemy,Transform enemyTransform)
    {
        this.enemy = enemy;
        this.enemyTransform = enemyTransform;
    }
}
