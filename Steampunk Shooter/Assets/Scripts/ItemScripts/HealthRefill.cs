using System;
using UnityEngine;

public class HealthRefill : Item
{
    public float healAmount = 5f;
    public override void HandlePickup(IActor actor)
    {
        actor.TakeDamage(-healAmount);
    }
}
