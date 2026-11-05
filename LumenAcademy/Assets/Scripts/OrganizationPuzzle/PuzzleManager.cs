
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public BoxCollider2D puzzleBox;
    public ObjectDrag[] puzzleObjects;

    private bool puzzleCompleted = false;

    // Check whether an object is completely inside the puzzle box
    public bool IsInsideBox(ObjectDrag item)
    {
        if (item == null || puzzleBox == null)
        {
            return false;
        }

        Collider2D itemCollider =
            item.GetComponent<Collider2D>();

        if (itemCollider == null)
        {
            return false;
        }

        Bounds boxBounds = puzzleBox.bounds;
        Bounds itemBounds = itemCollider.bounds;

        return boxBounds.Contains(itemBounds.min) &&
               boxBounds.Contains(itemBounds.max);
    }

    // Check whether two objects overlap
    public bool IsOverlapping(ObjectDrag itemA, ObjectDrag itemB)
    {
        if (itemA == null || itemB == null)
        {
            return false;
        }

        Collider2D colliderA =
            itemA.GetComponent<Collider2D>();

        Collider2D colliderB =
            itemB.GetComponent<Collider2D>();

        if (colliderA == null || colliderB == null)
        {
            return false;
        }

        return colliderA.Distance(colliderB).isOverlapped;
    }

    // Check object placement after dragging
    public void CheckObjectPlacement(ObjectDrag item)
    {
        if (item == null || item.IsLocked)
        {
            return;
        }

        Physics2D.SyncTransforms();

        // Check whether the object is inside the puzzle box
        if (!IsInsideBox(item))
        {
            item.ResetPosition();
            Debug.Log("Object returned: Outside the box.");
            return;
        }

        // Check whether the object overlaps with another object
        foreach (ObjectDrag other in puzzleObjects)
        {
            if (other == null || other == item)
            {
                continue;
            }

            if (IsOverlapping(item, other))
            {
                item.ResetPosition();
                Debug.Log("Object returned: Overlapping.");
                return;
            }
        }

        // Try to snap the object to its target position and rotation
        if (item.TrySnapToTarget())
        {
            Physics2D.SyncTransforms();

            Debug.Log(item.name + " snapped and locked!");

            CheckPuzzleCompletion();
        }
        else
        {
            Debug.Log("Object placed, but not in the correct position.");
        }
    }

    // Check whether all puzzle objects are locked
    private void CheckPuzzleCompletion()
    {
        if (puzzleCompleted)
        {
            return;
        }

        foreach (ObjectDrag item in puzzleObjects)
        {
            if (item == null || !item.IsLocked)
            {
                return;
            }
        }

        puzzleCompleted = true;

        Debug.Log("Puzzle Completed!");
    }
}
