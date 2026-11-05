
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public BoxCollider2D puzzleBox;
    public ObjectDrag[] puzzleObjects;

    // Check whether an object is completely inside the puzzle box
    public bool IsInsideBox(ObjectDrag item)
    {
        if (item == null || puzzleBox == null)
        {
            return false;
        }

        // Support Box Collider 2D and Polygon Collider 2D
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
        if (item == null)
        {
            return;
        }

        // Make sure physics uses the latest Transform positions
        Physics2D.SyncTransforms();

        // Check if the object is inside the puzzle box
        if (!IsInsideBox(item))
        {
            item.ResetPosition();
            Debug.Log("Object returned: Outside the box.");
            return;
        }

        // Check if the object overlaps with another object
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

        Debug.Log("Object placed successfully!");
    }
}
