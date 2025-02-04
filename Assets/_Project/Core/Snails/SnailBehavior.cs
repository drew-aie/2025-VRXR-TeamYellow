using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class SnailBehavior : MonoBehaviour
{
    [SerializeField, Tooltip("What the snail will advance towards. (The Player)")]
    private GameObject _target;

    [SerializeField, Tooltip("How much health the snail has.")]
    private float _snailHealth = 5.0f;

    [SerializeField, Tooltip("How fast the snail moves towards the target.")]
    private float _snailSpeed = 3.5f;

    private float _healthReset;
    private float _maxDamage;
    private float _bulletDamage = 2.5f;

    private bool _bDefeated;

    private NavMeshAgent _snail;

    // Start is called before the first frame update
    void Awake()
    {
        _snail = GetComponent<NavMeshAgent>();
        _snail.speed = _snailSpeed;
        _healthReset = _snailHealth;
        _maxDamage = _snailHealth * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_snail.enabled)
            return;

        //Setting agent to seek the player
        _snail.destination = _target.transform.position;
        //Making agent face the direction it's travelling using it's position and velocity
        _snail.transform.LookAt(_snail.transform.position + _snail.velocity);
    }

    //Set the health of the snail back to default value, called when returned to pool
    private void ResetHealth() => _snailHealth = _healthReset;

    //Reseting agent, called when respawning from pool
    private void ResetAgent()
    {
        _bDefeated = false;
        _snail.enabled = true;
        ResetHealth();
    }

    //Called when snail collides the hitbox of a bullet or explosion
    private void TakeDamage(float amount)
    {
        _snailHealth -= amount;

        if (_snailHealth <= 0.0f)
        {
            _bDefeated = true;
            PlayDeath();
        }
    }

    private void PlayDeath()
    {
        if (!_bDefeated)
            return;

        //Do death

        //Despawn after 3 seconds and reset health
        StartCoroutine(Delay(() => { ObjectPoolManager.ReturnObjectToPool(_snail.gameObject); ResetAgent(); }, 3.0f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("projectile"))
            return;

        TakeDamage(_bulletDamage);

        Vector3 bulletForce = new Vector3(1.5f, 1.5f, 1.5f);
        _snail.GetComponent<Rigidbody>().AddForce(bulletForce, ForceMode.Impulse);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Explosion")
        {
            TakeDamage(_maxDamage);

            //Explosion values
            float explosionRadius = other.gameObject.transform.localScale.x;
            Vector3 explosionPosition = other.gameObject.transform.position;

            //Disable navmesh agent for physics
            _snail.enabled = false;

            //Apply explosion force
            _snail.GetComponent<Rigidbody>().AddExplosionForce(100.0f, explosionPosition, explosionRadius);
        }
        if (other.gameObject.tag == "Damage Trigger")
        {
            GameplayManager.DamagePlayer();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Damage Trigger")
            StartCoroutine(Delay(() => { GameplayManager.DamagePlayer(); }, 3f));
    }

    private IEnumerator Delay(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }
}
