using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormDebuff : Debuff
{
    public StormDebuff(Monster target) : base(target, 1)
    {
        // target.Speed *= 0.5f;
    }

}
