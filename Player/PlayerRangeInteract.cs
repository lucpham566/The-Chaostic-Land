using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeInteract : MonoBehaviour
{
    public PlayerRangeTarget playerRangeTarget;
    public List<GameObject> interactableObjects = new List<GameObject>();
    // ...

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra kiểu của đối tượng va chạm và gán là currentTarget nếu là một đối tượng target được hỗ trợ (NPC, Enemy, Item, ...)
        ITargetable targetable = other.GetComponent<ITargetable>();
        if (targetable != null)
        {
            GameObject objectGameObject = other.gameObject;
            interactableObjects.Add(objectGameObject);

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (interactableObjects.Contains(other.gameObject))
        {
            interactableObjects.Remove(other.gameObject);
        }
    }

    public bool CheckIntertactable()
    {
        return interactableObjects.Contains(playerRangeTarget.currentTarget);
    }

}
