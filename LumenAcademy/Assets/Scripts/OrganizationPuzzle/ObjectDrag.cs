
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 dragOffset;
    private Vector3 startPosition;

    [Header("Puzzle Settings")]
    public Vector3 correctPosition;
    public float positionTolerance = 0.6f;

    private void Awake()
    {
        mainCamera = Camera.main;
        startPosition = transform.position;
    }

    private void OnMouseDown()
{
    if (Input.GetMouseButtonDown(0))
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z;

        Vector3 worldPosition =
            mainCamera.ScreenToWorldPoint(mousePosition);

        dragOffset = transform.position - worldPosition;
    }
}

    private void OnMouseOver()
{
    if (Input.GetMouseButtonDown(1))
    {
        transform.Rotate(0f, 0f, 90f);
    }
}

    private void OnMouseDrag()
    {
        if (!Input.GetMouseButton(0))
    {
        return;
    }
        if (mainCamera == null)
        {
            return;
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z;

        Vector3 worldPosition =
            mainCamera.ScreenToWorldPoint(mousePosition);

        transform.position = new Vector3(
            worldPosition.x + dragOffset.x,
            worldPosition.y + dragOffset.y,
            startPosition.z
        );
    }

    private void OnMouseUp()
    {
        PuzzleManager manager =
            FindFirstObjectByType<PuzzleManager>();

        if (manager != null)
        {
            manager.CheckPuzzle();
        }
    }

    public bool IsAtCorrectPosition()
    {
        return Vector2.Distance(
            transform.position,
            correctPosition
        ) <= positionTolerance;
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}
