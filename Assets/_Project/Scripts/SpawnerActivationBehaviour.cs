using HurricaneVR.Framework.Core.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerActivationBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemySpawner;

    public void EnableSelectedSpawner()
    {
        EnemySpawner.SetActive(true);
    }
}
