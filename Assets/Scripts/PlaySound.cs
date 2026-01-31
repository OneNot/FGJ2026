using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip[] SoundEffects = new AudioClip[3];

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Player") && this.gameObject.tag == "Checkpoint")
        {
            Debug.Log("Here");
            AudioSource.clip = SoundEffects[1];
            AudioSource.Play();
        }       
    }
}
