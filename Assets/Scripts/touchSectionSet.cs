// 버튼의 투명한 부분은 터치되지 않도록 하는 코드
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class touchSectionSet : MonoBehaviour
{
    // alpha 값을 저장 할 실수 변수
    public float alphaValue = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Image img = GetComponent<Image>();
        img.alphaHitTestMinimumThreshold = alphaValue;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
