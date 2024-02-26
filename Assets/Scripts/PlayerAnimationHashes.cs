using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//made by Amrit Chatha

public class PlayerAnimationHashes : MonoBehaviour
{
    public Animator animator;

    public int isIdleBool;
    public int isWalkingBool;
    public int isRunningBool;
    public int isPickingUpBool;
    public int isPuttingDownBool;
    public int isJumpingBool;

    private void Awake()
    {
        //set the ID references from the animator
        animator = gameObject.GetComponent<Animator>();

        //bools
        isIdleBool = Animator.StringToHash("isIdle");
        isWalkingBool = Animator.StringToHash("isWalking");
        isRunningBool = Animator.StringToHash("isRunning");
        isJumpingBool = Animator.StringToHash("isJumping");
        isPickingUpBool = Animator.StringToHash("isPickingUp");
        isPuttingDownBool = Animator.StringToHash("isPuttingDown");
    }
}