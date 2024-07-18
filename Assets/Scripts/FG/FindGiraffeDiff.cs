// 기린의 다른 점을 찾아줘! 를 위한 스크립트
// 기린의 다른 점을 찾아서 클릭하면, 다른 점을 찾았다는 메시지를 띄워준다.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;


public class FindGiraffeDiff : MonoBehaviour
{
    // 초기 시간, 종료 시간을 저장 할 변수
    private int startTime;
    private float endTime;

    // 시도 횟수를 저장 할 변수
    private int tryCount = 0;
    //집중점수 변수
    //private float attentionScore;

    private GameObject msg_congrate;
    private GameObject msg_retry;

    public AudioSource backgroundMusicSource;

    public AudioClip failSound; // 실패 시 재생할 음성

    public AudioClip successSound; // 성공 시 재생할 음성

    public string nextSceneName; // 전환할 씬 이름

    private AudioSource audioSource;



    private int answer;


    // Start is called before the first frame update
    void Start()
    {
        // 시작 시간 저장
        startTime = int.Parse(DateTime.Now.ToString("HHmmss"));

        // 메시지 오브젝트 설정
        msg_congrate = GameObject.Find("GiraffeDiffCanvas/msg_congrate");
        msg_retry = GameObject.Find("GiraffeDiffCanvas/msg_retry");

        // msg_congrate, msg_retry 오브젝트를 비활성화
        msg_congrate.SetActive(false);
        msg_retry.SetActive(false);

        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();

        GameObject bgmObject = GameObject.Find("AudioManager");
        backgroundMusicSource = bgmObject.GetComponent<AudioSource>();


        // 난수 생성
        // 범위는 1 ~ 15
        answer = UnityEngine.Random.Range(1, 16);

        // btn_giraffe_dot_1~15와 btn_giraffe_dot_dummy_1~5, btn_clean_giraffe를 씬에서 찾은 다음, 동일한 배열에 저장
        GameObject[] btn_giraffe = new GameObject[21];

        for (int i = 0; i <= 14; i++)
        {
            btn_giraffe[i] = GameObject.Find("GiraffeDiffCanvas/btn_giraffe_dot_" + (i + 1));
        }

        for (int i = 0; i <= 4; i++)
        {
            btn_giraffe[i + 15] = GameObject.Find("GiraffeDiffCanvas/btn_giraffe_dot_dummy_" + (i + 1));
        }

        btn_giraffe[20] = GameObject.Find("GiraffeDiffCanvas/btn_clean_giraffe");

        // 오브젝트를 찾았는지 확인
        for (int i = 0; i <= 20; i++)
        {
            if (btn_giraffe[i] == null)
            {
                Debug.LogError("Can't find object for index: " + i);
                return;
            }
        }

        // answer에 따라 정답인 버튼을 투명하게 변경
        btn_giraffe[answer].GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);

        // 각 버튼에 대한 클릭 이벤트 추가
        for (int i = 0; i <= 20; i++)
        {
            int index = i;
            btn_giraffe[i].GetComponent<Button>().onClick.AddListener(() => CheckAnswer(index));
        }


    }

    // 정답, 오답 판정
    public void CheckAnswer(int selected)
    {
        // 디버깅 메시지
        // Debug.Log("selected: " + selected + ", answer: " + answer);

        // 시도 횟수 증가
        tryCount++;

        // 정답 판정
        if (selected == answer)
        {
            msg_retry.SetActive(false);
            // 정답 판정 시, msg_congrate 오브젝트를 활성화
            msg_congrate.SetActive(true);

            // msg_congrate 오브젝트를 1초 후 비활성화
            StartCoroutine(DisableMsgCongrate());

            IEnumerator DisableMsgCongrate()
            {
                backgroundMusicSource.Stop();
                audioSource.clip = successSound;
                audioSource.Play();
                msg_congrate.SetActive(true);

                yield return new WaitForSeconds(successSound.length);
                
            }

            // 종료 시간 저장
            endTime = int.Parse(DateTime.Now.ToString("HHmmss"));

            ProgressScoreManager.Instance.CalculateProgressScore("fg", 2, startTime, endTime, tryCount);

            // 게임 종료 코드 추가
        }
        // 오답 판정
        else
        {
            // 오답 판정 시, msg_retry 오브젝트를 활성화하고, 1초 후 비활성화
            msg_congrate.SetActive(false);
            msg_retry.SetActive(true);

            StartCoroutine(DisableMsgRetry());

            IEnumerator DisableMsgRetry()
            {
                audioSource.clip = failSound;
                audioSource.Play();
                // 실패 음성 길이만큼 대기
                yield return new WaitForSeconds(failSound.length);

                msg_retry.SetActive(false);
            }
        }
    }
}
