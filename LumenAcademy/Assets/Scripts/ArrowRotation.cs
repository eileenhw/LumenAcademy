using UnityEngine;

public class DestinationArrow : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private void Update()
    {
        if (destination == null) return;

        Vector3 direction = destination.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
