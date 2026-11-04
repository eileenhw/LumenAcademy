
using UnityEngine;

public class LensController : MonoBehaviour
{
    public Camera mainCamera;
    public RenderTexture captureTexture;

    [Range(0f, 20f)]
    public float swirlStrength = 8f;

    private Renderer lensRenderer;
    private MaterialPropertyBlock properties;

    private static readonly int CaptureTexID =Shader.PropertyToID("_CaptureTex");

    private static readonly int LensCenterID =Shader.PropertyToID("_LensCenter");

    private static readonly int SwirlStrengthID = Shader.PropertyToID("_SwirlStrength");

    void Awake()
    {
        lensRenderer = GetComponent<Renderer>();
        properties = new MaterialPropertyBlock();

        if (lensRenderer == null){
            Debug.LogError("No Renderer", this);
        }
    }

    void LateUpdate()
    {
        if (lensRenderer == null ||mainCamera == null || captureTexture == null) return;

        // get the lens position in screen coordinates
        Vector3 center = mainCamera.WorldToViewportPoint(transform.position);

        
        lensRenderer.GetPropertyBlock(properties);//Get existing material properties

        properties.SetTexture(CaptureTexID, captureTexture);// Assign captured image

        //pass lens center to shader
        properties.SetVector( LensCenterID, new Vector4(center.x, center.y, 0, 0));

        properties.SetFloat(SwirlStrengthID, swirlStrength);

        lensRenderer.SetPropertyBlock(properties); //apply properties to this lens only
    }
}
