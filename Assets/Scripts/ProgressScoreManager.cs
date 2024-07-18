using Firebase.Extensions;
using System;
using UnityEngine;

public class ProgressScoreManager : MonoBehaviour
{
    public static ProgressScoreManager Instance { get; private set; }

    private int initialProgressScore;

    // 매개변수로 게임명(스테이지), 레벨, 시작시간, 종료시간, 시도 횟수 받아와서 진척도 계산해서 DB에 저장
    // 진척도는 기본 유저 진척도 - (( 종료시간 - 시작시간 ) / 10 ) - ( (시도 횟수 - 1) * 5 )

    private int gamelevel;
    private int star;
    private int progressScore;

    private int playTime;
    private int tryNumber;
    private int newProgressScore;
    private float correctScore;
    private float attentionScore;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void CalculateProgressScore(string gameName, int level, float startTime, float endTime, int tryCount)
    {
        initialProgressScore = UserDataManager.Instance.UserDocument.GetValue<int>("gen_prog");

        UserDataManager.Instance.GetGameData(gameName).ContinueWith(task =>
        {
            (gamelevel, star, progressScore) = task.Result;
            playTime = (int)(endTime - startTime);
            tryNumber = tryCount - 1;
            newProgressScore = initialProgressScore - (playTime / 10) - (tryNumber * 5);
            correctScore = 1 / tryCount;
            /*attentionScore = TrackingManager.Instance.getAttentionScore();*/
            attentionScore = 0;

            if (gameName == "fg" && level == 0)
            {
                correctScore = 3 / tryCount;
            }

            LocalDataManager.Instance.AddGameSession(gameName, DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"), level, star, tryCount, 1 / (tryCount), playTime, (int) attentionScore);

            progressScore += newProgressScore;

            if (progressScore > 350)
            {
                if (level == 3)
                {
                    return;
                }
                level += 1;
                progressScore = 0;
            }
            else if (progressScore > 250)
            {
                star = 3;
            }
            else if (progressScore > 150)
            {
                star = 2;
            }
            else if (progressScore > 50)
            {
                star = 1;
            }
            else
            {
                star = 0;
            }

            UserDataManager.Instance.UpdateLevel(gameName, gamelevel, star, progressScore);
        });
    }
}
