
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    [Header("Target Settings")]
    public Vector2 targetPosition;
    public float targetRotation;

    [Header("Snap Settings")]
    public float positionTolerance = 0.5f;
    public float rotationTolerance = 5f;

    private Camera mainCamera;
    private Vector3 dragOffset;
    private Vector3 startPosition;

    private SpriteRenderer spriteRenderer;
    private int originalSortingOrder;

    private bool isDragging = false;
    private bool isLocked = false;

    public bool IsLocked => isLocked;

    private void Awake()
    {
        mainCamera = Camera.main;
        startPosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalSortingOrder = spriteRenderer.sortingOrder;
        }
    }

    private void OnMouseDown()
    {
        if (isLocked)
        {
            return;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            return;
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z;

        Vector3 worldPosition =
            mainCamera.ScreenToWorldPoint(mousePosition);

        dragOffset = transform.position - worldPosition;
        isDragging = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 100;
        }
    }

    private void OnMouseOver()
    {
        if (isLocked)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            transform.Rotate(0f, 0f, 90f);
        }
    }

    private void OnMouseDrag()
    {
        if (isLocked || !isDragging || mainCamera == null)
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
        isDragging = false;

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = originalSortingOrder;
        }

        if (isLocked)
        {
            return;
        }

        PuzzleManager manager =
            FindFirstObjectByType<PuzzleManager>();

        if (manager != null)
        {
            manager.CheckObjectPlacement(this);
        }
    }

    // Check whether the object is close enough to its target
    public bool TrySnapToTarget()
    {
        if (isLocked)
        {
            return false;
        }

        float distance = Vector2.Distance(
            transform.position,
            targetPosition
        );

        float angleDifference = Mathf.Abs(
            Mathf.DeltaAngle(
                transform.eulerAngles.z,
                targetRotation
            )
        );

        if (distance <= positionTolerance &&
            angleDifference <= rotationTolerance)
        {
            transform.position = new Vector3(
                targetPosition.x,
                targetPosition.y,
                startPosition.z
            );

            transform.rotation = Quaternion.Euler(
                0f,
                0f,
                targetRotation
            );

            isLocked = true;

            return true;
        }

        return false;
    }

    public void ResetPosition()
    {
        if (!isLocked)
        {
            transform.position = startPosition;
        }
    }
}
