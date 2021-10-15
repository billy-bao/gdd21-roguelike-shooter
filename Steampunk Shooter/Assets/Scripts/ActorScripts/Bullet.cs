using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Unity_vars
    Rigidbody2D BulletRB; //https://docs.unity3d.com/ScriptReference/Rigidbody.html
    SpriteRenderer Renderer;
    #endregion

    #region Fields
    /// <summary> Directly gets velocity of the bullet RigidBody. </summary>
    public Vector2 Velocity { get => BulletRB.velocity; private set { velocity = value; BulletRB.velocity = value; } }
    /// <summary> Damage dealt by bullet. </summary>
    public float Power { get; private set; }
    /// <summary> Faction bullet belongs to. </summary>
    public Faction Faction { get; private set; }
    /// <summary> Timer for remaining duration of bullet before disappearing. </summary>
    public float RemTime { get; private set; }

    private Vector2 velocity; // stores velocity
    #endregion

    #region Unity_funcs
    private void Start()
    {
        BulletRB = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        // set correct velocity (can only set BulletRB.velocity after GetComponent)
        Velocity = velocity;
        // placeholder color code to differentiate player/enemy bullets
        // TODO: use different sprites here instead?
        if (Faction == Faction.Player)
        {
            Renderer.color = Color.yellow;
        }
        else
        {
            Renderer.color = Color.white;
        }
    }
    private void Update()
    {
        // does bullet run out of time?
        RemTime -= Time.deltaTime;
        if(RemTime <= 0f)
        {
            Destroy(BulletRB.gameObject);
        }
    }
    #endregion

    /// <summary>
    /// Initializes the bullet with a velocity, damage, faction, and duration. Always call this once after instantiation.
    /// </summary>
    /// <param name="vel">velocity of the bullet</param>
    /// <param name="pow">how much damage the bullet does</param>
    /// <param name="fac">the faction which the bullet belongs to</param>
    /// <param name="time">amount of time the bullet should last before disappearing.</param>
    public void Init(Vector2 vel, float pow, Faction fac, float time)
    {
        //Debug.Log("initing bullet with: " + vel + pow + fac + time);
        velocity = vel;
        Power = pow;
        Faction = fac;
        RemTime = time;
    }

    /// <summary>
    /// Collision trigger for bullets. Damages actors based on faction of bullet, and destroys bullet on hitting walls.
    /// </summary>
    /// <param name="collider">colliding object</param>
    public void OnTriggerEnter2D(Collider2D collider)
    {
        // check for collision with correct type of actor
        IActor hitObject = null;
        switch(Faction)
        {
            case Faction.Player:
                {
                    hitObject = collider.gameObject.GetComponent<IEnemy>();
                    break;
                }
            case Faction.Enemy:
                {
                    hitObject = collider.gameObject.GetComponent<Player>();
                    break;
                }
        }
        if(hitObject != null)
        {
            hitObject.TakeDamage(Power);
            Destroy(gameObject);
        }

        // destroy bullet on hitting walls
        if (collider.gameObject.tag.Equals("Wall")) { Destroy(BulletRB.gameObject); }
    }
}
