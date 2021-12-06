using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCoilScript : MonoBehaviour
{
    private float attackTimer = 0f;
    private float attackFreq = 1f;
    private float damage = 1f;
    // Start is called before the first frame update
    void Start()
    {
        // fart
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    // public void OnCollisionStay2D(Collision2D collision)
    // {
    //     Player p = collision.gameObject.GetComponent<Player>();
    //     if (p != null)
    //     {
    //         Attack(p);
    //     }
    // }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hit");
        Player p = other.gameObject.GetComponent<Player>();
        if (p != null)
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
}
