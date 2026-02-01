using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnFlag : MonoBehaviour
{
    private RespawnHelper respawnHelper;
    [SerializeField] private MeshRenderer flagUp;
    [SerializeField] private MeshRenderer flagDown;

    void Start()
    {
	respawnHelper = GameObject.FindWithTag("Player").GetComponent<RespawnHelper>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other is CapsuleCollider)
        {
            respawnHelper.UpdateRespawnPosition(gameObject.GetComponent<RespawnFlag>());
	    ToggleFlag();
        }
    }
    public void ToggleFlag() {
	flagUp.enabled = true;
	flagDown.enabled = false;
	
    }
}
