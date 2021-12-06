using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBar : MonoBehaviour
{
    // Controls HP Bars above enemies
    // Could have done this in Enemy1.cs but I didn't want to screw up the spawns lol.
    private Vector3 scale;
    public Enemy1 enemy;

    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            Destroy(gameObject);
        }
        transform.position = enemy.transform.position - new Vector3((enemy.MaxLife / 5) * 0.33f, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        scale.x = enemy.Life / 40f;
        transform.localScale = scale;
    }
}
