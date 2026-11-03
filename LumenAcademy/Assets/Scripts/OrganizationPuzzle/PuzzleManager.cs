
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Objects")]
    public ObjectDrag[] puzzleObjects;

    private bool puzzleCompleted = false;

    public void CheckPuzzle()
    {
        if (puzzleCompleted)
        {
            return;
        }

        foreach (ObjectDrag item in puzzleObjects)
        {
            if (item == null || !item.IsAtCorrectPosition())
            {
                return;
            }
        }

        puzzleCompleted = true;
        Debug.Log("Puzzle Completed!");
    }

    public void ResetPuzzle()
    {
        foreach (ObjectDrag item in puzzleObjects)
        {
            if (item != null)
            {
                item.ResetPosition();
            }
        }

        puzzleCompleted = false;
        Debug.Log("Puzzle Reset!");
    }
}
