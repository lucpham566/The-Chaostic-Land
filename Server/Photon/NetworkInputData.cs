using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{

    public bool inputJump;
    public bool inputDash ;
    public bool inputAttack;
    public bool inputDefence;
    public bool inputChangeTarget;
    public bool inputInteract;
    public int inputUseSkill;
    public int moveInput;

    public Vector2 direction;

}