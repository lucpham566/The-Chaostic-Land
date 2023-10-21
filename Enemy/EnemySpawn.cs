using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class  EnemyPosition
{
    public int quantity=0;

    public EnemyPosition()
    {
        quantity = 0;
    }

    public EnemyPosition(int quantity)
    {
        this.quantity = quantity;
    }
}

public class EnemySpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public NetworkRunner _networkRunner;
    public List<EnemyPosition> enemyList;
    
    public GameObject[] gameObjects;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
