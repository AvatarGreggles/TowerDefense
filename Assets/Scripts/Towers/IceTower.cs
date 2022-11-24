using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : Tower
{
    private void Start()
    {
        ElementType = Element.Ice;
    }

    public override Debuff GetDebuff()
    {
        return new IceDebuff(Target);

    }

}
