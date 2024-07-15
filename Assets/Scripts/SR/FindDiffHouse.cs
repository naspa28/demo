// 다람쥐 집 빠진 부분을 찾아줘! 에서 사용되는 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FindDiffHouse : MonoBehaviour
{
    // 메시지 오브젝트 설정 
    private GameObject msg_congrate;
    private GameObject msg_retry;

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
        // 메시지 오브젝트 설정
        msg_congrate = GameObject.Find("FindDiffHouseCanvas/msg_congrate");
        msg_retry = GameObject.Find("FindDiffHouseCanvas/msg_retry");


        // msg_congrate, msg_retry 오브젝트를 비활성화
        msg_congrate.SetActive(false);
        msg_retry.SetActive(false);

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
        Debug.Log("selected: " + selected + ", answer: " + answer);

        // 정답 판정
        if (selected == answer)
        {
            // 정답 판정 시, msg_congrate 오브젝트를 활성화
            // GameObject msg_congrate = GameObject.Find("UpsideDownCanvas/msg_congrate");
            msg_congrate.SetActive(true);

            // msg_congrate 오브젝트를 1초 후 비활성화
            StartCoroutine(DisableMsgCongrate());

            IEnumerator DisableMsgCongrate()
            {
                yield return new WaitForSeconds(1.0f);
                msg_congrate.SetActive(false);
            }

        }
        // 오답 판정
        else
        {
            // 오답 판정 시, msg_retry 오브젝트를 활성화하고, 1초 후 비활성화
            // GameObject msg_retry = GameObject.Find("UpsideDownCanvas/msg_retry");
            msg_retry.SetActive(true);

            StartCoroutine(DisableMsgRetry());

            IEnumerator DisableMsgRetry()
            {
                yield return new WaitForSeconds(1.0f);
                msg_retry.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
