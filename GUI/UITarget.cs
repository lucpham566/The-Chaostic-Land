using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITarget : MonoBehaviour
{
    public GameObject heathBar;
    public PlayerRangeTarget playerRangeTarget; 
    public GameObject targetObject; 
    public GameObject UnitframeObject;
    public TextMeshProUGUI name;

    void Start()
    {
        playerRangeTarget= GetComponent<PlayerRangeTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (playerRangeTarget.currentTarget != null)
        {
            targetObject.SetActive(true);
            ITargetable targetable = playerRangeTarget.currentTarget.GetComponent<ITargetable>();
            name.text = targetable.GetName();
            if (playerRangeTarget.currentTarget.GetComponent<EnemyCharacter>() != null)
            {
                EnemyCharacter enemyCharacter = playerRangeTarget.currentTarget.GetComponent<EnemyCharacter>();

                float maxHeath = enemyCharacter.MaxHealth;
                float currentHeath = enemyCharacter.Health;

                if (heathBar)
                {
                    heathBar.GetComponent<Image>().fillAmount = currentHeath / maxHeath;
                }
                UnitframeObject.SetActive(true);
            }
            else
            {
                UnitframeObject.SetActive(false);
            }
        }
        else
        {
            targetObject.SetActive(false);
        }

    }
}
