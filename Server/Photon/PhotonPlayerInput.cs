using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerInput : NetworkBehaviour
{
    Vector2 inputVector;

    private void Update()
    {
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        networkInputData.direction = inputVector;

        return networkInputData;
    }
}
