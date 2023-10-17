using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer rbSprite;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rbSprite = rb.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.x < 0)
        {
            rbSprite.flipX = true;
        }
        else
        {
            rbSprite.flipX = false;
        }
    }
}
