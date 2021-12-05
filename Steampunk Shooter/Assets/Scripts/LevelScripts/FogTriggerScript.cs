using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FogTriggerScript : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Fog").SetActive(false);
            gameObject.SetActive(false);
        }
    }
}