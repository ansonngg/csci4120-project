using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    public int deadArm = 0;
    public GameObject winCanvas;
    Pause gameManager;

    private void Start()
    {
        gameManager = GetComponent<Pause>();
    }

    // Update is called once per frame
    void Update()
    {
        if (deadArm == 2)
        {
            winCanvas.SetActive(true);
            Cursor.visible = true;
            gameManager.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
