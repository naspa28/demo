// 도형 소지 훈련의 Lv2 "하트를 찾아줘"를 위한 스크립트
// 범위에 btn_heart, btn_dummy_1, btn_dummy_2, btn_dummy_3 위치 할 좌표를 난수로 생성한다( 배열로 저장한다 ).
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public class FindHeart : MonoBehaviour
{
    // 초기 시간, 종료 시간을 저장 할 변수
    private int startTime;
    private float endTime;

    // 시도 횟수를 저장 할 변수
    private int tryCount = 0;

    public GameObject btn_animal;
    public GameObject btn_heart;
    public GameObject btn_dummy_1;
    public GameObject btn_dummy_2;
    public GameObject btn_dummy_3;


    public GameObject msg_congrate;
    public GameObject msg_retry;
    
    public AudioSource backgroundMusicSource;

    public AudioClip failSound; // 실패 시 재생할 음성

    public AudioClip successSound; // 성공 시 재생할 음성

    public string nextSceneName; // 전환할 씬 이름

    private AudioSource audioSource;



    public float offset = 10.0f;

    private Vector3[] starPos = new Vector3[4];

    public float xRandomRange = 4.4f;
    public float yRandomRange = 2f;

    void Start()
    {
        // 시작 시간 설정
        startTime = int.Parse(DateTime.Now.ToString("HHmmss"));

        // 각 오브젝트를 씬에서 찾아서 할당
        // 오브젝트는 FindHeartCanvas의 자식으로 들어가 있어용
        btn_animal = GameObject.Find("FindHeartCanvas/btn_animal");
        btn_heart = GameObject.Find("FindHeartCanvas/btn_heart");
        btn_dummy_1 = GameObject.Find("FindHeartCanvas/btn_dummy_1");
        btn_dummy_2 = GameObject.Find("FindHeartCanvas/btn_dummy_2");
        btn_dummy_3 = GameObject.Find("FindHeartCanvas/btn_dummy_3");

        msg_congrate = GameObject.Find("FindHeartCanvas/msg_congrate");
        msg_retry = GameObject.Find("FindHeartCanvas/msg_retry");

        // msg_congrate, msg_retry는 비활성화 상태로 시작
        msg_congrate.SetActive(false);
        msg_retry.SetActive(false);

        RectTransform rect = btn_animal.GetComponent<RectTransform>();

        // 중심에서 범위 내에 랜덤 좌표를 생성하고, 배열에 저장한다.
        // 각 좌표의 범위를 변수에 저장 해놓는다.
        // xRandomRange = 4.4f;
        // yRandomRange = 2f;

        starPos[0] = new Vector3(UnityEngine.Random.Range(xRandomRange, -xRandomRange), UnityEngine.Random.Range(yRandomRange, -yRandomRange), 0);
        starPos[1] = new Vector3(UnityEngine.Random.Range(xRandomRange, -xRandomRange), UnityEngine.Random.Range(yRandomRange, -yRandomRange), 0);
        starPos[2] = new Vector3(UnityEngine.Random.Range(xRandomRange, -xRandomRange), UnityEngine.Random.Range(yRandomRange, -yRandomRange), 0);
        starPos[3] = new Vector3(UnityEngine.Random.Range(xRandomRange, -xRandomRange), UnityEngine.Random.Range(yRandomRange, -yRandomRange), 0);

        RectTransform heartRectTransform = btn_heart.GetComponent<RectTransform>();
        RectTransform dummy1RectTransform = btn_dummy_1.GetComponent<RectTransform>();
        RectTransform dummy2RectTransform = btn_dummy_2.GetComponent<RectTransform>();
        RectTransform dummy3RectTransform = btn_dummy_3.GetComponent<RectTransform>();

        heartRectTransform.position = btn_animal.transform.position + starPos[0];
        dummy1RectTransform.position = btn_animal.transform.position + starPos[1];
        dummy2RectTransform.position = btn_animal.transform.position + starPos[2];
        dummy3RectTransform.position = btn_animal.transform.position + starPos[3];

        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();

        GameObject bgmObject = GameObject.Find("AudioManager");
        backgroundMusicSource = bgmObject.GetComponent<AudioSource>();



    }

    // touch 된게 star면, congrate 메세지를 1초 동안 띄우고, 해당 별을 비활성화 시킨다.
    // touch 된게 animal이면, retry 메세지를 1초 동안  띄운다.
    public void btnClicked()
    {
        tryCount++;

        GameObject clkedObj = EventSystem.current.currentSelectedGameObject;
        Debug.Log("Clicked Object Name: " + clkedObj.name);

        if (clkedObj.name == "btn_heart")
        {
            msg_retry.SetActive(false);
            msg_congrate.SetActive(true);
            StartCoroutine(ShowCongrate());

            endTime = int.Parse(DateTime.Now.ToString("HHmmss"));

            // DB에 저장하는 함수 호출
            // concentrationScore 아직 미구현
            ProgressScoreManager.Instance.CalculateProgressScore("fg", 1, startTime, endTime, tryCount);

            // 게임 종료 코드 추가
            StartCoroutine(WaitForSoundAndLoadScene());

        }
        else if (clkedObj.name == "btn_dummy_1" || clkedObj.name == "btn_dummy_2" || clkedObj.name == "btn_dummy_3" || clkedObj.name == "btn_animal")
        {
            msg_congrate.SetActive(false);
            msg_retry.SetActive(true);
            StartCoroutine(ShowRetry());
        }
    }

    IEnumerator ShowCongrate()
    {
        backgroundMusicSource.Stop();
        audioSource.clip = successSound;
        audioSource.Play();
        msg_congrate.SetActive(true);

        yield return new WaitForSeconds(successSound.length);

    }
    IEnumerator ShowRetry()
    {
        audioSource.clip = failSound;
        audioSource.Play();
         // 실패 음성 길이만큼 대기
        yield return new WaitForSeconds(failSound.length);

        msg_retry.SetActive(false);
    }
    private IEnumerator WaitForSoundAndLoadScene()
    {
        // 음성이 끝날 때까지 대기
        while (audioSource.isPlaying)
        {
            yield return null; // 다음 프레임까지 대기
        }

        // 씬 전환
        SceneManager.LoadScene(nextSceneName);
    }



    void Update()
    {
        // 터치가 들어오고, 클릭 된 오브젝트가 있으면, btnClicked 함수를 호출한다
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                btnClicked();
            }
        }
    }
}
