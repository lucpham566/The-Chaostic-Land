using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyDamage enemyDamage;
    void Start()
    {
        Invoke("Remove", 5f);
        enemyDamage = GetComponent<EnemyDamage>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyDamage.isCollide)
        {
            Remove();
        }
    }


    private void Remove()
    {
        Destroy(gameObject);
    }

}
