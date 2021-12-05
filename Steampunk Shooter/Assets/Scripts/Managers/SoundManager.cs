using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioSource BGM;
    public static SoundManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeBGM(AudioClip music)
    {
        Debug.Log("CHANGING MUSIC");
        this.BGM.Stop();
        this.BGM.clip = music;
        this.BGM.Play();
    }
}
