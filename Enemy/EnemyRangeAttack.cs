using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour
{
    public EnemyController enemyController;
    // Start is called before the first frame update

     void OnTriggerEnter2D(Collider2D other)
    {

        // Kiểm tra va chạm với collider dạng trigger
        if (other && other.CompareTag("Player"))
        {
            enemyController.isPlayerInRangeAttack = true;
        }
    }

     void OnTriggerExit2D(Collider2D other)
    {

        // Kiểm tra va chạm với collider dạng trigger
        if (other && other.CompareTag("Player"))
        {
            enemyController.isPlayerInRangeAttack = false;
        }
    }
}
