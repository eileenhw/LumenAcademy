
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 dragOffset;
    private Vector3 startPosition;

    private SpriteRenderer spriteRenderer;
    private int originalSortingOrder;

    private bool isDragging = false;

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
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
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
        if (Input.GetMouseButtonDown(1))
        {
            transform.Rotate(0f, 0f, 90f);
        }
    }

    private void OnMouseDrag()
    {
        if (!isDragging || mainCamera == null)
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

        PuzzleManager manager =
            FindFirstObjectByType<PuzzleManager>();

        if (manager != null)
        {
            manager.CheckObjectPlacement(this);
        }
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}
