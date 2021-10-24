using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy1 : MonoBehaviour, IEnemy
{
    

    #region Unity_vars
    private Rigidbody2D enemyRB; //https://docs.unity3d.com/ScriptReference/Rigidbody.html
    private SpriteRenderer Renderer;
    private Player player;
    #endregion

    #region Health_vars
    private Color orgColor; // stores original color for hit flash
    [SerializeField]
    private float maxLife = 5f;

    /// <summary> Max health. </summary>
    public float MaxLife { get => maxLife; private set { maxLife = value; } }
    /// <summary> Current health. </summary>
    public float Life { get; private set; }
    #endregion

    #region Attack_vars
    [SerializeField]
    private float damage = 1f; // damage done by this enemy
    [SerializeField]
    private float attackFreq = 1f; // cooldown period of attacks
    private float attackTimer = 0f;

    /// <summary> Attack speed of this enemy. </summary>
    public float AttackSpeed { get { return 1f / attackFreq; } set { attackFreq = 1f / value; } }
    #endregion

    #region Movement_vars
    [SerializeField]
    private float moveSpeed = 4f;
    private Vector2 direction;

    /// <summary> Movement speed of this enemy. </summary>
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    // AI stuff
    public float nextWaypointDistance = 5f;
    Path path;
    int currWaypoint;
    bool reachedEndofPath = false;
    Seeker seeker;
    #endregion

    #region Anim_Objects
    public Animator e1_animator;

    #endregion

    #region Unity_funcs
    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        orgColor = Renderer.color;

    }
    private void Start()
    {
        Life = MaxLife;
        // find the player and start calculating movement
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    private void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if(path == null)
        {
            return;
        }

        Move();

        look();
    }
    #endregion

    #region Move_funcs

    void UpdatePath()
    {
        if (seeker.IsDone()) {
            seeker.StartPath(enemyRB.position, player.transform.position, OnPathComplete);
        }
    }

    private void look()
    {
        
        if((Vector2) transform.up != direction)
        {
            transform.up = direction;
        }
        
    } 

    private void Move()
    {
        if(currWaypoint >= path.vectorPath.Count)
        {
            reachedEndofPath = true;
        }
        else
        {
            reachedEndofPath = false;
        }

        // use direction to influence the enemies velocity
        direction = ((Vector2)path.vectorPath[currWaypoint] - enemyRB.position).normalized;
        enemyRB.velocity = direction * moveSpeed;

        float distance = Vector2.Distance(enemyRB.position, path.vectorPath[currWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currWaypoint++;
        }

    }
    #endregion

    #region Attack_funcs

    // attack or wait to attack while colliding with player
    public void OnCollisionStay2D(Collision2D collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();
        if(p != null)
        {
            Attack(p);
        }
    }

    private void Attack(IActor p)
    {
        if (attackTimer <= 0)
        {
            p.TakeDamage(damage);
            attackTimer = attackFreq;
        }
        
    }
    #endregion

    #region AI_funcs
    void OnPathComplete(Path P)
    {
        Debug.Log("Line 88");
        if (!P.error)
        {
            path = P;
            currWaypoint = 0;
        }
    }
    #endregion

    #region Health_funcs
    // coroutine for flashing red on being damaged
    IEnumerator HitFlash()
    {
        for(int i = 0; i < 2; i++)
        {
            Renderer.color = Color.Lerp(orgColor, Color.white, 0.5f);
            yield return new WaitForSeconds(0.2f);
            Renderer.color = orgColor;
            yield return new WaitForSeconds(0.2f);
        }
    }

    /// <summary>
    /// Causes this enemy to take damage. Implements IActor.TakeDamage().
    /// </summary>
    /// <param name="dmg">amount of damage</param>
    public void TakeDamage(float dmg)
    {
        Life -= dmg;
        e1_animator.SetTrigger("Hit");
        if (Life <= 0f)
        {
            Life = 0f;
            e1_animator.SetBool("IsDead", true);
            Destroy(enemyRB.gameObject);
            return;
        }
        if (Life > maxLife) { Life = maxLife; }
        StartCoroutine(HitFlash());
    }
    #endregion

    public void ApplyEffect(ActiveEffect eff)
    {
        throw new NotImplementedException();
    }
}
