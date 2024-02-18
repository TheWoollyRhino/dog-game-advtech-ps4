using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//made by Amrit Chatha

[CreateAssetMenu(menuName = "Powerups/Speed Powerup")]
public class SpeedBuff : Powerup
{
    [SerializeField] private float jogSpeedMultilplier = 1.7f;

    private ParticleSystem speedBoostExplosionParticles;
    private PlayerController playerController;

    public override void Init()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        speedBoostExplosionParticles = GameObject.Find("SpeedBoostExplosion").GetComponent<ParticleSystem>();
    }

    public override void ApplyEffects(GameObject target)
    {
        Init();
        Debug.Log("speed active");

        speedBoostExplosionParticles.Play();
        playerController.speedBoostActive = true;

        target.GetComponent<Animator>().SetFloat("jogSpeedMultiplier", jogSpeedMultilplier);
    }

    public override void RemoveEffects(GameObject target)
    {
        Debug.Log("speed inactive");

        playerController.speedBoostActive = false;

        target.GetComponent<Animator>().SetFloat("jogSpeedMultiplier", 1);
    }
}
