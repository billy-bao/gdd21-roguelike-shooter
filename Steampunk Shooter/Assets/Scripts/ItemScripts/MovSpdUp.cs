using System;
using System.Collections;
using UnityEngine;

public class MovSpdUp : Item
{
    public float incAmount = 5f;
    public bool permanent = true;
    public float duration = 10f;

    public override void HandlePickup(IActor actor)
    {
        if(permanent)
        {
            actor.MoveSpeed += incAmount;
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
