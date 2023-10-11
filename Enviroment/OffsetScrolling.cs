using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScrolling : MonoBehaviour
{
    // Start is called before the first frame update
    public float scrollSpeed;

    private Renderer renderer;
    private Vector2 saveOffset;


    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2 (x, 0);
        renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}
