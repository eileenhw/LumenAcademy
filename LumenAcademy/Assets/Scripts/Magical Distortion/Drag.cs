
using UnityEngine;

public class Drag : MonoBehaviour
{
    public Camera mainCamera;

    private Vector3 offset;
    private float cameraDistance;

    void OnMouseDown()
    {
        if(mainCamera == null) mainCamera = Camera.main; //sets main cam

        cameraDistance = Vector3.Dot(
            transform.position - mainCamera.transform.position,
            mainCamera.transform.forward
        );

        Vector3 mousePosition =Input.mousePosition;//gets the current mouse position 
        mousePosition.z = cameraDistance;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        offset = transform.position-worldPosition; //calc offset
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = Input.mousePosition; 
        mousePosition.z = cameraDistance;

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition); //sets the position of the object to the current mouse location

        transform.position = worldPosition + offset; //applies offset
    }
}
