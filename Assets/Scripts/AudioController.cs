using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource1;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.ReadValue() == 1)
        {
            if (audioSource1.isPlaying == false)
            {
                audioSource1.Play();               
            }          
        }
        else
        {
            audioSource1.Stop();
        }
    }
}
