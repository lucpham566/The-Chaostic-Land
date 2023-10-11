using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    // Start is called before the first frame update
    public int damge;
    public GameObject collidePrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyCharacter enemyCharacter = other.GetComponent<EnemyCharacter>();
            if (enemyCharacter != null)
            {
                enemyCharacter.TakeDamage(damge);
                if (collidePrefab != null)
                {
                    GameObject newPrefab = Instantiate(collidePrefab, enemyCharacter.transform.position, Quaternion.identity);
                    Destroy(newPrefab,0.5f);
                }
            }
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
