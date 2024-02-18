using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//made by Amrit Chatha

[CreateAssetMenu(menuName = "Powerups/Double Jump Powerup")]
public class DoubleJumpBuff : Powerup
{
    private ParticleSystem doubleJumpExplosionParticles;
    private PlayerController playerController;

    public override void Init()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        doubleJumpExplosionParticles = GameObject.Find("DoubleJumpExplosion").GetComponent<ParticleSystem>();
    }

    public override void ApplyEffects(GameObject target)
    {
        Init();
        Debug.Log("Double Jump active");

        doubleJumpExplosionParticles.Play();
        playerController.canDoubleJump = true;
    }

    public override void RemoveEffects(GameObject target)
    {
        Debug.Log("Double Jump inactive");

        playerController.canDoubleJump = false;
    }
}
