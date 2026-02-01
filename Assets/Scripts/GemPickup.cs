using UnityEngine;

public class GemPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().AddGem();
            Destroy(gameObject);
        }
    }
}
