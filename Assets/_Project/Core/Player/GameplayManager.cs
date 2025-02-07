using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private static GameplayManager _instance;

    public static GameplayManager Instance => _instance;

    private static float _healthReset;
    private static float _playerHealth = 3f;
    private static float _playerKillCount = 0f;
    private static float _enemyCount;
    private static float _enemyLimit;
    private static float _grenadeCount;
    private static float _grenadeLimit = 3f;
    private static float _difficultyTimer;

    public static bool _bGameStarted = false;
    private static bool _bPlayerIsInvincible = false;
    private static bool _bEnemyCountMaxed = false;
    private static bool _bGrenadeCountMaxed = false;

    private static EState _currentState = EState.EASY;

    //Read only property for player invincibility boolean
    public static bool Invincible => _bPlayerIsInvincible;

    //Property for timer to be used in checking the difficulty of the game
    public static float DifficultyTimer
    {
        get => _difficultyTimer;
        set => _difficultyTimer = value;
    }

    public static float KillCount => _playerKillCount;

    public static float GrenadeLimit => _grenadeLimit;

    //Read only property for current difficulty state
    public static EState State => _currentState;

    public enum EState
    {
        START,
        EASY,
        MEDIUM,
        HARD,
        END
    }

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
            _instance = this;
        DontDestroyOnLoad(gameObject);

        _healthReset = _playerHealth;
    }

    private static void ResetHealth() => _playerHealth = _healthReset;

    //Static function to damage the player
    public static void DamagePlayer()
    {
        if (_bPlayerIsInvincible)
            return;

        _playerHealth -= 1f;

        PlayerInvincibility();

        if (_playerHealth == 0)
        {
            TriggerEndState();
        }
    }

    /// <summary>
    /// Checks if the number of enemies in the scene has reached its limit.
    /// </summary>
    /// <returns>Returns if the enemy count is maxed.</returns>
    public static bool bCheckEnemyCount()
    {
        CheckGameDifficulty();

        if (_enemyCount >= _enemyLimit)
            _bEnemyCountMaxed = true;
        else
            _bEnemyCountMaxed = false;

        return _bEnemyCountMaxed;
    }

    public static bool bCheckGrenadeCount()
    {
        if (_grenadeCount >= _grenadeLimit)
            _bGrenadeCountMaxed = true;
        else
            _bGrenadeCountMaxed = false;

        return _bGrenadeCountMaxed;
    }

    public static void IncreaseKillCount() => _playerKillCount += 1f;

    public static void IncreaseEnemyCount() => _enemyCount += 1f;

    public static void IncreaseGrenadeCount() => _grenadeCount += 1f;

    public static void DecreaseEnemyCount() => _enemyCount -= 1f;

    public static void DecreaseGrenadeCount() => _grenadeCount -= 1f;

    //Makes the player invincible
    private static void PlayerInvincibility() => _bPlayerIsInvincible = true;

    //Ends the player's invincibility
    public static void EndInvincibility() => _bPlayerIsInvincible = false;

    public static void TriggerEndState()
    {
        Debug.Log("Dead");
        //Game over stuff
    }

    private static void OnReset()
    {
        ResetHealth();
        ResetValues();
        TransitionTo(EState.START);
    }

    private static void ResetValues()
    {
        _playerKillCount = 0f;
        _enemyCount = 0f;
        _grenadeCount = 0f;
    }

    private static void CheckGameDifficulty()
    {
        if (_currentState == EState.START)
        {
            if (_bGameStarted)
                TransitionTo(EState.EASY);

            return;
        }
        else if (_currentState == EState.EASY)
        {
            _enemyLimit = 5f;

            if (DifficultyTimer >= 30f)
                TransitionTo(EState.MEDIUM);

            return;
        }
        else if (_currentState == EState.MEDIUM)
        {
            _enemyLimit = 7f;
            Debug.Log("Time");
            if (DifficultyTimer >= 90f)
                TransitionTo(EState.HARD);

            return;
        }
        else if (_currentState == EState.HARD)
        {
            _enemyLimit = 10f;

            if (DifficultyTimer >= 120f)
                TransitionTo(EState.END);

            return;
        }
        else if (_currentState == EState.END)
        {
            //Player wins
        }
        else
            return;
    }

    private static void TransitionTo(EState state)
    {
        if (state == EState.END || state == _currentState)
            return;

        _currentState = state;
    }
}
