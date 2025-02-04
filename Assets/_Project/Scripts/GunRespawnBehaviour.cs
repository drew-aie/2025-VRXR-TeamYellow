using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Core.Utils;
using HurricaneVR.Framework.Weapons;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GunRespawnBehaviour : MonoBehaviour
{
    [SerializeField]
    Transform RespawnLocation;

    public void ResetGunLocation()
    {
        StartCoroutine(DelayedRespawn());
    }
    IEnumerator DelayedRespawn()
    {
        yield return new WaitForSeconds(2);
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        this.GetComponent<Rigidbody>().position = RespawnLocation.position;
    }
}
