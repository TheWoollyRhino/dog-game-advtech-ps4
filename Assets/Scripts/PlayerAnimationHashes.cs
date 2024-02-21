using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//made by Amrit Chatha

public class PlayerAnimationHashes : MonoBehaviour
{
    public Animator animator;

    public int isWalkingBool;
    public int isJoggingBool;
    public int isPressingBool;
    public int isJumpingBool;

    private void Awake()
    {
        //set the ID references from the animator
        animator = gameObject.GetComponent<Animator>();

        //bools
        isWalkingBool = Animator.StringToHash("isWalking");
        isJoggingBool = Animator.StringToHash("isJogging");
        isJumpingBool = Animator.StringToHash("isJumping");
        isPressingBool = Animator.StringToHash("isPressing");
    }
}