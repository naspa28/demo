// 다람쥐 집 빠진 부분을 찾아줘! 에서 사용되는 스크립트
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FindDiffHouse : MonoBehaviour
{
    // 초기 시간, 종료 시간을 저장 할 변수
    private int startTime;
    private float endTime;

    // 시도 횟수를 저장 할 변수
    private int tryCount = 0;

    // 메시지 오브젝트 설정 
    private GameObject msg_congrate;
    private GameObject msg_retry;

    public AudioSource backgroundMusicSource;

    public AudioClip failSound; // 실패 시 재생할 음성

    public AudioClip successSound; // 성공 시 재생할 음성

    public string nextSceneName; // 전환할 씬 이름

    private AudioSource audioSource;


    // 버튼 오브젝트 설정
    private GameObject btn_option_1;
    private GameObject btn_option_2;
    private GameObject btn_option_1_background;
    private GameObject btn_option_2_background;

    // 이미지 오브젝트 설정
    private GameObject img_roof;
    private GameObject img_window;

    // 문제 배치
    // 난수 생성을 통해 0 ~ 1 숫자 중 정답을 설정
    private int answer;

    void Start()
    {
        // 시작 시간 저장
        startTime = int.Parse(DateTime.Now.ToString("HHmmss"));
        
        // 메시지 오브젝트 설정
        msg_congrate = GameObject.Find("FindDiffHouseCanvas/msg_congrate");
        msg_retry = GameObject.Find("FindDiffHouseCanvas/msg_retry");


        // msg_congrate, msg_retry 오브젝트를 비활성화
        msg_congrate.SetActive(false);
        msg_retry.SetActive(false);

        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();

        GameObject bgmObject = GameObject.Find("AudioManager");
        backgroundMusicSource = bgmObject.GetComponent<AudioSource>();


        // 난수 생성
        answer = UnityEngine.Random.Range(0, 2);

        // 씬에서 모든 btn_option_*과 그 배경을 불러옴
        // img_clean_house안의 img_window, img_roof 또한 불러옴
        btn_option_1 = GameObject.Find("FindDiffHouseCanvas/btn_option_1");
        btn_option_2 = GameObject.Find("FindDiffHouseCanvas/btn_option_2");
        btn_option_1_background = GameObject.Find("FindDiffHouseCanvas/btn_option_1_back");
        btn_option_2_background = GameObject.Find("FindDiffHouseCanvas/btn_option_2_back");

        img_roof = GameObject.Find("FindDiffHouseCanvas/img_clean_house/img_roof");
        img_window = GameObject.Find("FindDiffHouseCanvas/img_clean_house/img_window");

        // 오브젝트를 찾았는지 확인
        if (btn_option_1 == null || btn_option_2 == null || btn_option_1_background == null || btn_option_2_background == null || img_roof == null || img_window == null)
        {
            Debug.LogError("Can't find object");
            return;
        }

        // answer에 따라 roof, window를 각각 비활성화
        switch (answer)
        {
            case 0:
                img_window.SetActive(false);
                break;
            case 1:
                img_roof.SetActive(false);
                break;
        }

        // 각 버튼에 대한 이벤트 추가 배경에도 이벤트 추가
        btn_option_1.GetComponent<Button>().onClick.AddListener(() => CheckAnswer(0));
        btn_option_1_background.GetComponent<Button>().onClick.AddListener(() => CheckAnswer(0));

        btn_option_2.GetComponent<Button>().onClick.AddListener(() => CheckAnswer(1));
        btn_option_2_background.GetComponent<Button>().onClick.AddListener(() => CheckAnswer(1));
    }
    // 정답, 오답 판정
    public void CheckAnswer(int selected)
    {
        // 디버깅 메시지
        // Debug.Log("selected: " + selected + ", answer: " + answer);
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

            ProgressScoreManager.Instance.CalculateProgressScore("sr", 2, startTime, endTime, tryCount);

            // 게임 종료 코드 추가
            //SceneManager.LoadScene(nextSceneName);


        }
        // 오답 판정
        else
        {
            msg_congrate.SetActive(false);

            // 오답 판정 시, msg_retry 오브젝트를 활성화하고, 1초 후 비활성화
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
