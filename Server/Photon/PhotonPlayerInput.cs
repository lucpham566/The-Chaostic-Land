using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerInput : NetworkBehaviour
{
    Vector2 inputVector;
    bool inputJump = false;
    bool inputDash = false;
    bool inputAttack = false;
    bool inputDefence = false;
    int inputUseSkill = 0;
    int moveInput = 0;
    private void Update()
    {
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1;
        }

        if (Input.GetButtonDown("Jump"))
        {
            inputJump = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            inputAttack = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            inputDefence = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            inputDefence = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            inputDash = false;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            inputChangeTarget = false;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            inputInteract = true;
        }

    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        networkInputData.direction = inputVector;
        networkInputData.inputJump = inputJump;
        networkInputData.inputAttack = inputAttack;
        networkInputData.inputDefence = inputDefence;
        networkInputData.inputDash = inputDash;
        networkInputData.inputChangeTarget = inputChangeTarget;
        networkInputData.inputInteract = inputInteract;
        networkInputData.moveInput = moveInput;

        return networkInputData;
    }
}
