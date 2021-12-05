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
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.Find("UI"));
        SceneManager.MoveGameObjectToScene(FindObjectOfType<GameManager>().gameObject, SceneManager.GetActiveScene());
        PauseMenu.GameIsPaused = false;
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
