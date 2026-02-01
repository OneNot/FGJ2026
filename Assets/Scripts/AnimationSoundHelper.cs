using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationSoundHelper : MonoBehaviour
{
	[SerializeField] private PlayerController pc;
	public void PlayFootStepSound() {
		pc.Footstep();
	}
	
}
