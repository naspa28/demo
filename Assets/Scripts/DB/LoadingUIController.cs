using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingUIController : MonoBehaviour
{
    public GameObject loadingAnimation;
    public TextMeshProUGUI explanationText;

    private bool isWaitingForUserInput = false;
    private bool canProceed = false;

    void Start()
    {
        // Hide the loading UI initially
        HideLoadingUI();
    }

    public void ShowLoadingUI(string explanation)
    {
        loadingAnimation.SetActive(true);
        explanationText.gameObject.SetActive(true);
        explanationText.text = explanation;
        isWaitingForUserInput = false;
        canProceed = false;
    }

    public void HideLoadingUI()
    {
        loadingAnimation.SetActive(false);
        explanationText.gameObject.SetActive(false);
    }

    public void UpdateExplanationText(string newExplanation, bool waitForUserInput = false, bool proceed = false)
    {
        explanationText.text = newExplanation;
        isWaitingForUserInput = waitForUserInput;
        canProceed = proceed;

        if (waitForUserInput)
        {
            loadingAnimation.SetActive(false);
        }
    }

    void Update()
    {
        if (isWaitingForUserInput && Input.GetMouseButtonDown(0))
        {
            if (canProceed)
            {
                // Handle screen touch or mouse click if can proceed
                LoadScene();
            }
            else
            {
                Debug.Log("Asset check failed. Cannot proceed.");
                // Optionally, display an error message or retry button
            }
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("SignInScene");
        Debug.Log("Screen touched. Changing screen...");
    }
}
