using UnityEngine;
using UnityEngine.InputSystem;

public class MaskChangerTest : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActionAsset;
    private InputAction jumpAction;

    private Renderer rendererComponent;

    [SerializeField]
    private Texture2D newMaskTexture;
    private Texture2D originalMaskTexture;

    void Awake()
    {
        jumpAction = inputActionAsset.FindAction("Jump");
        rendererComponent = gameObject.GetComponent<Renderer>();
        originalMaskTexture = (Texture2D)rendererComponent.material.GetTexture("_OpacityMask");
    }

    void Update()
    {
        if (jumpAction.triggered)
        {
            if(rendererComponent.material.GetTexture("_OpacityMask") != newMaskTexture) {
                rendererComponent.material.SetTexture("_OpacityMask", newMaskTexture);
                Debug.Log("Mask Changed to New Texture");
            }
            else {
                rendererComponent.material.SetTexture("_OpacityMask", originalMaskTexture);
                Debug.Log("Mask Reverted to Original Texture");
            }
        }
    }
}
