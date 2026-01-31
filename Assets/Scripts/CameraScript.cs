using System.ComponentModel;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Transform target;
    public float smooth = 5f;
    public float distance = 6f;
    public float height = 1.2f;
    private float addpitch = 5.0f;
    private float distanceAdd = 0.2f;
    private float heightAdd = 0.2f;
    public float minPitch = -180f;
    [SerializeField] private float maxPitch = 180f;
    public float pitch;
    private float lockedY;
    private PlayerController player;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        target = GameObject.FindWithTag("Player").GetComponent<Transform>(); 
        pitch = Mathf.Clamp(Normalize180(transform.eulerAngles.x), minPitch, maxPitch);
        lockedY= target.position.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target==null) return;
        Vector3 pivot = new Vector3 (target.position.x, lockedY+height,target.position.z);
        Quaternion camtarget = Quaternion.Euler(pitch,0f,0f);
        Vector3 newpos = pivot +(camtarget*Vector3.back * distance);

        transform.position = Vector3.Lerp (transform.position, newpos, smooth*Time.deltaTime);
        

        transform.rotation = Quaternion.LookRotation(pivot - transform.position, Vector3.up);

        if (player.playerGrounded)
        {
            lockedY = Mathf.Lerp(lockedY,target.position.y, 5f * Time.deltaTime);
        }
    }


    void OnTiltUp(InputValue value)
    {
        if(!value.isPressed) return;
        pitch = Mathf.Clamp(pitch+ addpitch,minPitch,maxPitch);

    }

    void OnTiltDown(InputValue value)
    {
        if(!value.isPressed) return;
        pitch = Mathf.Clamp(pitch-addpitch,minPitch,maxPitch);

    }

    void OnHeightUp(InputValue value)
    {
        if(!value.isPressed) return;
        height = Mathf.Clamp(height+ heightAdd,minPitch,maxPitch);

    }

    void OnHeightDown(InputValue value)
    {
        if(!value.isPressed) return;
        height = Mathf.Clamp(height- heightAdd,minPitch,maxPitch);

    }

    void OnDistanceUp(InputValue value)
    {
        if(!value.isPressed) return;
        distance = Mathf.Clamp(distance+ distanceAdd,minPitch,maxPitch);

    }

    void OnDistanceDown(InputValue value)
    {
        if(!value.isPressed) return;
        distance = Mathf.Clamp(distance- distanceAdd,minPitch,maxPitch);

    }



    private static float Normalize180(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
