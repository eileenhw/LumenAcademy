using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [Header("Scene Transition")]
    [SerializeField] private string insideexample = "insideexample";
    public GameObject proximityText;

    public Transform cam;
    public float playerActivateDistance;
    bool active = false;

    private void Update()
    {
        RaycastHit hit;
        active = Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, playerActivateDistance);
        proximityText.SetActive(false);

        if (active == true && hit.transform.CompareTag("Door") == true)
        {
            proximityText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && active == true && hit.transform.CompareTag("Door") == true)
            {
                if (hit.transform != null)
                {
                    StartCoroutine(TransitionToInside());
                }
            }
        }
    }

    private IEnumerator TransitionToInside()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(insideexample);
    }
}
