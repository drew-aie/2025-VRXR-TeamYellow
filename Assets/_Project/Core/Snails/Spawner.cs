using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField, Tooltip("The gameobject the spawner will be spawning.")]
    private GameObject _spawnee;

    [Header("Spawn Criteria")]

    [SerializeField, Tooltip("How long until the spawner spawns the object.")]
    private float _timeToSpawn = 10.0f;

    [SerializeField, Tooltip("The minimum amount of points the player needs to obtain before this gameobject will spawn.")]
    private float _minimumScoreAmount = 5000f;

    [SerializeField, Tooltip("The minimum amount of snails the player needs to defeat before this gameobject will spawn.")]
    private float _minimumKillAmount = 5f;

    [SerializeField, Tooltip("The maximum amount that can exist in the scene. (For grenades only)")]
    private float _maxCount;

    [Header("Boolean Spawn Criteria")]

    [SerializeField, Tooltip("Sets whether to spawn one game object upon starting.")]
    private bool _initialSpawn = true;

    [SerializeField, Tooltip("Set whether this gameobject will require a minimum score count before it will spawn")]
    private bool _scoreRequirement = false;

    [SerializeField, Tooltip("Set whether this gameobject will require a minimum defeat count before it will spawn")]
    private bool _killRequirement = false;

    private float _spawnTimer = 0f;
    private float _scoreCheck = 0f;
    private float _killCheck = 0f;

    private bool _bSpawnTriggered;

    ScoreCounterBehaviour _scoreSystem;

    private void Start() => _scoreCheck = _minimumScoreAmount;

    // Update is called once per frame
    void Update()
    {
        //Don't spawn anything if the game hasn't started
        //if (!GameplayManager._bGameStarted)
        //    return;

        _spawnTimer += Time.deltaTime;

        //Before spawning check if the object has a requirement before it can spawn
        if (SpawnCriteriaMet() && !_initialSpawn)
            Spawn();

        if (_initialSpawn)
        {
            _initialSpawn = false;
            ObjectPoolManager.SpawnObject(_spawnee, transform.position, transform.rotation);
        }
    }

    private void Spawn()
    {
        //Check if already spawning an object and if snail or grenade count has reached it's limit
        if (_bSpawnTriggered || 
            GameplayManager.bCheckEnemyCount() && _spawnee.tag == "Snail" ||
            GameplayManager.bCheckGrenadeCount() && _spawnee.tag != "Snail")
            return;

        _bSpawnTriggered = true;

        if (_spawnTimer >= _timeToSpawn)
        {
            ObjectPoolManager.SpawnObject(_spawnee, transform.position, transform.rotation);
            _bSpawnTriggered = true;
            _spawnTimer = 0f;
        }

        //If the gameobject is a snail increase the snail count, otherwise increase greanade count
        if (_spawnee.tag == ("Snail"))
            GameplayManager.IncreaseEnemyCount();
        else
            GameplayManager.IncreaseGrenadeCount();
    }

    private bool SpawnCriteriaMet()
    {
        if (_scoreRequirement)
        {
            if (_scoreSystem == null)
            {
                Debug.LogWarning("Score System isn't set");
                return false;
            }

            //Check if player's current score is greater or equal to the minimum requirement
            if (_scoreSystem.Score >= _scoreCheck)
            {
                //Increase the minimum requirement if so
                _scoreCheck += _minimumScoreAmount;
                //Criteria is met
                return true;
            }
            //If not, criteria is not met
            else
                return false;
        }
        if (_killRequirement)
        {
            //Check if player's current kill count is greater or equal to the minimum requirement
            if (GameplayManager.KillCount >= _killCheck)
            {
                //Increase the minimum requirement if so
                _killCheck += _minimumKillAmount;
                //Criteria is met
                return true;
            }
            //If not, criteria is not met
            else
                return false;
        }
        //If there is no requirement, just return true
        else if (!_scoreRequirement && !_killRequirement)
            return true;

        else
            return true;
    }
}
