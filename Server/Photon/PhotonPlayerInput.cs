using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerInput : NetworkBehaviour
{
    Vector2 inputVector;
    [SerializeField] private bool inputJump = false;
    [SerializeField] bool inputDash = false;
    [SerializeField] bool inputAttack = false;
    [SerializeField] bool inputDefence = false;
    [SerializeField] bool inputChangeTarget = false;
    [SerializeField] bool inputInteract = false;
    [SerializeField] int inputUseSkill = 0;
    [SerializeField] int moveInput = 0;
    private void Update()
    {
        ResetData();
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1;
        }
        
        if (Input.GetKey(KeyCode.Space))
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

            inputDash = true;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            inputChangeTarget = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            inputInteract = true;
        }

    }

    private void ResetData()
    {
        moveInput = 0;
        inputAttack = false;
        inputJump = false;
        inputDash = false;
        inputChangeTarget = false;
        inputInteract = false;

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
