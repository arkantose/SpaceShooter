using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Powerup : MonoBehaviour
{

    [SerializeField]
    float _speed;
    [SerializeField]
    PowerupType _powerupType;
    [SerializeField]
    //collect powerup
    private Transform _playerTransform;
    private bool _isMovingToPlayer = false;

    //audio
    AudioManager audioManager;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();

        if(audioManager == null)
        {
            Debug.Log("No AudioManager found");
        }
              

    }

    void Update()
    {
        
        Movement();
        

    }

    //********************************Collect Powerups*********************//


    public void MoveToPlayer()
    {
        if (_isMovingToPlayer == false)
        {
            _isMovingToPlayer = true;
        }
    }

    public void StopMovingToPlayer()
    {
        if (_isMovingToPlayer == true)
        {
            _isMovingToPlayer = false;
        }
    }

    void Movement()
    {
        if (_isMovingToPlayer == false)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y < -6f)
                Destroy(gameObject);
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, _speed * Time.deltaTime);
        }

        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            audioManager.PlayPowerUpSound();

            if (collision.GetComponent<Player>() != null)
            {
                collision.GetComponent<Player>().EnablePowerup(_powerupType);
            }

            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject);
        }
    }
}

public enum PowerupType
{
    tripleShot,
    speedBoost,
    shield,
    ammo,
    health,
    homing,
    negative
    
}