using System;
using System.Collections;
using UnityEngine;

public class AtkDmgUp : Item
{
    public float incAmount = 1f;
    public bool permanent = true;
    public float duration = 10f;

    public override void HandlePickup(IActor actor)
    {
        if(permanent)
        {
            actor.Damage += incAmount;
        }
        else
        {
            throw new NotImplementedException("temp attack damage buff not yet implemented");
        }
    }
}
