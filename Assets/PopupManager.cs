using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel; // 팝업 Panel을 연결합니다.

    void Start()
    {
        // 시작할 때 팝업 창을 비활성화합니다.
        popupPanel.SetActive(false);
    }

    public void ShowPopup()
    {
        // 팝업 창을 활성화합니다.
        popupPanel.SetActive(true);
    }

    public void ClosePopup()
    {
        // 팝업 창을 비활성화합니다.
        popupPanel.SetActive(false);
    }
}
