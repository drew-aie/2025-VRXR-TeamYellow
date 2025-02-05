using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField, Tooltip("A reference to the player.")]
    private GameObject _player;

    private static float _playerHealth = 3f;
    private float _healthReset;

    public static bool _bGameStarted = false;
    private static bool _bPlayerIsInvincible = false;

    //Read only property for player invincibility boolean
    public static bool Invincible => _bPlayerIsInvincible;

    public enum EDifficultyState
    {
        START,
        EASY,
        MEDIUM,
        HARD,
        END
    }

    private void Awake()
    {
        _healthReset = _playerHealth;
    }

    private void ResetHealth() => _playerHealth = _healthReset;

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

    //Makes the player invincible
    public static void PlayerInvincibility() => _bPlayerIsInvincible = true;

    //Ends the player's invincibility
    public static void EndInvincibility() => _bPlayerIsInvincible = false;

    public static void TriggerEndState()
    {
        Debug.Log("Dead");
        //Game over stuff
    }

    private static void GameDifficulty()
    {

    }
}
