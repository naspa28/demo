using Firebase.Extensions;
using Firebase.Firestore;
using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AssetCheck : MonoBehaviour
{
    FirebaseFirestore db;
    FirebaseStorage storage;
    public LoadingUIController loadingUIController;  // Reference to the LoadingUIController
    private bool checkSuccess = true;

    void Start()
    {
        StartCoroutine(WaitForFirebaseInitialization());
    }

    IEnumerator WaitForFirebaseInitialization()
    {
        // Wait until Firebase is initialized
        while (!FirebaseManager.Instance.IsInitialized)
        {
            yield return null;
        }

        db = FirebaseManager.Instance.Firestore;
        storage = FirebaseStorage.DefaultInstance;
        StartCoroutine(CheckAssets());
    }

    IEnumerator CheckAssets()
    {
        loadingUIController.ShowLoadingUI("Checking assets, please wait...");

        // Fetch the document with the list of files
        DocumentReference docRef = db.Collection("assets").Document("ogYEeZKLCywmfOOB8WaE"); // replace with your document ID
        var task = docRef.GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsCompleted && !task.IsFaulted)
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> data = snapshot.ToDictionary();
                List<object> files = data["files"] as List<object>;

                foreach (string filePath in files)
                {
                    yield return CheckFileMetadata(filePath);
                }
            }
            else
            {
                Debug.LogError("Document does not exist.");
                checkSuccess = false;
            }
        }
        else
        {
            Debug.LogError("Failed to retrieve document: " + task.Exception.ToString());
            checkSuccess = false;
        }

        if (checkSuccess)
        {
            loadingUIController.UpdateExplanationText("Touch the screen to continue...", true, true);
        }
        else
        {
            loadingUIController.UpdateExplanationText("Asset check failed. Please try again.", true, false);
        }
    }

    IEnumerator CheckFileMetadata(string filePath)
    {
        StorageReference fileRef = storage.GetReference(filePath);
        var task = fileRef.GetMetadataAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsCompleted && !task.IsFaulted)
        {
            StorageMetadata metadata = task.Result;

            string updated = metadata.UpdatedTimeMillis.ToString();
            Debug.Log("Name: " + metadata.Name);
            Debug.Log("Size: " + metadata.SizeBytes + " bytes");
            Debug.Log("Updated: " + updated);
            Debug.Log("Content Type: " + metadata.ContentType);

            // Check if the file needs to be downloaded
            CheckIfFileNeedsUpdate(filePath, updated);
        }
        else
        {
            Debug.LogError("Failed to retrieve metadata: " + task.Exception.ToString());
            checkSuccess = false;
        }
    }

    void CheckIfFileNeedsUpdate(string filePath, string updated)
    {
        // Compare this with local metadata to determine if the file has changed
        string localLastUpdated = PlayerPrefs.GetString(filePath + "_updated", "");

        if (updated != localLastUpdated)
        {
            Debug.Log("File " + filePath + " has been updated. Downloading...");
            DownloadFile(filePath);
            PlayerPrefs.SetString(filePath + "_updated", updated);
        }
    }

    void DownloadFile(string filePath)
    {
        Debug.Log("Downloading file: " + filePath);

        StorageReference fileRef = storage.GetReference(filePath);
        fileRef.GetBytesAsync(long.MaxValue).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                byte[] fileContents = task.Result;
                string localPath = Application.persistentDataPath + "/" + filePath;
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(localPath)); // Ensure the directory exists
                System.IO.File.WriteAllBytes(localPath, fileContents);
                Debug.Log("File downloaded and saved: " + filePath);
            }
            else
            {
                Debug.LogError("Failed to download file: " + task.Exception.ToString());
                checkSuccess = false;
            }
        });
    }
}
