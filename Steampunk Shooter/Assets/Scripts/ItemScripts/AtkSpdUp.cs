using System;
using System.Collections;
using UnityEngine;

public class AtkSpdUp : Item
{
    public float incAmount = 1f;
    public bool permanent = true;
    public float duration = 10f;

    public override void HandlePickup(IActor actor)
    {
        if(permanent)
        {
            actor.AttackSpeed += incAmount;
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
