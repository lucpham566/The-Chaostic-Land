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
    public GameObject targetIcon;

    void Start()
    {
        //playerRangeTarget= PhotonPlayer.local.GetComponent<PlayerRangeTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRangeTarget.currentTarget != null)
        {
            targetIcon.SetActive(true);
            Collider2D objectCollider = playerRangeTarget.currentTarget.GetComponent<Collider2D>();
            Transform objectTransform = playerRangeTarget.currentTarget.GetComponent<Transform>();
            float objectHeight = objectCollider.bounds.size.y;
            targetIcon.transform.position = new Vector3(objectTransform.position.x, objectTransform.position.y + objectHeight / 2 + 0.5f, objectTransform.position.z);
        }
        else
        {
            targetIcon.SetActive(false);
        }

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
