using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject gameSceneUI;

    void Update()
    {
        if (Global.isPlayerDead)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Global.isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameSceneUI.SetActive(true);
        Time.timeScale = 1f;
        Global.isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        gameSceneUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Global.isPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Global.isPaused = false;
        SceneManager.LoadScene("Menu");
    }
}
