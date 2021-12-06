using System;
using System.Collections;
using UnityEngine;

public class AtkSpdUp : Item
{
    public float incAmount = 0.5f;
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
            actor.ApplyEffect(new ActiveEffect(ActiveEffect.EffectType.AtkSpdAdd, incAmount, duration));
        }
        if(actor as Player != null)
        {
            (actor as Player).maxBullets += (int)(incAmount / 0.5f);
        }
    }
}
