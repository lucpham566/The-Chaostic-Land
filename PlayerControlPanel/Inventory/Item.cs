using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, ITargetable
{
    public ItemClass item;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;


    public Vector3 targetPosition; // Đặt vị trí đích trong Inspector

    public Vector3 originalScale;
    public float pickupSpeed = 5f;

    public bool isPickedUp = false;

  
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        rb  = gameObject.GetComponent<Rigidbody2D>();
        sprite.sprite = item.itemIcon;
        originalScale = transform.localScale;
        pickupSpeed = 5f;

    }

    private void Update()
    {
        if (isPickedUp)
        {
            //làm cho vật phẩm nhỏ dần về 0
            float scalestep = Time.deltaTime;
            if (transform.localScale.x > 0.3)
            {
                transform.localScale -= new Vector3(scalestep, scalestep, scalestep);
            }

            // Di chuyển vật phẩm đến vị trí đích
            rb.gravityScale = 0;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, pickupSpeed * Time.deltaTime);

            // Kiểm tra nếu vật phẩm đã đến vị trí đích
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                // Vật phẩm đã được nhặt, có thể thực hiện các hành động tiếp theo ở đây
                Destroy(gameObject);
            }
        }
    }

    public string GetName()
    {
        return item.name;
    }


    public void PickUpItem(Vector3 position)
    {
        // Bắt đầu quá trình nhặt vật phẩm
        isPickedUp = true;
        targetPosition = position;
    }
}
