using System;
using System.Collections;
using UnityEngine;

public class ProjPierceUp : Item
{
    public int incAmount = 1;
    public bool permanent = true;
    public float duration = 10f;

    public override void HandlePickup(IActor actor)
    {
        if(permanent)
        {
            Player p = actor as Player;
            if(p != null) p.bulletPierce += incAmount;
        }
        else
        {
            throw new NotImplementedException("temp pierce buff not yet implemented");
        }
    }
}
