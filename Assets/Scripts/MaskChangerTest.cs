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
    private Texture2D maskTexture;

    private Color[] originalPixels;
    private bool isOriginal = true;


    void Awake()
    {
        jumpAction = inputActionAsset.FindAction("Jump");
        rendererComponent = gameObject.GetComponent<Renderer>();
        maskTexture = (Texture2D)rendererComponent.material.GetTexture("_OpacityMask");
        originalPixels = maskTexture.GetPixels();
    }

    void Update()
    {
        if (jumpAction.triggered)
        {
            if(isOriginal)
            {
                maskTexture.SetPixels(newMaskTexture.GetPixels());
                maskTexture.Apply();
                isOriginal = false;
            }
            else
            {
                maskTexture.SetPixels(originalPixels);
                maskTexture.Apply();
                isOriginal = true;
            }
        }
    }
}
