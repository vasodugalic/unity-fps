using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    public GameObject bullet;
    public GameObject gameScreen;
    public GameObject endScreen;
    public static bool isPaused;
    public static bool isPlayerDead;

    void Start()
    {
        isPlayerDead = false;
        isPaused = false;
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
