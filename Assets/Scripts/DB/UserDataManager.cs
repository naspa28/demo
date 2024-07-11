using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance { get; private set; }
    public DocumentSnapshot UserDocument { get; private set; }

    void Awake()
    {
        // Ensure this object persists across scene loads
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetUserDocument(DocumentSnapshot document)
    {
        UserDocument = document;
    }

    public void UpdateLevel(string game, int level, int stars)
    {
        DocumentReference docRef = Instance.UserDocument.Reference;

        docRef.UpdateAsync(new Dictionary<string, object>
        {
            { $"games.{game}.level", level },
            { $"games.{game}.star", stars }
        }).ContinueWithOnMainThread(task =>
        {
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

            Debug.Log("User document successfully updated in Firestore.");
        });
    }

    public Task<(int level, int star)> GetGameData(string game)
    {
        var tcs = new TaskCompletionSource<(int, int)>();

        DocumentReference docRef = Instance.UserDocument.Reference;

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("GetSnapshotAsync was canceled.");
                tcs.SetResult((0, 0));
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("GetSnapshotAsync encountered an error: " + task.Exception);
                tcs.SetResult((0, 0));
                return;
            }

            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> userData = snapshot.ToDictionary();

                if (userData.ContainsKey("games"))
                {
                    var games = userData["games"] as Dictionary<string, object>;
                    if (games.ContainsKey(game))
                    {
                        var gameInfo = games[game] as Dictionary<string, object>;
                        int level = Convert.ToInt32(gameInfo["level"]);
                        int star = Convert.ToInt32(gameInfo["star"]);

                        tcs.SetResult((level, star));
                        return;
                    }
                }
            }

            Debug.Log("No data found for the specified game.");
            tcs.SetResult((0, 0));
        });

        return tcs.Task;
    }

}
