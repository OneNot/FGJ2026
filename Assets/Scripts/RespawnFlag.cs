using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnFlag : MonoBehaviour
{
    private RespawnHelper respawnHelper;
    [SerializeField]
    private MeshRenderer flagUp;
    [SerializeField]
    private MeshRenderer flagDown;

    [SerializeField]
    private bool isWinFlag = false;

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
            if(isWinFlag)
            {
                GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().ShowWinScreen();
                GameObject.FindGameObjectsWithTag("SoundManager")[0].GetComponent<PlaySound>().VictorySound();
                Time.timeScale = 0f;
            }
        }
    }
    public void ToggleFlag() {
        flagUp.enabled = true;
        flagDown.enabled = false;
    }
}
