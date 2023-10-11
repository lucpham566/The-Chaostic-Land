using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject tabListFrefab;
    public bool isOpenMenu = false;

    void Start()
    {
        tabListFrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOpenMenu)
            {
                isOpenMenu = true;
                tabListFrefab.SetActive(true);
            }
            else
            {
                isOpenMenu = false;
                tabListFrefab.SetActive(false);
            }

        }
    }
}
