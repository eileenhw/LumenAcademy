using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject uiHolder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        uiHolder.SetActive(true);
        Debug.Log("UI Activated");
        
    }
}
