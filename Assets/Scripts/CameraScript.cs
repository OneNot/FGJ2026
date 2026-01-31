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
    public float minValue = -180f;
    private float maxValue = 180f;
    public float pitch;
    private float lockedY;
    private PlayerController player;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        target = GameObject.FindWithTag("Player").GetComponent<Transform>(); 
        pitch = Mathf.Clamp(Normalize180(transform.eulerAngles.x), minValue, maxValue);
        lockedY= target.position.y;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(target==null) return;
        Vector3 playerlocation = new Vector3 (target.position.x, lockedY+height,target.position.z);
        Quaternion camerarotation = Quaternion.Euler(pitch,0f,0f);
        Vector3 sightline = playerlocation +(camerarotation*Vector3.back * distance);

        transform.position = Vector3.Lerp (transform.position, sightline, smooth*Time.deltaTime);
        

        transform.rotation = Quaternion.LookRotation(playerlocation - transform.position, Vector3.up);

        if (player.playerGrounded)
        {
            lockedY = Mathf.Lerp(lockedY,target.position.y, 5f * Time.deltaTime);
        }
    }


    void OnTiltUp(InputValue value)
    {
        if(!value.isPressed) return;
        pitch = Mathf.Clamp(pitch+ addpitch,minValue,maxValue);

    }

    void OnTiltDown(InputValue value)
    {
        if(!value.isPressed) return;
        pitch = Mathf.Clamp(pitch-addpitch,minValue,maxValue);

    }

    void OnHeightUp(InputValue value)
    {
        if(!value.isPressed) return;
        height = Mathf.Clamp(height+ heightAdd,minValue,maxValue);

    }

    void OnHeightDown(InputValue value)
    {
        if(!value.isPressed) return;
        height = Mathf.Clamp(height- heightAdd,minValue,maxValue);

    }

    void OnDistanceUp(InputValue value)
    {
        if(!value.isPressed) return;
        distance = Mathf.Clamp(distance+ distanceAdd,minValue,maxValue);

    }

    void OnDistanceDown(InputValue value)
    {
        if(!value.isPressed) return;
        distance = Mathf.Clamp(distance- distanceAdd,minValue,maxValue);

    }



    private static float Normalize180(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
