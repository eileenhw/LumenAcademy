
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class LocationHandler : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    public RectTransform targetImage;
    public RectTransform draggableImage;
    public RectTransform targetCircle;
    public RectTransform draggableCircle;

    [Header("Settings")]
    public float checkDelay = 1f;
    public float snapDistance = 30f;

    [Header("Completion")]
    public UnityEvent onMatched;

    private Vector3 dragOffset;
    private Coroutine checkCoroutine;

    // Called when the player starts dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (checkCoroutine != null)
        {
            StopCoroutine(checkCoroutine);
            checkCoroutine = null;
        }

        Vector2 pointerPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            draggableImage,
            eventData.position,
            eventData.pressEventCamera,
            out pointerPosition
        );

        dragOffset = draggableCircle.localPosition -
                     (Vector3)pointerPosition;
    }

    // Called while the player drags
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pointerPosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            draggableImage,
            eventData.position,
            eventData.pressEventCamera,
            out pointerPosition))
        {
            draggableCircle.localPosition =
                (Vector3)pointerPosition + dragOffset;
        }
    }

    // Called when the player releases the mouse
    public void OnEndDrag(PointerEventData eventData)
    {
        if (checkCoroutine != null)
            StopCoroutine(checkCoroutine);

        checkCoroutine = StartCoroutine(CheckAfterDelay());
    }

    private IEnumerator CheckAfterDelay()
    {
        yield return new WaitForSeconds(checkDelay);

        // Get the target's normalized position
        Vector3 targetLocal =
            targetImage.InverseTransformPoint(targetCircle.position);

        Vector2 targetNormalized = new Vector2(
            (targetLocal.x - targetImage.rect.xMin) /
            targetImage.rect.width,

            (targetLocal.y - targetImage.rect.yMin) /
            targetImage.rect.height
        );

        // Convert that position to the draggable image's coordinates
        Vector3 correctPosition = new Vector3(
            draggableImage.rect.xMin +
            targetNormalized.x * draggableImage.rect.width,

            draggableImage.rect.yMin +
            targetNormalized.y * draggableImage.rect.height,

            draggableCircle.localPosition.z
        );

        // Calculate distance between the two circles
        float distance = Vector2.Distance(
            draggableCircle.localPosition,
            correctPosition
        );

        // Snap if close enough
        if (distance <= snapDistance)
        {
            draggableCircle.localPosition = correctPosition;

            // Trigger puzzle completion
            onMatched.Invoke();
        }

        checkCoroutine = null;
    }
}
