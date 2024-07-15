// 누가 누가 다를까? 에서 사용되는 스크립트
// 동물 이미지 배치, 구분기준 ( 깃발...  )등 배치하는 기능 가짐.
// 정답, 오답 판정 기능을 가지고 있음
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FindDiffSquirrel : MonoBehaviour
{
    // 메시지 오브젝트 설정 
    private GameObject msg_congrate;
    private GameObject msg_retry;

    // 문제 배치
    // 난수 생성을 통해 0 ~ 2 숫자 중 정답을 설정
    private int answer;

    void Start()
    {
        msg_congrate = GameObject.Find("FindDiffSquirrelCanvas/msg_congrate");
        msg_retry = GameObject.Find("FindDiffSquirrelCanvas/msg_retry");

        // msg_congrate, msg_retry 오브젝트를 비활성화
        msg_congrate.SetActive(false);
        msg_retry.SetActive(false);

        // 난수 생성
        answer = UnityEngine.Random.Range(0, 3);


        // answer에 따라 FindDiffCanvas의 btn_option_1, btn_option_2, btn_option_3
        // 의 Rotation y 값을 180도로 설정하고, x 좌표의 부호를 반전시키다.
        GameObject btn_option_1 = GameObject.Find("FindDiffSquirrelCanvas/btn_option_1");
        GameObject btn_option_2 = GameObject.Find("FindDiffSquirrelCanvas/btn_option_2");
        GameObject btn_option_3 = GameObject.Find("FindDiffSquirrelCanvas/btn_option_3");

        // 오브젝트를 찾았는지 확인
        if (btn_option_1 == null || btn_option_2 == null || btn_option_3 == null)
        {
            Debug.LogError("Can't find object");
            return;
        }

        // answer에 따라 각 버튼의 img의 transform을 변경
        switch (answer)
        {
            case 0:
                btn_option_1.transform.localPosition = new Vector3(btn_option_1.transform.localPosition.x - 350, btn_option_1.transform.localPosition.y, btn_option_1.transform.localPosition.z);
                btn_option_2.transform.localPosition = new Vector3(btn_option_2.transform.localPosition.x, btn_option_2.transform.localPosition.y + 350, btn_option_2.transform.localPosition.z);
                btn_option_3.transform.localPosition = new Vector3(btn_option_3.transform.localPosition.x, btn_option_3.transform.localPosition.y - 350, btn_option_3.transform.localPosition.z);
                break;
            case 1:
                btn_option_1.transform.localPosition = new Vector3(btn_option_1.transform.localPosition.x, btn_option_1.transform.localPosition.y + 350, btn_option_1.transform.localPosition.z);
                btn_option_2.transform.localPosition = new Vector3(btn_option_2.transform.localPosition.x - 350, btn_option_2.transform.localPosition.y, btn_option_2.transform.localPosition.z);
                btn_option_3.transform.localPosition = new Vector3(btn_option_3.transform.localPosition.x, btn_option_3.transform.localPosition.y - 350, btn_option_3.transform.localPosition.z);
                break;
            case 2:
                btn_option_1.transform.localPosition = new Vector3(btn_option_1.transform.localPosition.x, btn_option_1.transform.localPosition.y - 350, btn_option_1.transform.localPosition.z);
                btn_option_2.transform.localPosition = new Vector3(btn_option_2.transform.localPosition.x, btn_option_2.transform.localPosition.y + 350, btn_option_2.transform.localPosition.z);
                btn_option_3.transform.localPosition = new Vector3(btn_option_3.transform.localPosition.x - 350, btn_option_3.transform.localPosition.y, btn_option_3.transform.localPosition.z);
                break;
        }

        // 각 버튼에 대한 이벤트 추가
        btn_option_1.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => CheckAnswer(0));
        btn_option_2.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => CheckAnswer(1));
        btn_option_3.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => CheckAnswer(2));
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
