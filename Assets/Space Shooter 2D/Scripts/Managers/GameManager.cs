using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private int _score;
    private UIManager _uiManager;

    private bool _playerIsDead;

    //private bool to check if game is paused
    private  bool _isPaused;


    private void Start()
    {
        _uiManager = GameObject.Find("Game_Manager").GetComponent<UIManager>();

    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && _playerIsDead)
        {
            SceneManager.LoadScene(0);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

    }

    public void GameOver()
    {
        _playerIsDead = true;
    }

    public void IncreaseScore(int score)
    {
        _score += score;

        _uiManager.UpdateScoreUI(_score);
    }

    public void Pause()
    {
        _isPaused = !_isPaused;
        _uiManager.TogglePauseMenu();
        if (_isPaused)
        {

            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;

        }
               
    }

    public void QuitGame()
    {
        Application.Quit();        
    }

}
