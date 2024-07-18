using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignInManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;

    FirebaseAuth auth;
    FirebaseFirestore db;

    private void Awake()
    {
        auth = FirebaseManager.Instance.Auth;
        db = FirebaseManager.Instance.Firestore;
    }
    
    public void SignIn()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser user = task.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);

            LocalDataManager.Instance.Initialize();
            LoadUserDocument(user.UserId);
        });
    }

    private void LoadUserDocument(string userId)
    {
        DocumentReference docRef = db.Collection("users").Document(userId);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("GetSnapshotAsync encountered an error: " + task.Exception);
                return;
            }

            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                UserDataManager.Instance.SetUserDocument(snapshot);
                if (UserDataManager.Instance.UserDocument.GetValue<bool>("tutorial"))
                {
                    UpdateTutorialField(false, docRef);
                    SceneManager.LoadScene("TutorialScene");
                }
                else
                {
                    SceneManager.LoadScene("MainScene");
                }
            }
            else
            {
                Debug.LogError("User document does not exist!");
            }
        });
    }

    public void UpdateTutorialField(bool newValue, DocumentReference UserDocument)
    {
        if (UserDocument == null)
        {
            Debug.LogError("UserDocument is not set.");
            return;
        }

        UserDocument.UpdateAsync(new Dictionary<string, object> {
            { "tutorial", newValue }
        }).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("UpdateAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("UpdateAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("Tutorial field updated successfully.");
        });
    }
}