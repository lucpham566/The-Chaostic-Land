using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{

    public bool inputJump = false;
    public bool inputDash = false;
    public bool inputAttack = false;
    public bool inputDefence = false;
    public bool inputChangeTarget = false;
    public bool inputInteract = false;
    public int inputUseSkill = 0;
    public int moveInput = 0;
    public Vector2 direction;

}