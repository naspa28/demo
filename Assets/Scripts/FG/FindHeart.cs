// 도형 소지 훈련의 Lv2 "하트를 찾아줘"를 위한 스크립트
// 범위에 btn_heart, btn_dummy_1, btn_dummy_2, btn_dummy_3 위치 할 좌표를 난수로 생성한다( 배열로 저장한다 ).
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FindHeart : MonoBehaviour
{
    public GameObject btn_animal;
    public GameObject btn_heart;
    public GameObject btn_dummy_1;
    public GameObject btn_dummy_2;
    public GameObject btn_dummy_3;


    public GameObject msg_congrate;
    public GameObject msg_retry;

    public float offset = 10.0f;

    private Vector3[] starPos = new Vector3[4];

    public float xRandomRange = 4.4f;
    public float yRandomRange = 2f;

    void Start()
    {
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

        starPos[0] = new Vector3(Random.Range(xRandomRange, -xRandomRange), Random.Range(yRandomRange, -yRandomRange), 0);
        starPos[1] = new Vector3(Random.Range(xRandomRange, -xRandomRange), Random.Range(yRandomRange, -yRandomRange), 0);
        starPos[2] = new Vector3(Random.Range(xRandomRange, -xRandomRange), Random.Range(yRandomRange, -yRandomRange), 0);
        starPos[3] = new Vector3(Random.Range(xRandomRange, -xRandomRange), Random.Range(yRandomRange, -yRandomRange), 0);

        RectTransform heartRectTransform = btn_heart.GetComponent<RectTransform>();
        RectTransform dummy1RectTransform = btn_dummy_1.GetComponent<RectTransform>();
        RectTransform dummy2RectTransform = btn_dummy_2.GetComponent<RectTransform>();
        RectTransform dummy3RectTransform = btn_dummy_3.GetComponent<RectTransform>();

        heartRectTransform.position = btn_animal.transform.position + starPos[0];
        dummy1RectTransform.position = btn_animal.transform.position + starPos[1];
        dummy2RectTransform.position = btn_animal.transform.position + starPos[2];
        dummy3RectTransform.position = btn_animal.transform.position + starPos[3];

    }

    // touch 된게 star면, congrate 메세지를 1초 동안 띄우고, 해당 별을 비활성화 시킨다.
    // touch 된게 animal이면, retry 메세지를 1초 동안  띄운다.
    public void btnClicked()
    {
        GameObject clkedObj = EventSystem.current.currentSelectedGameObject;
        Debug.Log("Clicked Object Name: " + clkedObj.name);

        if (clkedObj.name == "btn_heart")
        {
            msg_congrate.SetActive(true);
            StartCoroutine(ShowCongrate());
            // clkedObj.SetActive(false);
        }
        else if (clkedObj.name == "btn_dummy_1" || clkedObj.name == "btn_dummy_2" || clkedObj.name == "btn_dummy_3" || clkedObj.name == "btn_animal")
        {
            msg_retry.SetActive(true);
            StartCoroutine(ShowRetry());
        }
    }

    IEnumerator ShowCongrate()
    {
        yield return new WaitForSeconds(1.0f);
        msg_congrate.SetActive(false);
    }
    IEnumerator ShowRetry()
    {
        yield return new WaitForSeconds(1.0f);
        msg_retry.SetActive(false);
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
