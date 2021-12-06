using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamVentScript : MonoBehaviour
{
    private float steamTimer = 0f;
    private float steamOnDuration = 2f;
    private float steamOffDuration = 2f;
    private float attackTimer = 0f;
    private float attackFreq = 1f;
    private float damage = 1f;
    private float range = 1.5f;
    private bool damageOn = false;
    private RaycastHit2D hitInfo;
    public GameObject hitEffect;
    // Start is called before the first frame update
    void Start()
    {
        // fart
    }

    // Update is called once per frame
    void Update()
    {
        if (steamTimer > 0)
        {
            steamTimer -= Time.deltaTime;
        }
        else
        {
            // turn steam on/off
            if (damageOn)
            {
                steamTimer = steamOffDuration;
            }
            else
            {
                steamTimer = steamOnDuration;
            }
            damageOn = !damageOn;
            toggleSteamCloud();
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        checkIfPlayerInCloud();
    }

    private void checkIfPlayerInCloud()
    {
        Vector2 dir = (GameObject.FindWithTag("Player").transform.position - transform.position);
        hitInfo = Physics2D.Raycast(transform.position, dir, range);
        if (hitInfo)
        {
            Player player = hitInfo.transform.GetComponent<Player>();
            if (player)
            {
                Debug.DrawRay(transform.position, dir * hitInfo.distance, Color.yellow);
                Attack(player);
            }
            else
            {
                Debug.DrawRay(transform.position, dir * 1000, Color.white);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, dir * 1000, Color.white);
        }
    }

    private void toggleSteamCloud()
    {
        if (damageOn)
        {
            GameObject tmpCloud = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(tmpCloud, 2f);
        }
    }

    private void Attack(IActor p)
    {
        if (attackTimer <= 0 && damageOn)
        {
            p.TakeDamage(damage);
            attackTimer = attackFreq;
        }

    }
}
