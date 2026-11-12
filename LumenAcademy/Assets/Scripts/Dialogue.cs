using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public Movement movement;

    private int index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

   public void startDialogue()
    {
        movement.enabled = false;
        textComponent.text = string.Empty;
        gameObject.SetActive(true);
        index = 0;
        StartCoroutine(TypeLine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            } else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else
        {
            movement.enabled = true;
            gameObject.SetActive(false);
        }
    }
}
