using UnityEngine;
using UnityEngine.InputSystem;

public class InputProbe : MonoBehaviour
{
    void Update()
    {
        // Raw device test (bypasses action assets entirely)
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            Debug.Log("PROBE: Keyboard key pressed");

        if (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)
            Debug.Log("PROBE: Gamepad A pressed");
    }
}
