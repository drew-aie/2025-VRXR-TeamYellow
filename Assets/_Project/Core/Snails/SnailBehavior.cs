using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SnailBehavior : MonoBehaviour
{
    [SerializeField, Tooltip("What the snail will advance towards. (The Player)")]
    private GameObject _target;

    [SerializeField, Tooltip("How fast the snail moves towards the target.")]
    private float _snailSpeed = 3.5f;

    private NavMeshAgent _snail;

    // Start is called before the first frame update
    void Awake()
    {
        _snail = GetComponent<NavMeshAgent>();
        _snail.speed = _snailSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Setting agent to seek the player
        _snail.destination = _target.transform.position;
        //Making agent face the direction it's travelling using it's position and velocity
        _snail.transform.LookAt(_snail.transform.position + _snail.velocity);
    }
}
