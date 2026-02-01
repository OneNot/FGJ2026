using System.Linq;
using UnityEngine;

public class MaskCollisionChecker : MonoBehaviour
{
    [SerializeField]
    private float rayCastDistance = 0.1f;


    void Update()
    {
        CheckForGround();
    }

    //TODO: Multiple casts. (Maybe one from each leg)
    private void CheckForGround()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayCastDistance, Color.red); //debug line
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, rayCastDistance, LayerMask.GetMask("MaskedObject")))
        {
            Vector2 textureCoord = hitInfo.textureCoord;

            Material[] materials = hitInfo.collider.gameObject.GetComponent<Renderer>().materials;
            foreach (Material mat in materials)
            {
                Texture2D mask = mat.GetTexture("_OpacityMask") as Texture2D;
                if (mask != null)
                {
                    Color pixelColor = mask.GetPixelBilinear(textureCoord.x, textureCoord.y);
                    if(pixelColor.r < 0.5f) //assuming black areas are transparent
                    {
                        hitInfo.collider.gameObject.GetComponent<InteractableObject>().SetPlayerCollisionAllowed(false, 1f);
                    }
                    else
                    {
                        hitInfo.collider.gameObject.GetComponent<InteractableObject>().SetPlayerCollisionAllowed(true); 
                    }
                    return; //exit after first found mask
                }
            }
        }
    }
}
