using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IActor
{

    #region Unity_vars
    Rigidbody2D PlayerRB; //https://docs.unity3d.com/ScriptReference/Rigidbody.html
    SpriteRenderer Renderer;
    #endregion

    #region Movement_vars
    [SerializeField]
    private float moveSpeed = 5f;
    private Vector3 mousepos; // determines the rotation of the player
    private Vector2 looking;  // position the mouse is pointing at
    private float inputX;
    private float inputY;

    /// <summary> Movement speed of the player. </summary>
    public float MoveSpeed { set { moveSpeed = value; } get { return moveSpeed; } }
    #endregion

    #region Health_vars
    private Color orgColor; // stores original color for hit flash
    [SerializeField]
    private float maxLife = 10f;

    /// <summary> Max health of the player. </summary>
    public float MaxLife { get => maxLife; private set { maxLife = value; } }
    /// <summary> Current health of the player. </summary>
    public float Life { get; private set; }
    #endregion

    #region Attack_vars
    public GameObject bulletObj; // which bullet to shoot (attach in Unity)
    [SerializeField]
    private float bulletSpd = 15f; // movement speed of bullet
    [SerializeField]
    private float atkSpd = 1f;
    private float bulletTimer = 0f;
    private float BulletFreq { get { return 1f / AdjustedAtkSpd(); } }
    [SerializeField]
    private float bulletDmg = 2f; // damage bullet does

    /// <summary> Attack speed of the player. </summary>
    public float AttackSpeed { get { return atkSpd; } set { atkSpd = value; } }
    #endregion

    private LinkedList<ActiveEffect> activeEffects;

    #region Unity_funcs
    private void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        activeEffects = new LinkedList<ActiveEffect>();
        Life = MaxLife;
        orgColor = Renderer.color;
    }
    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        Move();
        UpdateFacing();

        //cooldown bullets
        if(bulletTimer > 0f)
        {
            bulletTimer -= Time.deltaTime;
        }

        //check for attacking
        if(Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }
    #endregion


    #region Movement_funcs
    private void Move()
    {
        // normalize diagonal movement
        Vector2 moveNorm = new Vector2(inputX, inputY).normalized;
        PlayerRB.velocity = moveNorm * AdjustedMovSpd();
    }
    #endregion

    #region Attack_funcs
    private void UpdateFacing()
    {
        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        looking = new Vector2((mousepos.x - transform.position.x), (mousepos.y - transform.position.y));
        transform.up = looking;
    }
    private void Attack()
    {
        if (bulletTimer > 0f) return;
        GameObject obj = Instantiate(bulletObj, PlayerRB.GetRelativePoint(new Vector2(0, 0)), Quaternion.identity);
        obj.GetComponent<Bullet>().Init(looking.normalized * bulletSpd, bulletDmg, Faction.Player, 3f);
        bulletTimer = BulletFreq;
        
    }
    #endregion

    #region Health_funcs
    // coroutine for flashing red on being damaged
    private IEnumerator HitFlash(Color color)
    {
        for (int i = 0; i < 2; i++)
        {
            Renderer.color = color;
            yield return new WaitForSeconds(0.2f);
            Renderer.color = orgColor;
            yield return new WaitForSeconds(0.2f);
        }
    }

    /// <summary>
    /// Causes player to take damage. Implements IActor.TakeDamage().
    /// </summary>
    /// <param name="dmg">amount of damage</param>
    public void TakeDamage(float dmg)
    {
        Life -= dmg;
        if (Life <= 0f)
        {
            Life = 0f;
            PlayerRB.gameObject.SetActive(false);
            // do game over
            Debug.Log("GAME OVER");
            Application.Quit();
        }
        if (Life > maxLife) { Life = maxLife; }
        StartCoroutine(HitFlash(dmg > 0 ? Color.red : Color.green));
    }
    #endregion

    /// <summary>
    /// Placeholder method. Increases player's stats.
    /// </summary>
    public void LevelUp()
    {
        Debug.Log("Leveled Up!");
        bulletDmg += 2f;
        bulletSpd *= 1.3f;
        moveSpeed += 0.5f;
        atkSpd *= 1.3f;
    }

    #region Effect_funcs
    public void ApplyEffect(ActiveEffect eff)
    {
        GameObject effObj = new GameObject("effObj");
        activeEffects.AddLast(effObj.AddComponent<ActiveEffect>().Clone(eff));
    }

    // remove expired effects
    private void CleanEffList()
    {
        var curNode = activeEffects.First;
        while (curNode != null)
        {
            if (curNode.Value == null)
            {
                var tmpNode = curNode.Next;
                activeEffects.Remove(curNode);
                curNode = tmpNode;
            }
            else
            {
                curNode = curNode.Next;
            }
        }
    }

    public float AdjustedMovSpd()
    {
        float adjSpeed = moveSpeed;
        CleanEffList();
        foreach(ActiveEffect eff in activeEffects)
        {
            if(eff != null)
            {
                if (eff.EffType == ActiveEffect.EffectType.MovSpdAdd)
                {
                    adjSpeed += eff.Value;
                }
                else if(eff.EffType == ActiveEffect.EffectType.MovSpdMul)
                {
                    adjSpeed *= eff.Value;
                }
            }
        }
        return adjSpeed;
    }

    public float AdjustedAtkSpd()
    {
        float adjSpeed = atkSpd;
        CleanEffList();
        foreach (ActiveEffect eff in activeEffects)
        {
            if (eff != null)
            {
                if (eff.EffType == ActiveEffect.EffectType.AtkSpdAdd)
                {
                    adjSpeed += eff.Value;
                }
                else if (eff.EffType == ActiveEffect.EffectType.AtkSpdMul)
                {
                    adjSpeed *= eff.Value;
                }
            }
        }
        return adjSpeed;
    }
    #endregion
}
