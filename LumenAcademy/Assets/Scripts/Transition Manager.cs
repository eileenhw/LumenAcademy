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
    public GameObject proximityDialogue;
    public RectTransform starAsset;
    public float shrinkDuration = 0.80f;
    public static bool enteredDoor = false;

    public Dialogue dialogue;
    public Transform cam;
    public float playerActivateDistance;
    bool active = false;

    private void Update()
    {
        RaycastHit hit;
        active = Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, playerActivateDistance);
        proximityText.SetActive(false);
        proximityDialogue.SetActive(false);

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
        } else if (active == true && hit.transform.CompareTag("NPC") == true && !dialogue.gameObject.activeSelf)
        {
            proximityDialogue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E) && active == true && hit.transform.CompareTag("NPC") == true)
            {
                if (hit.transform != null)
                {
                    dialogue.startDialogue();
                }
            }
        }
    }

    private IEnumerator TransitionToInside()
    {
        float startSize = starAsset.sizeDelta.x;
        yield return null;
        float timer = 0;
        while (timer < shrinkDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / shrinkDuration;
            float size = Mathf.SmoothStep(startSize, 0f, progress);
            starAsset.sizeDelta = new Vector2(size, size);
            yield return null;
        }
        starAsset.sizeDelta = Vector2.zero;
        yield return new WaitForSeconds(0.2f);
        enteredDoor = true;
        SceneManager.LoadScene(insideexample);
    }
}
