// Scene의 Image 소스를 일괄 적용하는 스크립트
// 이미지들 중 랜덤으로 하나를 선택하여 적용하는 로직이 필요함
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SetImg : MonoBehaviour
{
    public Sprite animalImg;
    public Sprite objectImg;

    // Canvas이름을 담을 문자열 변수
    public string canvasName = "Canvas";

    // Scene의 btn_option_*/img_object, btn_option_*/img_animal에 동일한 이미지 적용

    void Start()
    {
        // Scene에 있는 모든 btn_option_*/img_object, btn_option_*/img_animal 불러 옴
        // btn_option_1 ~ 3, img_object, img_animal
        for (int i = 1; i <= 3; i++)
        {
            GameObject img_object = GameObject.Find(canvasName + "/btn_option_" + i + "/img_object");
            GameObject img_animal = GameObject.Find(canvasName + "/btn_option_" + i + "/img_animal");

            // 오브젝트를 찾았는지 확인
            if (img_object == null || img_animal == null)
            {
                Debug.LogError("Can't find object");
                return;
            }

            // 이미지 적용
            img_object.GetComponent<Image>().sprite = objectImg;
            img_animal.GetComponent<Image>().sprite = animalImg;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
