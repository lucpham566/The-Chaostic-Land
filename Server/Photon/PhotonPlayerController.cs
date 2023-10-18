using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerController : NetworkBehaviour
{
    Rigidbody2D rb;
    Vector2 directionInput;
    public float speed = 20;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            directionInput = data.direction;
            Debug.Log("ghi nhận " +  directionInput);
        }
        else
        {
            directionInput = Vector2.zero;
            Debug.Log("Không ghi nhận " + directionInput);
        }
        rb.velocity = directionInput*speed;

    }

}
