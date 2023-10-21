using Cinemachine;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhotonPlayer : NetworkBehaviour, IPlayerLeft
{
    public static PhotonPlayer local { get; set; }
    public GameObject localGameObject;
    public GameObject allForPlayer;
    CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake()
    {
        
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            local = this;
            Debug.Log("local spawned");
            localGameObject = gameObject;
            cinemachineVirtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
            cinemachineVirtualCamera.LookAt = null;
            cinemachineVirtualCamera.Follow = transform;
            local = this;
            GameObject newObject = Instantiate(allForPlayer);
        }
        else
        {
            Debug.Log("client spawned");

        }
    }
    void IPlayerLeft.PlayerLeft(PlayerRef player)
    {
        if (player == Object.HasInputAuthority)
        {
            Runner.Despawn(Object);
        }
    }
}
