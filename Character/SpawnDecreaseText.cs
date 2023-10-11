using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnDecreaseText : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefabText;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnText(string text)
    {
        float randomX = Random.Range(transform.position.x - 1f, transform.position.x + 1f);
        float randomY = Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f);
        Vector3 spawnPosition = new Vector3(randomX, randomY, transform.position.z);

        GameObject newPrefab = Instantiate(prefabText, spawnPosition, Quaternion.identity);
        newPrefab.transform.SetParent(transform);
        newPrefab.transform.localScale = Vector3.one;

        TextMeshProUGUI DecreaseText = newPrefab.GetComponent<TextMeshProUGUI>();
        DecreaseText.text = text;
    }
}
