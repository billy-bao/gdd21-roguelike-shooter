using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // couldve put this and Menu in one script but oh well
    public static bool GameIsPaused = false;
    // public static bool GameEnded = false;
    public GameObject pauseMenuUI;

    private void Awake()
    {
        pauseMenuUI.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Player.gameOver.activeInHierarchy && !GameManager.winScreen.activeInHierarchy)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        FindObjectOfType<SoundManager>().BGM.UnPause();
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        FindObjectOfType<SoundManager>().BGM.Pause();
    }
}