using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Player Health Bar Manager
    public Slider slider;

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        SetHealth(maxHealth);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    //singleton stuff
    public static HealthBar instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(transform.parent.gameObject);
    }
}
