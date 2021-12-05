using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Player : MonoBehaviour, IActor
{

    #region Unity_vars
    Rigidbody2D PlayerRB; //https://docs.unity3d.com/ScriptReference/Rigidbody.html
    SpriteRenderer Renderer;
    #endregion

    #region Audio_Vars
    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private AudioClip gameOverMusic;
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
    private float meleeFreq = 1.6f;
    private float meleeTimer = 0f;
    private float BulletFreq { get { return 1f / AdjustedAtkSpd(); } }
    [SerializeField]
    private float bulletDmg = 2f; // damage bullet does
    [SerializeField]
    private int maxBullets = 5;
    private int currBullets; //Total bullets in one clip (before reloading)
    [SerializeField]
    private float reloadFreq = 1f; //Reload Speed
    private float reloadTimer = 0f;

    public float meleeDmg = 4f; // damage melee attack does

    public float meleeRange = 0.5f; //range of melee attack, currently a circle a little bit to the front of the player
    public Transform meleePoint; //center of the "melee cirle"
    public LayerMask enemyLayers;
    /// <summary> Attack speed of the player. </summary>
    public float AttackSpeed { get { return atkSpd; } set { atkSpd = value; } }
    #endregion

    #region Amination_Vars
    public Animator player_animator;
    #endregion

    #region UI_vars
    [SerializeField]
    private TextMeshProUGUI ammoText;
    [SerializeField]
    private TextMeshProUGUI enemyText;
    public bool areEnemiesCleared = false;

    private GameObject gameOver;

    [SerializeField]
    private HealthBar healthbar;

    private int numEnemies;
    #endregion
    private LinkedList<ActiveEffect> activeEffects;

    public static Player instance;

    #region Unity_funcs
    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }
    private void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        
        // Find UI elements.
        ammoText = GameObject.FindWithTag("AmmoText")?.GetComponent<TextMeshProUGUI>();
        healthbar = GameObject.FindWithTag("HealthBar")?.GetComponent<HealthBar>();
        enemyText = GameObject.FindWithTag("EnemyText")?.GetComponent<TextMeshProUGUI>();
        gameOver = GameObject.FindWithTag("GameOver");
        numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        activeEffects = new LinkedList<ActiveEffect>();
        Life = MaxLife;
        currBullets = maxBullets;
        if (ammoText) ammoText.text = "Ammo: " + maxBullets;
        if (enemyText) enemyText.text = "Enemies: " + numEnemies;
        if (gameOver) gameOver.SetActive(false);
        orgColor = Renderer.color;
        healthbar?.SetMaxHealth((int)MaxLife);

    }
    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        Move();
        UpdateFacing();

        numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (ammoText) ammoText.text = "Ammo: " + currBullets;
        if (enemyText)
        {
            if (!areEnemiesCleared)
            {
                enemyText.color = Color.white;
                enemyText.text = "Enemies: " + numEnemies;
            }
            else
            {
                enemyText.color = Color.green;
                enemyText.text = "Level cleared!";
            }
        }
        //cooldown bullets
        if (bulletTimer > 0f)
        {
            bulletTimer -= Time.deltaTime;
        }

        if (reloadTimer > 0f)
        {
            reloadTimer -= Time.deltaTime;
        }

        //check for attacking
        if (Input.GetButton("Fire1"))
        {
            Attack();
        }
        
        if(meleeTimer > 0f)
        {
            meleeTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Melee();
        }

        if(Input.GetButtonDown("Reload"))
        {
            Reload();
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
        if (bulletTimer > 0f || currBullets == 0 || reloadTimer > 0f) return; //maybe put these checks in a func later down the line
        GameObject obj = Instantiate(bulletObj, PlayerRB.GetRelativePoint(new Vector2(0, 0)), Quaternion.identity);
        obj.GetComponent<Bullet>().Init(looking.normalized * bulletSpd, bulletDmg, Faction.Player, 3f);
        bulletTimer = BulletFreq;
        currBullets -= 1;  
    }

    private void Reload()
    {
        reloadTimer = reloadFreq;
        currBullets = maxBullets;
        //Insert animation here?
    }


    private void Melee() //Brackeys tutorial
    {
        //play animation (add this later?)
        player_animator.SetTrigger("melee_trigger");
        //detect all the enemies in range (should the range be adjusted as the game progresses? 
        //for example, do we want the melee to hit the pigeons in any case?)
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, meleeRange, enemyLayers);
        //decrease all emeny health
        foreach(Collider2D enemy in hitEnemies)
        {
            //Debug.Log("player hit " + enemy.name);
            enemy.GetComponent<Enemy1>().TakeDamage(meleeDmg);
        }
        meleeTimer = meleeFreq;
    }

    private void OnDrawGizmosSelected()
    {
        if (meleePoint == null) return;
        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
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
        healthbar?.SetHealth((int)Life);
        if (Life <= 0f)
        {
            Life = 0f;
            PlayerRB.gameObject.SetActive(false);
            // do game over
            Debug.Log("GAME OVER");
            gameOver.SetActive(true);
            PlayGameOverMusic();
            Application.Quit();
        }
        if (Life > maxLife) { Life = maxLife; }
        StartCoroutine(HitFlash(dmg > 0 ? Color.red : Color.green));
    }
    #endregion

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

    #region Audio_funcs
    private void PlayGameOverMusic()
    {
        soundManager.ChangeBGM(gameOverMusic);
    }
    #endregion
}
