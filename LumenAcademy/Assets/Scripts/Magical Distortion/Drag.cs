
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
public class Drag : MonoBehaviour
{
    public Camera mainCamera;

    private Vector3 offset;
    private float cameraDistance;
    private bool isDragging = false;
    
    public Transform targetCircle; 
    public float checkDelay = 1f; 
    public float snapDistance = 10f; 

    public UnityEvent onMatched;
    private Coroutine checkCoroutine; //sets up coroutine 
    void OnMouseDown()
    {
        if(mainCamera == null) mainCamera = Camera.main; //sets main cam
        isDragging = true;

        if(checkCoroutine != null)
        {
            StopCoroutine(checkCoroutine);
            checkCoroutine = null;
        }

         
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


    void OnMouseUp()
    {
        isDragging = false;

        // checks after releasing the mouse
        if(checkCoroutine != null)StopCoroutine(checkCoroutine);

        checkCoroutine = StartCoroutine(CheckPosition());
    }

    IEnumerator CheckPosition()
    {
        //wait until the player has stopped interacting
        yield return new WaitForSeconds(checkDelay);

        //compare the positions
        float distance = Vector3.Distance(transform.position,targetCircle.position);

        //snap if in ragne
        if(distance <= snapDistance)
        {
            transform.position = targetCircle.position;

            onMatched.Invoke(); //calls event
            Debug.Log("In Range");
        }

        checkCoroutine = null;
    }
}
