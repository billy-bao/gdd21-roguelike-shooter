using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private int levelIndex = 1;

    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private AudioClip gameMusic;
    [SerializeField]
    private AudioClip menuMusic;

    public void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }
    public void Play()
    {
        SceneManager.LoadScene(levelIndex);
        soundManager.ChangeBGM(gameMusic);
    }

    public void Return()
    {
        SceneManager.LoadScene(0);
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.Find("UI"));
        SceneManager.MoveGameObjectToScene(FindObjectOfType<GameManager>().gameObject, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(FindObjectOfType<SoundManager>().gameObject, SceneManager.GetActiveScene());
        PauseMenu.GameIsPaused = false;
        Time.timeScale = 1f;
        soundManager.ChangeBGM(menuMusic);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
