using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Stamina Bar")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float currentStamina;
    [SerializeField] private float staminaDepletationRate = 20f;
    [SerializeField] private float staminaRecoveryRate = 10f;

    public GameObject heathBar;
    public TextMeshProUGUI heathBarValue;
    public GameObject heathBarMini;
    private PlayerCharacter playerCharacter;

    void Start()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateUI();
    }

    private void UpdateUI()
    {
        float maxHeath = playerCharacter.MaxHealth;
        float currentHeath = playerCharacter.Health;

        if (heathBar)
        {
            heathBar.GetComponent<Image>().fillAmount = currentHeath / maxHeath;
            heathBarValue.GetComponent<TextMeshProUGUI>().text = currentHeath + "/" + maxHeath;
            heathBarMini.GetComponent<Image>().fillAmount = currentHeath / maxHeath;
        }

    }
}
