using System;
using System.Collections;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Rigidbody rb;
    private Coroutine reEnableCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetPlayerCollisionAllowed(bool allowed, float time = 0f)
    {
        rb.excludeLayers = allowed ? LayerMask.GetMask("Nothing") : LayerMask.GetMask("Player");

        if(time > 0f)
        {
            if(reEnableCoroutine != null)
            {
                StopCoroutine(reEnableCoroutine);
            }   
            reEnableCoroutine = StartCoroutine(IE_ReEnablePlayerCollision(time));
        }
    }

    private IEnumerator IE_ReEnablePlayerCollision(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetPlayerCollisionAllowed(true);
    }
}
