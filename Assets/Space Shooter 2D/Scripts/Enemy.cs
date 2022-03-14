using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variables to create an aggressive enemy
    [Header("Aggressive Settings")]
    [SerializeField]
    private float _ramRange = 5f;
    [SerializeField]
    private float _rammingSpeed = 6f;
    [SerializeField]
    private float _recoveryTime = 5f;
    [SerializeField]
    private float _hitPlayerOffset = 0.8f;
    [SerializeField]
    private bool _isInRamRange = false;
    [SerializeField]
    private bool _isRecovering = false;
    [SerializeField]
    private int _randomAggressor;
    //Enemy wave Manager
    public enum SpawnState { Spawning, Waiting, CoolDown };
    public SpawnState state = SpawnState.CoolDown;
    //Enemy Movement Selector and left right/ down 
    [SerializeField]
    private float _enemyMovementSelector;
    [SerializeField]
    private float _newSpeed = 2f;
    [SerializeField]
    private float _switchTime = 1f;
    [SerializeField]
    //Enemy Movement Circle
    private float _angle = 90f;
    [SerializeField]
    private float _circleSpeed = 0.5f;
    [SerializeField]
    private float _radius = 5f;
    [SerializeField]
    private float _centerPosX, _centerPosY;
    private float _x, _y;
    //Private variable for enemy speed
    [SerializeField]
    float _speed;

    [SerializeField]
    int _scoreAmount;

    [SerializeField]
    GameObject _projectile;

    //show player as game object
    [SerializeField]
    GameObject _player;

    [SerializeField]
    Vector3 _projectileSpawnPos;
        
    float _timeToNextShot;

    private bool _isDead = false;

    //Does the enemy have a shield
    [SerializeField]
    GameObject _enemyShield;
    [SerializeField]
    private int _randomizeEnemyShield;
    private bool _shieldIsActive = false;
    public bool IsDead
    {
        get { return _isDead; }
    }

    Transform _projectileParent;

    GameManager _gameManager;

    Animator _animator;

    AudioManager audioManager;

    WaveManager _waveManager;
    [SerializeField]
    

    public void Start()
    {
        //Player initialization for aggressive enemy
        _randomAggressor = Random.Range(0, 2);
        _player = GameObject.FindGameObjectWithTag("Player");
        //Random Shield Initialize
        _randomizeEnemyShield = Random.Range(0, 4);
        StartEnemyShield();
        //Movement Selector Initialize
        _enemyMovementSelector = Random.Range(0, 4);
        //Initialize new speed for movement types
        if (Random.value > 0.5f)
        {
            _newSpeed *= -1;
        }


        _centerPosX = Random.Range(-8f, 8f);
        _centerPosY = Random.Range(5f, 9f);
        _radius = Random.Range(5f, 12f);
        _circleSpeed = Random.Range(0.5f, 1f);
        if (Random.value > 0.5f)
        {
            _circleSpeed *= -1;
        }
        //Swap out Movement Types for Enemy
        if (_enemyMovementSelector == 1)
        {
            StartCoroutine(ZigZagSwitch());
        }

        //Initialize Variables
        _timeToNextShot = Time.time + Random.Range(3, 6);

        _animator = GetComponent<Animator>();

        _projectileParent = GameObject.Find("Projectiles_parent").GetComponent<Transform>();

        if (_animator == null)
        {
            Debug.Log("No Animator found");
        }


        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        

        if (_gameManager == null)
        {
            Debug.Log("GameManager not found");

        }

        audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();

        if (audioManager == null)
        {
            Debug.Log("No AudioManager found");
        }

        _waveManager = GameObject.Find("Wave Manager").GetComponent<WaveManager>();

        if (_waveManager == null)
        {
            Debug.Log("No Wave Manager found");
        }

        
    }

    public void Update()
    {
        
        //Ram Player
        RamMechanism();


        if (_isDead == false)
        {
            Shoot();

            EnemyMovement();
        }
    }

    //Ram player methods

    public void RamMechanism()
    {
        if (_isRecovering == false)
        {
            CheckPlayerRange();
        }

    }
    IEnumerator RecoverRoutine()
    {
        _isRecovering = true;
        yield return new WaitForSeconds(_recoveryTime);
        _isRecovering = false;
    }

    private float GetDistanceFromPlayer()
    {

        if (_player != null)
        {
            float playerYAxis = _player.transform.position.y;
            float enemyYAxis = transform.position.y;
            if (playerYAxis > enemyYAxis)
            {
                return Mathf.Infinity;
            }

            Vector2 currentPosition = transform.position;
            Vector2 playerPosition = _player.transform.position;

            float distance = Vector2.Distance(currentPosition, playerPosition);
            return distance;
        }
        /*else
        {
            Debug.Log("Player null");
        }*/
        return Mathf.Infinity;
    }
    private void CheckPlayerRange()
    {
        float distance = GetDistanceFromPlayer();
        if (distance < _hitPlayerOffset)
        {
            _isInRamRange = false;

            if (_isDead == true)
            {
                return;
            }

            StartCoroutine(RecoverRoutine());
            return;
        }

        if (distance <= _ramRange)
        {
            _isInRamRange = true;
        }
        else
        {
            _isInRamRange = false;
        }
    }

    void RamPlayer()
    {
        Vector2 playerPosition = _player.transform.position;
        float adjustedSpeed = _rammingSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, playerPosition, adjustedSpeed);
    }
    //Enemy Shield
    public void StartEnemyShield()
    {
        if (_randomizeEnemyShield == 1)
        {
            _enemyShield.SetActive(true);
            _shieldIsActive = true;
        }
    }

    //Different types of Enemy Movement//
    void EnemyMovement()
    {

        //Select Which movement type to give the enemy
        if (_isInRamRange == true && _randomAggressor == 1)
        {
            RamPlayer();
            //Debug.Log("Ramming");
        }
        else
        {
            switch (_enemyMovementSelector)
            {
                case 3:
                    //Enemy will move down
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                    break;
                case 2:
                    //Enemy will move left or right
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                    EnemyMovementLeftAndRight();
                    break;

                case 1:
                    //Enemy will move in a zigzag
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                    transform.Translate(Vector3.right * _newSpeed * Time.deltaTime);
                    break;

                case 0:
                    //enemy will move in a circle
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                    EnemyMovementCircle();
                    break;
            }
        }
        
        //If the enemy is outside of the screen move it back to the top
        if (transform.position.y < -7f)
            transform.position = new Vector3(Random.Range(-9f, 9f), 10f, 0f);
        //If enemy moves off the screen left and right; wrap around.
        if (transform.position.x < -11.3)
        {
           transform.position = new Vector3(11.3f, transform.position.y, 0f);
        }
        else if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0f);
        }
    }

    void EnemyMovementLeftAndRight()
    {
        
            transform.Translate(Vector3.right * _newSpeed * Time.deltaTime);
                
    }

    IEnumerator ZigZagSwitch()
    {
        while (true)
        {
            _newSpeed *= -1;
            yield return new WaitForSeconds(_switchTime);
        }
    }

    void EnemyMovementCircle()
    {
        _angle += _circleSpeed * Time.deltaTime;
        _x = Mathf.Cos(_angle) * _radius + _centerPosX;
        _y = Mathf.Sin(_angle) * _radius + _centerPosY;
        transform.position = new Vector3(_x, _y, 0);
    }
    //Enemy Shooting lasers back at player
    private void Shoot()
    {

        if (Time.time > _timeToNextShot)
        {
            //Reset the cooldown timer to the time since game started plus fire rate
            _timeToNextShot = Time.time + Random.Range(2 , 4);
                        
            GameObject laser = Instantiate(_projectile, transform.position + _projectileSpawnPos, Quaternion.identity);

            laser.transform.parent = _projectileParent;
                          

            audioManager.PlayLaserSound();

        }
    }

    /*void Respawn()
    {
         //Move to random position on top of the screen
        transform.position = new Vector3(Random.Range(-9f, 9f), 10f, 0f);
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if (_shieldIsActive == true)
            {
                _enemyShield.SetActive(false);
                _shieldIsActive = false;
            }

            else 
            { 
            if (other.GetComponent<Player>() != null)
            {
                other.GetComponent<Player>().DamagePlayer();
            }
            audioManager.PlayExplosionSound();
            GetComponent<BoxCollider2D>().enabled = false;
            _isDead = true;
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _waveManager.UpdateEnemyCount();
            Destroy(gameObject, 3f);
        }
        }

        if (other.tag == "Projectile")
        {
            if (_shieldIsActive == true)
            {
                _enemyShield.SetActive(false);
                _shieldIsActive = false;
                Destroy(other.gameObject);
            }
            else
            {
                audioManager.PlayExplosionSound();

                _gameManager.IncreaseScore(_scoreAmount);
                _animator.SetTrigger("OnEnemyDeath");
                _isDead = true;
                _speed = 0;
                _waveManager.UpdateEnemyCount();
                GetComponent<BoxCollider2D>().enabled = false;
                Destroy(other.gameObject);
                Destroy(gameObject, 3f);
            }
        }
    }

    

}
