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

    [SerializeField]
    float RespawnDelay;

    public void ResetGunLocation()
    {
        StartCoroutine(DelayedRespawn());
    }
    IEnumerator DelayedRespawn()
    {
        Rigidbody GunRigidBody = this.GetComponent<Rigidbody>();
        yield return new WaitForSeconds(2);
        GunRigidBody.velocity = new Vector3(0, 0, 0);
        GunRigidBody.angularVelocity = new Vector3(0, 0, 0);
        GunRigidBody.position = RespawnLocation.position;
    }
}
