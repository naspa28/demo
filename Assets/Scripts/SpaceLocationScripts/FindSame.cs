// 정답과 동일한 이미지를 찾아서 선택하는 "나를 찾아봐"를 위한 스크립트
// 랜덤하게 object의 위치를 변경한다.
// 정답과 정답인 선택지는 동일한 object의 위치를 가진다.
// 이미지 확정되면, offset
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindSame : MonoBehaviour
{
    // 메시지 오브젝트 설정
    private GameObject msg_congrate;
    private GameObject msg_retry;

    // 정답 들어갈 변수
    private int answer;

    void Start()
    {
        // 메시지 오브젝트 설정
        msg_congrate = GameObject.Find("FindSameCanvas/msg_congrate");
        msg_retry = GameObject.Find("FindSameCanvas/msg_retry");

        // msg_congrate, msg_retry 오브젝트를 비활성화
        msg_congrate.SetActive(false);
        msg_retry.SetActive(false);


        // 정답이 될 선택지 1개를 랜덤하게 선택
        answer = UnityEngine.Random.Range(1, 4);

        // scene에 있는 모든 btn_option_*/img_object를 불러옴
        for (int i = 1; i <= 3; i++)
        {
            MoveObjectToRandomPosition(i, answer);
        }


    }


    // Update is called once per frame
    void Update()
    {

    }

    void MoveObjectToRandomPosition(int index, int answer)
    {
        GameObject img_object = GameObject.Find("FindSameCanvas/btn_option_" + index + "/img_object");

        // 오브젝트를 찾았는지 확인
        if (img_object == null)
        {
            Debug.LogError("Can't find object for index: " + index);
            return;
        }

        // 랜덤한 위치로 해당 object를 이동 

        // 각 offset을 난수 발생으로 변수에 저장한다
        float offsetX = UnityEngine.Random.Range(-1.0f, 1.0f);
        float offsetY = UnityEngine.Random.Range(-1.0f, 1.0f);

        // 절대 좌표가 아니라, 상대 좌표로 이동시키기 위해, 현재 위치에서 랜덤한 위치를 더함
        img_object.transform.position = new Vector3(img_object.transform.position.x + offsetX, img_object.transform.position.y + offsetY, img_object.transform.position.z);


        // 정답이 될 선택지라면, 동일한 위치로 img_answer/obj_answer도 이동시킴

        if (index == answer)
        {
            GameObject img_answer = GameObject.Find("FindSameCanvas/img_answer/img_object");
            if (img_answer != null)
            {
                // 동일한 offset을 적용
                img_answer.transform.position = new Vector3(img_answer.transform.position.x + offsetX, img_answer.transform.position.y + offsetY, img_answer.transform.position.z);
            }
            else
            {
                Debug.LogError("Can't find img_answer object");
            }
        }

        // 버튼 클릭 시, CheckAnswer 함수를 호출하도록 btn_option_*에 리스너 추가
        GameObject btn_option_n = GameObject.Find("FindSameCanvas/btn_option_" + index);
        btn_option_n.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => CheckAnswer(index));
    }
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
