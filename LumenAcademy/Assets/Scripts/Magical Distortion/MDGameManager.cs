using UnityEngine;

[CreateAssetMenu(fileName = "MDGameManager", menuName = "Scriptable Objects/MDGameManager")]
public class MDGameManager : MonoBehaviour
{
    [SerializeField] GameObject locations; //set to array when multiple lens are added.
    [SerializeField] GameObject moveableLens;

    private Vector2 location;
    private Vector2 offset = new Vector2(0f, 0f); //temporary offset

    void Start()
    {
        location = (Vector2)(locations.transform.position) + offset; //needs offset, later on consider different aspect ratios
    }
    void Update()
    {
        //once the lens stops moving, it will check if the lens is in the correct position.
        //Quick check for now, timing will need to be adjusted, 
        //it should only check when not in movement for a certain duration
        if ((Vector2)moveableLens.transform.position == location) 
        {
            Debug.Log("Found Location");
        }

    }

}
