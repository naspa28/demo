using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    private void Awake()
    {
        LocalDataManager.Instance.AddGameSession("sp", "20200811", 1, 2, 3, 87, 50, 180, 90);
        FetchGameData();
    }

    private void FetchGameData()
    {
        UserDataManager.Instance.GetGameData("vm").ContinueWith(task =>
        {
            var (level, star) = task.Result;
            Debug.Log($"Level: {level}, Star: {star}");
        });
    }

}
