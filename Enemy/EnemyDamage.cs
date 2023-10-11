using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // Start is called before the first frame update
    public int damge;
    public bool isCollide = false;
    public GameObject collidePrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCharacter playerCharacter = other.GetComponent<PlayerCharacter>();
            if (playerCharacter != null && playerCharacter.dameable)
            {
                isCollide = true;
                if (damge!=0)
                {
                    playerCharacter.TakeDamage(damge);
                    if (collidePrefab != null)
                    {
                        GameObject newPrefab = Instantiate(collidePrefab, playerCharacter.transform.position, Quaternion.identity);
                        Destroy(newPrefab, 0.5f);
                    }
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
