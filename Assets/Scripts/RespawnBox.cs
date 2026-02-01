using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnBox : MonoBehaviour
{
    private RespawnHelper respawnHelper;

    void Start()
    {
	respawnHelper = GameObject.FindWithTag("Player").GetComponent<RespawnHelper>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other is CapsuleCollider)
        {
            respawnHelper.ReturnToRespawn();
        }
    }
}
