using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text _scoreText;

    [SerializeField]
    Image _currentLivesImage;

    [SerializeField]
    Sprite[] _livesSprites;

    [SerializeField]
    Text _gameOverText;

    [SerializeField]
    Text _restartText;


    [SerializeField]
    GameObject _pauseMenu;

    //***PHASE 1: FRAMEWORK***///

    [SerializeField]
    Text _ammoText;

    [SerializeField]
    GameObject _noAmmoWarningText;

    [SerializeField]
    Text _waveAnnouncement;

    [SerializeField]
    Text _enemiesLeft;

    [SerializeField]
    Text _wavesLeft;
    
    [SerializeField]
    Text _youHaveWon;

    //**********************************//



    bool _playerIsDead;
    bool _playerHasWon;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    public void UpdateScoreUI(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLivesUI(int lives)
    {
        if (lives > 3)
            return;

        _currentLivesImage.sprite = _livesSprites[lives];
    }
    //Shows the player what wave they are starting
    public void UpdateWaveUI(int waveNumber)
    {
       
        _waveAnnouncement.text = "Wave: " + waveNumber;
        StartCoroutine(UpdateWaveAnnouncement());
    }
    IEnumerator UpdateWaveAnnouncement()
    {
        _waveAnnouncement.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _waveAnnouncement.gameObject.SetActive(false);
        StopCoroutine(UpdateWaveAnnouncement());
    }
    //Tells the player the information regarding current wave and enemies that are left.
    public void CurrentWaveInfoUI(int wavesLeft, int enemiesLeft)
    {
        _wavesLeft.text = "Waves Remaining: " + wavesLeft;
        _enemiesLeft.text = "Enemies Remaining: " + enemiesLeft;
    }

    //Show the player they have won the game
    public void YouveWon()
    {
        _playerHasWon = true;
        _restartText.gameObject.SetActive(true);
        gameManager.GameOver();
        StartCoroutine(YouHaveWon());
    }
    public void ShowGameOver()
    {
        _playerIsDead = true;
        _restartText.gameObject.SetActive(true);
        gameManager.GameOver();
        StartCoroutine(TextFlicker());


    }

   
    IEnumerator TextFlicker()
    {
        while(_playerIsDead)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator YouHaveWon()
    {
        while(_playerHasWon)
        {
            _youHaveWon.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            _youHaveWon.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void TogglePauseMenu()
    {
        if (_pauseMenu.activeSelf != true)
        {
            _pauseMenu.SetActive(true);

        }
        else
        {
            _pauseMenu.SetActive(false);
        }
    }
    //************AMMO COUNT*************************//
    public void UpdateAmmoUI(int ammoAmount)
    {
        _ammoText.text = ammoAmount + " /  15";

    }

    public void NoAmmo()
    {
        StartCoroutine(ShowAmmoNoWarning());
    }

    IEnumerator ShowAmmoNoWarning()
    {
        int count = 4;
        while(count > 1 )
        {
            _noAmmoWarningText.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _noAmmoWarningText.SetActive(false);
            yield return new WaitForSeconds(.5f);
            count--;
        }
    }
    //************************************************//

    
}
