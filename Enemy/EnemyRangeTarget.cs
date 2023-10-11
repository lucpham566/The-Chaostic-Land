using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeTarget : MonoBehaviour
{
    public EnemyController enemyController;
    // Start is called before the first frame update

    void OnTriggerEnter2D(Collider2D other)
    {

        // Kiểm tra va chạm với collider dạng trigger
        if (other.CompareTag("Player"))
        {
            enemyController.playerTarget = other.gameObject;
            Debug.Log("va chạm với nhân vật rồi dmd");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        // Kiểm tra va chạm với collider dạng trigger
        if (other.CompareTag("Player"))
        {
            enemyController.playerTarget = null;
            Debug.Log("thoát va chạm với nhân vật rồi dmd");
        }
    }
}
