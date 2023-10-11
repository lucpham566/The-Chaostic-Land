using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public interface ITargetable
{
    string GetName();
}

public class PlayerRangeTarget : MonoBehaviour
{
    public List<GameObject> targetableObjects = new List<GameObject>();
    public int currentTargetIndex = -1; // Chỉ số của vật thể đang được target, -1 nghĩa là chưa có target
    public GameObject currentTarget;
    // ...

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra kiểu của đối tượng va chạm và gán là currentTarget nếu là một đối tượng target được hỗ trợ (NPC, Enemy, Item, ...)
        ITargetable targetable = other.GetComponent<ITargetable>();
        if (targetable != null)
        {
            GameObject objectGameObject = other.gameObject;
            targetableObjects.Add(objectGameObject);

            // Nếu chưa có target, hoặc đối tượng hiện tại ra khỏi vùng target, thiết lập target là đối tượng mới
            if (currentTargetIndex == -1)
            {
                currentTargetIndex = targetableObjects.IndexOf(objectGameObject);
                // Nếu chưa có target, hoặc đối tượng hiện tại ra khỏi vùng target, thiết lập target là đối tượng mới
                currentTarget = objectGameObject;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Kiểm tra nếu đối tượng ra khỏi vùng tự động target và là currentTarget, thì đặt currentTarget thành null
        if (other.gameObject == currentTarget)
        {
            currentTarget = null;
        }

        // Kiểm tra xem đối tượng ra khỏi vùng tự động target có trong danh sách không, nếu có thì loại bỏ nó khỏi danh sách
        if (targetableObjects.Contains(other.gameObject))
        {
            targetableObjects.Remove(other.gameObject);

            // Nếu đối tượng ra khỏi vùng target và nó là đối tượng hiện tại, thay đổi target
            if (currentTargetIndex != -1 && targetableObjects.Count > 0 && currentTargetIndex < targetableObjects.Count)
            {
                // Tăng chỉ số để thay đổi target cho đối tượng tiếp theo trong danh sách
                currentTargetIndex++;
                if (currentTargetIndex >= targetableObjects.Count)
                {
                    currentTargetIndex = 0; // Quay lại đầu danh sách nếu đã ở cuối
                }

                currentTarget = targetableObjects[currentTargetIndex];

            }
            else
            {
                // Nếu không còn đối tượng có thể target, đặt currentTargetIndex thành -1
                currentTargetIndex = -1;
            }
        }
    }
    void Update()
    {
        if (currentTargetIndex == -1 && targetableObjects.Count>0)
        {
            currentTargetIndex = 0;
            currentTarget = targetableObjects[currentTargetIndex];
        }
    }

    public void SwipeTarget()
    {
        if (currentTargetIndex != -1 && targetableObjects.Count > 0 && currentTargetIndex < targetableObjects.Count)
        {
            // Tăng chỉ số để thay đổi target cho đối tượng tiếp theo trong danh sách
            currentTargetIndex++;
            if (currentTargetIndex >= targetableObjects.Count)
            {
                currentTargetIndex = 0; // Quay lại đầu danh sách nếu đã ở cuối
            }

            currentTarget = targetableObjects[currentTargetIndex];

        }
        else
        {
            // Nếu không còn đối tượng có thể target, đặt currentTargetIndex thành -1
            currentTargetIndex = -1;
        }
    }
}
