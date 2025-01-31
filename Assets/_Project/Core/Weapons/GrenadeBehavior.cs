using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehavior : MonoBehaviour
{
    [SerializeField, Tooltip("The mesh for the grenade.")]
    private GameObject _grenade;

    [SerializeField, Tooltip("The collider for the grenades explosion")]
    private GameObject _explosionCollider;

    [SerializeField, Tooltip("How many grenades the player currently has available.")]
    private float _grenadeCount = 1;

    [SerializeField, Tooltip("The maximum amount of grenades the player can have.")]
    private float _maxGrenadeCount = 3;

    [SerializeField, Tooltip("How long until the grenade explodes in seconds.")]
    private float _grenadeTimer = 5;

    [SerializeField, Tooltip("If the grenade is armed or not.")]
    private bool _grenadeIsPrimed;

    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        _grenadeIsPrimed = false;
        _explosionCollider.SetActive(false);

        _rigidbody = GetComponent<Rigidbody>();
    }

    //Handles the behaviors for detonation
    private void Detonation()
    {
        if (_explosionCollider == null)
        {
            Debug.Log("Explosion isn't set.");
            return;
        }

        //Activating explosion and deactiving grenade
        _explosionCollider.SetActive(true);
        _grenade.SetActive(false);

        //Making rigid body kinematic to prevent the explosion from rolling
        _rigidbody.isKinematic = true;

        //Using a coroutine to deactive the explosion after 3 seconds
        StartCoroutine(Delay(() => { _explosionCollider.SetActive(false); }, 3.0f));
    }

    public void OnThrow() => Invoke("Detonation", _grenadeTimer);

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Snail"))
            return;

        Detonation();
    }

    private IEnumerator Delay(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }
}
