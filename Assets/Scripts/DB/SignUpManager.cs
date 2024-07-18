using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignUpManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nickname;
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;

    FirebaseAuth auth;
    FirebaseFirestore db;

    private void Awake()
    {
        auth = FirebaseManager.Instance.Auth;
        db = FirebaseManager.Instance.Firestore;
    }

    public void SignUp()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            FirebaseUser newUser = task.Result.User;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            
            // Create Firestore document with the user's UID as the document ID
            CreateUserDocument(newUser.UserId, nickname.text, 0);
        });
    }

    private void CreateUserDocument(string userId, string nickname, int gen_prog)
    {
        DocumentReference docRef = db.Collection("users").Document(userId);

        // Create an anonymous object with multiple fields
        var userData = new
        {
            nickname,
            gen_prog,
            games = new
            {
                vm = new { level = 0, star = 0, prog = 0 },
                fg = new { level = 0, star = 0, prog = 0 },
                pc = new { level = 0, star = 0, prog = 0 },
                sp = new { level = 0, star = 0, prog = 0 },
                sr = new { level = 1, star = 0, prog = 0 },
            },
            tutorial = true
        };

        docRef.SetAsync(userData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SetAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SetAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("User document successfully created in Firestore.");
            SceneManager.LoadScene("SignInScene");
        });
    }
}
