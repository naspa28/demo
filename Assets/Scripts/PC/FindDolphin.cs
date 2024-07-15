using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindDolphin : MonoBehaviour
{
    // 이미지 오브젝트 설정
    private GameObject msg_congrate;
    private GameObject msg_retry;

    // 난수 들어갈 변수
    private int random;
    private int answer = 2;

    // 씬의 오브젝트들을 저장 할 변수
    private GameObject btn_option_1;
    private GameObject btn_option_2;
    private GameObject btn_option_3;
    private GameObject btn_option_4;

    // btn_option_*의 위치를 위해 더할 값을 저장 할 배열
    private Vector3[] offset = new Vector3[4] { new Vector3(6, -1, 0), new Vector3(2, -3, 0), new Vector3(1, 0.4f, 0), new Vector3(-2, -2, 0) };

    void Start()
    {
        // 메시지 오브젝트 설정
        msg_congrate = GameObject.Find("FindDolphinCanvas/msg_congrate");
        msg_retry = GameObject.Find("FindDolphinCanvas/msg_retry");

        // msg_congrate, msg_retry 오브젝트를 비활성화
        msg_congrate.SetActive(false);
        msg_retry.SetActive(false);

        // 난수 생성
        random = UnityEngine.Random.Range(0, 4);

        // 디버깅 용으로 현재 btn_option_1, btn_option_2, btn_option_3의 위치를 출력
        btn_option_1 = GameObject.Find("FindDolphinCanvas/btn_option_1");
        btn_option_2 = GameObject.Find("FindDolphinCanvas/btn_option_2");
        btn_option_3 = GameObject.Find("FindDolphinCanvas/btn_option_3");
        btn_option_4 = GameObject.Find("FindDolphinCanvas/btn_option_4");

        // 오브젝트를 찾았는지 확인
        if (btn_option_1 == null || btn_option_2 == null || btn_option_3 == null || btn_option_4 == null)
        {
            Debug.LogError("Can't find object");
            return;
        }

        // answer에 따라 각 버튼의 위치를 변경
        switch (random)
        {
            case 0:
                btn_option_1.transform.position += offset[0];
                btn_option_2.transform.position += offset[1];
                btn_option_3.transform.position += offset[2];
                btn_option_4.transform.position += offset[3];
                break;
            case 1:
                btn_option_1.transform.position += offset[1];
                btn_option_2.transform.position += offset[0];
                btn_option_3.transform.position += offset[2];
                btn_option_4.transform.position += offset[3];
                break;
            case 2:
                btn_option_1.transform.position += offset[2];
                btn_option_2.transform.position += offset[1];
                btn_option_3.transform.position += offset[0];
                btn_option_4.transform.position += offset[3];
                break;
            case 3:
                btn_option_1.transform.position += offset[3];
                btn_option_2.transform.position += offset[1];
                btn_option_3.transform.position += offset[2];
                btn_option_4.transform.position += offset[0];
                break;
        }

        // 각 버튼에 대한 이벤트 추가
        btn_option_1.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => CheckAnswer(0));
        btn_option_2.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => CheckAnswer(1));
        btn_option_3.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => CheckAnswer(2));
        btn_option_4.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => CheckAnswer(3));

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


}
