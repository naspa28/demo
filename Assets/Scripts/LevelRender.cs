// 레벨의 진척도에 따라 렌더링할 오브젝트들을 설정하는 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRender : MonoBehaviour
{
    // 스테이지의 이름을 가져오는 변수
    public string stageName;

    // 스테이지의 진척도를 저장 할 배열
    // 각 인덱스는 스테이지의 진척도를 나타내며, 0 ~ 3의 값을 가질 수 있다.
    // 0: 미해금, 1: 1개 해결, 2: 2개 해결, 3: 3개 해결
    public int[] progress;

    void Start()
    {
        // progress에 맞게 오브젝트를 렌더링
        // btn_to_stage_1, btn_to_stage_2, btn_to_stage_3 에 각각 있는 
        // ui_lock,ui_star_1, ui_star_2, ui_star_3을 렌더링
        for (int i = 0; i < progress.Length; i++)
        {
            Debug.Log("progress[" + i + "] = " + progress[i]);
            // 렌더링할 오브젝트를 가져옴
            GameObject ui_lock = GameObject.Find("LevelSelectCanvas/btn_to_stage_" + (i + 1) + "/ui_lock");
            GameObject ui_star_1 = GameObject.Find("LevelSelectCanvas/btn_to_stage_" + (i + 1) + "/ui_star_1");
            GameObject ui_star_2 = GameObject.Find("LevelSelectCanvas/btn_to_stage_" + (i + 1) + "/ui_star_2");
            GameObject ui_star_3 = GameObject.Find("LevelSelectCanvas/btn_to_stage_" + (i + 1) + "/ui_star_3");

            // 오브젝트를 찾았는지 확인
            if (ui_lock == null || ui_star_1 == null || ui_star_2 == null || ui_star_3 == null)
            {
                Debug.LogError("Can't find object");
                return;
            }

            // progress에 맞게 렌더링
            switch (progress[i])
            {
                case 0:
                    ui_lock.SetActive(true);
                    ui_star_1.SetActive(false);
                    ui_star_2.SetActive(false);
                    ui_star_3.SetActive(false);
                    break;
                case 1:
                    ui_lock.SetActive(false);
                    ui_star_1.SetActive(true);
                    ui_star_2.SetActive(false);
                    ui_star_3.SetActive(false);
                    break;
                case 2:
                    ui_lock.SetActive(false);
                    ui_star_1.SetActive(true);
                    ui_star_2.SetActive(true);
                    ui_star_3.SetActive(false);
                    break;
                case 3:
                    ui_lock.SetActive(false);
                    ui_star_1.SetActive(true);
                    ui_star_2.SetActive(true);
                    ui_star_3.SetActive(true);
                    break;
            }
        }

    }

    void Update()
    {

    }
}
