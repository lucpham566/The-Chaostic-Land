using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class  EnemyPosition
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

public class EnemySpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public NetworkRunner _networkRunner;
    public List<EnemyPosition> enemyList = new List<EnemyPosition>();
    public List<SlotClass> items = new List<SlotClass>();
    public GameObject[] gameObjects;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
