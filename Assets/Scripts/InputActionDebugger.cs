using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionDebugger : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset asset;

    void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
    }

    void OnDisable()
    {
        InputSystem.onActionChange -= OnActionChange;
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (obj is InputActionMap map)
        {
            if (change == InputActionChange.ActionMapEnabled)
                Debug.Log($"[InputDebugger] ActionMap enabled: {map.name}");
            else if (change == InputActionChange.ActionMapDisabled)
                Debug.Log($"[InputDebugger] ActionMap disabled: {map.name}");
        }
        else if (obj is InputAction action)
        {
            if (change == InputActionChange.ActionEnabled)
                Debug.Log($"[InputDebugger] Action enabled: {action.name}");
            else if (change == InputActionChange.ActionDisabled)
                Debug.Log($"[InputDebugger] Action disabled: {action.name}");
        }
        else if (obj is InputActionAsset a)
        {
            if (change == InputActionChange.ActionEnabled)
                Debug.Log($"[InputDebugger] Asset enabled: {a.name}");
            else if (change == InputActionChange.ActionDisabled)
                Debug.Log($"[InputDebugger] Asset disabled: {a.name}");
        }
    }
}
