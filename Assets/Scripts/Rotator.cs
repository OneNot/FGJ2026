using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Rotator : MonoBehaviour
{
    [SerializeField] public float RotSpeed;
    [SerializeField] public GameObject rotator;
    [SerializeField] private Vector3 rotType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Update()
    {
        transform.RotateAround(rotator.transform.position, rotType, RotSpeed * Time.deltaTime);
    }
}
