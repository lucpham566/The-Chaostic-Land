using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public NetworkRunner _networkRunner;
    public List<EnemySpawnPosition> enemyList = new List<EnemySpawnPosition>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnEnemyStart(NetworkRunner runner)
    {
        foreach (EnemySpawnPosition enemyPosition in enemyList)
        {
            Debug.Log("Vào gen ra phần tử");
            try
            {
                NetworkObject networkObject = runner.Spawn(enemyPosition.enemy, enemyPosition.enemyTransform.position, Quaternion.identity);

            }
            catch
            {
                Debug.Log("lỗi dấdà");
            }
        }
    }
}
