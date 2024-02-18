using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//made by Amrit Chatha
public abstract class Powerup : ScriptableObject
{
    public float timeToReachTarget = 0.5f;
    public float powerupDuration = 10;

    public abstract void Init();
    public abstract void ApplyEffects(GameObject target);
    public abstract void RemoveEffects(GameObject target);
}
