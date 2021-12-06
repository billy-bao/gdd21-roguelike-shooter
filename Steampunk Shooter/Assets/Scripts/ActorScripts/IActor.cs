using System;
/// <summary>
/// General interface for all actors (player and enemies).
/// </summary>
public interface IActor
{
    /// <summary> Movement speed of this IActor. </summary>
    float MoveSpeed { get; set; }
    /// <summary> Attack speed of this IActor. </summary>
    float AttackSpeed { get; set; }
    /// <summary> Attack damage of this IActor. </summary>
    float Damage { get; set; }

    /// <summary>
    /// Call this function whenever the IActor should take damage.
    /// </summary>
    /// <param name="dmg">amount of damage</param>
    void TakeDamage(float dmg);
    /// <summary>
    /// Call this function to apply an effect to this IActor.
    /// </summary>
    /// <param name="eff"></param>
    void ApplyEffect(ActiveEffect eff);
}
