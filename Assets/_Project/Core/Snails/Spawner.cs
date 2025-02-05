using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField, Tooltip("The game object the spawner will be spawning.")]
    private GameObject _spawnee;

    [Header("Spawn Criteria")]

    [SerializeField, Tooltip("How long until the spawner spawns the object.")]
    private float _timeToSpawn = 10.0f;

    [SerializeField, Tooltip("Sets whether to spawn one game object upon starting.")]
    private bool _initialSpawn = true;

    private bool _bSpawnTriggered;

    // Update is called once per frame
    void Update()
    {
        //if (!GameplayManager._bGameStarted)
        //    return;

        Spawn();
    }

    private void Spawn()
    {
        if (_initialSpawn)
        {
            _initialSpawn = false;
            ObjectPoolManager.SpawnObject(_spawnee, transform.position, transform.rotation);
            return;
        }

        if (_bSpawnTriggered)
            return;

        _bSpawnTriggered = true;

        //Spawn object after spawn time elapses
        StartCoroutine(Delay(() => { ObjectPoolManager.SpawnObject(_spawnee, transform.position, transform.rotation); _bSpawnTriggered = false; }, _timeToSpawn));
    }

    private IEnumerator Delay(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }
}
