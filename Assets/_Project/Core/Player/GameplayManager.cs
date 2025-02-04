using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField, Tooltip("A reference to the player.")]
    private GameObject _player;

    [SerializeField, Tooltip("How many hits the player can take before the game ends.")]
    private static float _playerHealth = 3f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void DamagePlayer()
    {
        _playerHealth -= 1f;

        if (_playerHealth == 0)
        {
            Debug.Log("Dead");
        }
    }
}
