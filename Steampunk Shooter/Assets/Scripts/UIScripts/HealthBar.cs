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

    public void HealthIncreasedAnim()
    {
        StartCoroutine(ColorGradientFlash(Color.green));
    }

    private IEnumerator ColorGradientFlash(Color color)
    {
        Image im = slider.fillRect.gameObject.GetComponent<Image>();
        Color orgColor = im.color;
        for (int i = 0; i <= 10; i++)
        {
            im.color = Color.Lerp(orgColor, color, i / 10f);
            yield return new WaitForSeconds(0.01f);
        }
        im.color = color;
        yield return new WaitForSeconds(0.1f);
        im.color = orgColor;
    }

    //singleton stuff
    public static HealthBar instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(transform.parent.gameObject);
    }
}
