using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Pause gameManager;

    public void Resume()
    {
        gameManager.Unpause();
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
