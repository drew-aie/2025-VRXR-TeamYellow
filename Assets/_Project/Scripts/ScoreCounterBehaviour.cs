using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounterBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    static ScoreCounterBehaviour _instance = null;

    private int _currentScore = 0;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        else if (_instance != this)
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        _text.text = "Score: " + _instance._currentScore;
    }

    public void AddScore(int value)
    {
        _instance._currentScore += value;
    }

    public void ResetScore()
    {
        _instance._currentScore = 0;
    }
}
