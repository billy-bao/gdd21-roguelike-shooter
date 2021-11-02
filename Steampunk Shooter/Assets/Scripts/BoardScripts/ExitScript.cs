﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player") 
        {
            Restart();
        }

    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
