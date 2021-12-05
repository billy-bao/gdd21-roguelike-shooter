using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private int levelIndex = 1;

    public void Play()
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void Return()
    {
        SceneManager.LoadScene(0);
    }
}
