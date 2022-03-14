using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AttractPowerUps : MonoBehaviour
{

    private List<Powerup> _powerupsInRange = new List<Powerup>();
    private bool _isObjectInRange = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C) && _isObjectInRange == true)
        {
            for (int i = 0; i < _powerupsInRange.Count; i++)
            {
                _powerupsInRange[i].MoveToPlayer();
            }
        }

        if (Input.GetKeyUp(KeyCode.C) && _isObjectInRange == true)
        {
            for (int i = 0; i < _powerupsInRange.Count; i++)
            {
                _powerupsInRange[i].StopMovingToPlayer();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Powerup")
        {
            Powerup powerUp = other.GetComponent<Powerup>();
            if (powerUp != null)
            {
                _powerupsInRange.Add(powerUp);

                if (_isObjectInRange == false)
                {
                    _isObjectInRange = true;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Powerup")
        {
            Powerup powerUp = other.GetComponent<Powerup>();
            if (powerUp != null)
            {
                powerUp.StopMovingToPlayer();
                _powerupsInRange.Remove(powerUp);

                if (_powerupsInRange.Count == 0)
                {
                    _isObjectInRange = false;
                }
            }
        }
    }
}
