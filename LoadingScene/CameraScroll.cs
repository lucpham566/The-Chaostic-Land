using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public float speed = 5.0f; // Tốc độ di chuyển
    private Transform myTransform;
    private void Start()
    {
        myTransform = transform;
    }

    private void Update()
    {
        // Di chuyển theo tọa độ x theo tốc độ speed và thời gian deltaTime
        Vector3 newPosition = myTransform.position + Vector3.right * speed * Time.deltaTime;
        myTransform.position = newPosition;
    }
}
