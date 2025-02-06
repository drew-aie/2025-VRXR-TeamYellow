using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBehaviour : MonoBehaviour
{
    [SerializeField] 
    private int _pointValue;

    [SerializeField, Tooltip("The score counter points are added to.")]
    private ScoreCounterBehaviour _scoreCounter;

    private void OnDestroy()
    {
        _scoreCounter.AddScore(_pointValue);
    }
}
