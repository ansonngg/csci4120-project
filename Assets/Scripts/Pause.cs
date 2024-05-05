using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTreeFPS;

public class Pause : MonoBehaviour
{
    public GameObject pauseCanvas;
    public FPSController player;
    bool paused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Unpause();
            }
            else
            {
                Time.timeScale = 0;
                paused = true;
                pauseCanvas.SetActive(true);
                player.enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        paused = false;
        pauseCanvas.SetActive(false);
        player.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
