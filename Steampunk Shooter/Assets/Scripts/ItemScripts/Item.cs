using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField]
    protected bool playerPickUp = true; // can item be picked up by player?
    [SerializeField]
    protected bool enemyPickUp = false; // can item be picked up by enemies?
    [System.NonSerialized]
    public int id = 0;

    /// <summary>
    /// Collision trigger for item pickups. Calls HandlePickup() if needed.
    /// </summary>
    /// <param name="collider">colliding object</param>
    public void OnTriggerEnter2D(Collider2D collider)
    {
        IActor hitObject = null;
        if (playerPickUp) // prioitize player pickup
        {
            hitObject = collider.gameObject.GetComponent<Player>();
        }
        if (hitObject == null && enemyPickUp)
        {
            hitObject = collider.gameObject.GetComponent<IEnemy>();
        }

        if(hitObject != null)
        {
            HandlePickup(hitObject);
            // clear item from level flags
            FindObjectOfType<LevelManager>()?.OnItemPickup(this);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Performs the effect of the actor picking up the item.
    /// All derived classes should implement this method.
    /// If only one of playerPickup and enemyPickup is set, this will always be called with a Player (or IEnemy).
    /// </summary>
    /// <param name="actor">The actor that picks up this item.</param>
    public abstract void HandlePickup(IActor actor);
}
