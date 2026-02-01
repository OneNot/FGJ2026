using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnHelper : MonoBehaviour
{
    [SerializeField]private Vector3 lastRespawnPosition;
    [SerializeField]private RespawnFlag previousFlag;
    [SerializeField]private CharacterController cc;

    void Start()
    {
	UpdateRespawnPosition(null);
	cc = gameObject.GetComponent<CharacterController>();
    }

    public void UpdateRespawnPosition(RespawnFlag prev) {
	lastRespawnPosition = gameObject.transform.position;
	previousFlag = prev;
	if(previousFlag != null)
	    previousFlag.ToggleFlag();
    }

    public void ReturnToRespawn() {
	cc.enabled = false;
	gameObject.transform.position = lastRespawnPosition; 
	cc.enabled = true;
    }
}
