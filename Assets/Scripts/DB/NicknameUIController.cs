using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class NicknameUIManager : MonoBehaviour
{
    public TextMeshProUGUI nicknameText;

    void Start()
    {
        DocumentSnapshot userDocument = UserDataManager.Instance.UserDocument;

        if (userDocument != null)
        {
            nicknameText.text = "Hi " + userDocument.GetValue<string>("nickname") + "!    Let's enjoy playing the game!";
        }
        else
        {
            Debug.LogError("User document is not available!");
        }
    }
}
