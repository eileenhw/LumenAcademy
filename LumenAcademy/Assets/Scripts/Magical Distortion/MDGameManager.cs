using UnityEngine;

[CreateAssetMenu(fileName = "MDGameManager", menuName = "Scriptable Objects/MDGameManager")]
public class MDGameManager : MonoBehaviour
{
    [SerializeField] Transform locations; //set to array when multiple lens are added.
    [SerializeField] GameObject moveableLens;


    private Vector2 location;
    private Vector2 offset = new Vector2(0f, 0f); //temporary offset
    public float range = 0.5f; //temporary range, will need to be adjusted for different aspect ratios
    void Start()
    {
        location = (Vector2)(locations.transform.position) + offset; //needs offset, later on consider different aspect ratios
    }
    void Update()
    {
        //once the lens stops moving, it will check if the lens is in the correct position.
        //Quick check for now, timing will need to be adjusted, 
        //it should only check when not in movement for a certain duration
        /* if ((Vector2)moveableLens.transform.position == location) 
        {
            Debug.Log("Found Location");
        } */
        if(InRange((location, range)))
        {
            Debug.Log("Location In range");
        }

    }

    bool InRange((Vector2, float) locationRange)
    {
        Vector2 location = locationRange.Item1;
        float range = locationRange.Item2;

        Vector2 lensPosition = (Vector2)moveableLens.transform.position;

        return Mathf.Abs(lensPosition.x - location.x) <= range && Mathf.Abs(lensPosition.y - location.y) <= range;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)(locations.transform.position) + offset, new Vector3(range * 2, range * 2, 0f));
    }
}
