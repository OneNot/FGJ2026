using UnityEngine;

public class GemPickup : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        GameObject SoundManager = GameObject.Find("SoundManager");
        PlaySound playSound = SoundManager.GetComponent<PlaySound>();
        if (other.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().AddGem();
            playSound.GemPickSound();
            Destroy(gameObject);
        }
    }
}
