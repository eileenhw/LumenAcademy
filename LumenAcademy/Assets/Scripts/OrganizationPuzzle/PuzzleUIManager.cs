
using UnityEngine;

public class PuzzleUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject pausePanel;
    public GameObject successPanel;

    private bool isPaused = false;

    private void Start()
    {
        startPanel.SetActive(true);
        pausePanel.SetActive(false);
        successPanel.SetActive(false);

        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!startPanel.activeSelf && !successPanel.activeSelf)
            {
                PausePuzzle();
            }
        }
    }

    public void StartPuzzle()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PausePuzzle()
    {
        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumePuzzle()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowSuccess()
    {
        successPanel.SetActive(true);
        pausePanel.SetActive(false);

        isPaused = false;
        Time.timeScale = 0f;
    }

    public void ContinuePuzzle()
    {
        Time.timeScale = 1f;
        Debug.Log("Continue button clicked!");
    }
}
