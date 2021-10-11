using System;
/// <summary>
/// General interface for all actors (player and enemies).
/// </summary>
public interface IActor
{
    /// <summary>
    /// Call this function whenever the IActor should take damage.
    /// </summary>
    /// <param name="dmg">amount of damage</param>
    void TakeDamage(float dmg);
}
