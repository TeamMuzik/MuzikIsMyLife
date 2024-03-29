using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject pausePanel;
    public GameObject AudioManager;
    private AudioSource audioSource;

    
    void Start()
    {
        audioSource = AudioManager.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        audioSource.UnPause();
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        audioSource.Pause();
    }
}
