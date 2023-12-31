﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject tabListFrefab;
    public PhotonPlayerController playerController;
    public bool isOpenMenu = false;
    public Canvas canvas = null;
    void Start()
    {
        playerController = PhotonPlayer.local.GetComponent<PhotonPlayerController>();
        tabListFrefab.SetActive(false);
        canvas = GetComponent<Canvas>();    
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
                playerController.controlEnable = false;
                canvas.sortingLayerName = "GUI";
            }
            else
            {
                isOpenMenu = false;
                tabListFrefab.SetActive(false);
                playerController.controlEnable = true;
                canvas.sortingLayerName = "Default";
            }

        }
    }
}
