using UnityEngine;

public class FreezeRotation : MonoBehaviour
{
    [SerializeField] Vector3 StartRotation;
    [SerializeField] bool UseCustomRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!UseCustomRotation)
        {
            StartRotation = gameObject.transform.rotation.eulerAngles;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localRotation = Quaternion.Euler(StartRotation);// = StartRotation;
    }
}
