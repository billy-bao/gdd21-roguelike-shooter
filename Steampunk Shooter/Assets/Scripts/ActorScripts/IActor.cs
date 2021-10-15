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

    /// <summary>
    /// Call this function whenever the IActor should take damage.
    /// </summary>
    /// <param name="dmg">amount of damage</param>
    void TakeDamage(float dmg);
}
