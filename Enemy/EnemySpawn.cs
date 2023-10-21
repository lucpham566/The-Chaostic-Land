using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;

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
        if (_networkRunner && _networkRunner.IsServer)
        {
            foreach (EnemySpawnPosition enemyPosition in enemyList)
            {
                if (enemyPosition.enemyNetworkObject)
                {
                    if (enemyPosition.enemyNetworkObject.isDeath && !enemyPosition.isSpawning)
                    {
                        StartCoroutine(DieOverTime(enemyPosition));
                    }
                }
            }
        }
       
    }

    public void SpawnEnemyStart(NetworkRunner runner)
    {
        _networkRunner = runner;
        foreach (EnemySpawnPosition enemyPosition in enemyList)
        {
            Debug.Log("Vào gen ra phần tử");
            try
            {
                NetworkObject networkObject = runner.Spawn(enemyPosition.enemy, enemyPosition.enemyTransform.position, Quaternion.identity);
                enemyPosition.enemyNetworkObject = networkObject.GetComponent<EnemyCharacter>();
            }
            catch
            {
                Debug.Log("lỗi dấdà");
            }
        }
    }

    public IEnumerator DieOverTime(EnemySpawnPosition enemyPosition)
    {
        Debug.Log("vào đợi 30s");
        enemyPosition.isSpawning = true;
        yield return new WaitForSeconds(30);
        Debug.Log("đợi xong 30s");

        enemyPosition.enemyNetworkObject.gameObject.SetActive(true);
        enemyPosition.enemyNetworkObject.transform.position = enemyPosition.enemyTransform.position;
        enemyPosition.enemyNetworkObject.ResetProperties();
        enemyPosition.isSpawning = false;

    }
}
