using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormTower : Tower
{
    private void Start()
    {
        ElementType = Element.Storm;
    }

    public override Debuff GetDebuff()
    {
        return new StormDebuff(Target);

    }
}
