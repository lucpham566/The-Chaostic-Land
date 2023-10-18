using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayer : NetworkBehaviour, IPlayerLeft
{
    public static PhotonPlayer local { get; set; }
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            local = this;
            Debug.Log("local spawned");

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
