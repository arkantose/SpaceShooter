using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Private variable for the speed of the projectile
    [SerializeField]
    private float _speed;
    
    [SerializeField]
    Transform target;
    //**Secondary Fire Powerup**//

    [SerializeField]
    bool _homing;

    bool _gotTarget;
    
    void Start()
    {
        //To prevent checking for enemy every frame restrict it by invoking 0.25 second
        if(_homing == true)
            InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }
    //**************************************************************************//
    void Update()
    {
        MoveProjectile();
        DestroyProjectile();
    }

    void DestroyProjectile()
    {
        //If projectile is outside of the screen destroy the object
        if (transform.position.y > 10f)
        {
            if(transform.parent == null || transform.parent.name == "Projectiles_parent")
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
        else if (transform.position.y < -10f)
        {
            if (transform.parent == null || transform.parent.name == "Projectiles_parent")
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private void MoveProjectile()
    {
                 

        if(target == null)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime, Space.Self);

        }
        else
        {
            //**Secondary Fire Powerup**//

            //Ensure that the game object is rotated towards the target
            transform.up = target.position - transform.position;

            //if enemy is dead stop following target
            if (target.TryGetComponent(out Enemy enemyTarget))
            {
                if(enemyTarget.IsDead)
                {
                    target = null;
                }
            }
            else if (target.TryGetComponent(out EnemyRed enemyTarget2))
            {
                if(enemyTarget2.IsDead)
                {
                    target = null;
                }
            }

           // if (target.GetComponent<Enemy>().IsDead || target.GetComponent<EnemyRed>().IsDead)
                //target = null;

            //Move towards target
            transform.Translate(Vector3.up * _speed * Time.deltaTime, Space.Self);
            

            //***************************************************//
        }

    }
    //**Secondary Fire Powerup**//

    private void UpdateTarget()
    {
        //If target has already been found do not check again
        if (_gotTarget)
            return;

        //Look for all enemy objects in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //Initliaste with with Mathf.inifinity to ensure the distance is not to short
        float shortestDistance = Mathf.Infinity;

        GameObject nearestEnemy = null;

        //Iterate through each enemy in the seen and find the closest one
        foreach (GameObject enemy in enemies)
        {
            //Skip to next if enemy is dead already
            //if (enemy.GetComponent<Enemy>().IsDead || enemy.GetComponent<EnemyRed>().IsDead)
            //continue;

            if (enemy.TryGetComponent(out Enemy enemyTarget))
            {
                if (enemyTarget.IsDead)
                {
                    continue;
                }
            }
            else if (enemy.TryGetComponent(out EnemyRed enemyTarget2))
            {
                if (enemyTarget2.IsDead)
                {
                    continue;
                }
            }

            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;

                nearestEnemy = enemy;
            }
        }

        //If an enemy has been found set target
        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
            _gotTarget = true;
        }
        else
        {
            target = null;
        }
    }
    //***********************************************************//

}
