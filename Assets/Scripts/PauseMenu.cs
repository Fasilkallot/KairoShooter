using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuUI;
    [SerializeField]
    private GameObject reticle;
    [SerializeField]
    private GameObject winnerScreen;
    [SerializeField]
    private GameObject gameOverScreen;
    private PlayerInput input;
    private InputAction escapeButton;
    public static bool gameIsPaused = false;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();

        escapeButton = input.actions["Pause"];
    }

    // Update is called once per frame
    void Update()
    {

 
        if (escapeButton.triggered)
        {
            if(gameIsPaused)
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
        reticle.SetActive(true);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        gameIsPaused = false;
    }
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        reticle.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        gameIsPaused = true;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        reticle.SetActive(false);
    }
    public void Winner()
    {
        winnerScreen.SetActive(true);
        reticle.SetActive(false);
    }
    public void Retry()
    {
        SceneManager.LoadScene(1);

    }
}
