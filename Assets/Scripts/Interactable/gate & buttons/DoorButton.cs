using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//made by Amrit Chatha

[CreateAssetMenu(menuName = "Door Buttons/Door Button")]
public class DoorButton : InteractableButton
{
    private Animator doorAnimator;
    [SerializeField] private string animatorGameObject;
    [SerializeField] private string doorOpenAnimation; // set this to the name of the animation in the door animator

    public override void Init()
    {
        doorAnimator = GameObject.Find(animatorGameObject).GetComponent<Animator>();
    }

    public override void ApplyEffects()
    {
        Init();

        doorAnimator.Play(doorOpenAnimation);
    }
}
