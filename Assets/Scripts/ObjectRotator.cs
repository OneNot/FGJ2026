using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 30f; // degrees per second
    
    [SerializeField]
    private float bobSpeed = 2f; // cycles per second
    
    [SerializeField]
    private float bobDistance = 0.5f; // how far up and down to bob (in units)

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Rotate around Y axis
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        
        // Bob up and down using cosine wave (starts at lowest point)
        float bobOffset = -Mathf.Cos(Time.time * bobSpeed * Mathf.PI) * bobDistance;
        transform.position = startPosition + Vector3.up * bobOffset;
    }
}
