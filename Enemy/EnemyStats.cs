using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [Header("Stamina Bar")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float currentStamina;
    [SerializeField] private float staminaDepletationRate = 20f;
    [SerializeField] private float staminaRecoveryRate = 10f;

    [SerializeField] private float delay = 1f;

    public GameObject heathBar;
    public GameObject enemyStats;
    public GameObject Enemy;
    private EnemyCharacter enemyCharactor;

    void Start()
    {
        enemyCharactor = GetComponent<EnemyCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckHeath();
        UpdateUI();
    }

    private void CheckHeath()
    {
        if (enemyCharactor.isDeath)
        {
            Destroy(enemyStats,0.5f);
        }
        else
        {
            return;
        }
    }

    private void UpdateUI()
    {
        float maxHeath = enemyCharactor.MaxHealth;
        float currentHeath = enemyCharactor.Health;

        if (heathBar)
        {
            heathBar.GetComponent<Image>().fillAmount = currentHeath / maxHeath;
        }

    }
}
