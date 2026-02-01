using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip[] SoundEffects = new AudioClip[3];

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player") && this.gameObject.tag == "Checkpoint")
        {
            Debug.Log("kaksi");
            AudioSource.clip = SoundEffects[1];
            AudioSource.Play();
        }   
    }

    public void GemPickSound()
    {
        AudioSource.PlayOneShot(SoundEffects[2]);
    }

    public void VictorySound()
    {
        AudioSource.PlayOneShot(SoundEffects[0]);
    }

    public void ExclamationSound()
    {
        AudioSource.PlayOneShot(SoundEffects[3]);
    }

}
