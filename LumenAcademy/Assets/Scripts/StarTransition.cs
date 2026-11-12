using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inside : MonoBehaviour
{
    public RectTransform starAsset;
    public float growDuration = 0.80f;
    private float fullSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (TransitionManager.enteredDoor)
        {
            fullSize = starAsset.sizeDelta.x;
            starAsset.sizeDelta = Vector2.zero;
            StartCoroutine(TransitionFromOutside());
            TransitionManager.enteredDoor = false;
        }
    }

    private IEnumerator TransitionFromOutside()
    {
        float startSize = starAsset.sizeDelta.x;
        yield return null;
        float timer = 0;
        while (timer < growDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / growDuration;
            float size = Mathf.SmoothStep(0f, fullSize, progress);
            starAsset.sizeDelta = new Vector2(size, size);
            yield return null;
        }
        starAsset.sizeDelta = new Vector2(fullSize, fullSize);
    }
}