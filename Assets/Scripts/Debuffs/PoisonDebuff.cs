using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoisonDebuff : Debuff
{
    private float timeSinceTick;
    public float tickTime;

    private PoisonSplash splashPrefab;
    public PoisonDebuff(Monster target) : base(target, 1)
    {
        // target.Speed *= 0.5f;
    }

}
