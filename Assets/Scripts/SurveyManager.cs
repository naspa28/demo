// 사용자 최초 가입 시 설문조사 화면을 진행하는 스트립트
// 설문조사를 진행하면서 사용자가 입력한 정보를 변수에 저장한다.
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

// TODO
// 설문 결과 변수
// FinishButton 리스너 추가
public class SurveyMamager : MonoBehaviour
{
    // Question_1 ~ Question_8 오브젝트를 배열로 저장
    private GameObject[] Questions;

    // alert 창을 저장할 변수
    private GameObject Alert;

    // 다음, 이전, 완료 버튼을 저장할 변수
    private Button NextButton;
    private Button PrevButton;
    private Button FinishButton;

    // 토글 버튼을 저장할 변수
    // Q2의 Yes, No 토글, Q3의 Yes, No 버튼과 Mild, Moderate, Severe, Profound 토글
    private ToggleGroup Q2_yes_no_Toggle;
    private ToggleGroup Q3_yes_no_Toggle;
    private ToggleGroup Q3_level_Toggle;

    // 문제의 index를 저장할 변수
    private int QuestionIndex;

    // 설문 결과 저장 배열 Yes : 1, No : 0 
    // index 0 : Q1 성별, 1 : Q2 보유 여부, 2 : Q3 보유 여부
    private int[] SurveyResult;

    // IQ 입력값 저장 변수
    private int input_IQ;
    // 지적 레벨 저장 변수 1 : Mild, 2 : Moderate, 3 : Severe, 4: Profound
    private int level_toggle;
    // 출생연도 저장 변수
    private int birth_year;

    void Start()
    {
        // 설문 결과 저장 배열 초기화
        SurveyResult = new int[8];

        // alert 창을 찾아서 변수에 저장
        Alert = GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/alert_popup");

        // QuestionIndex 변수 초기화
        QuestionIndex = 0;

        // 다음, 이전 버튼을 찾아서 변수에 저장
        NextButton = GameObject.Find("Main Camera/Canvas/SurveyPanel/btn_next").GetComponent<Button>();
        PrevButton = GameObject.Find("Main Camera/Canvas/SurveyPanel/btn_prev").GetComponent<Button>();
        FinishButton = GameObject.Find("Main Camera/Canvas/SurveyPanel/btn_finish").GetComponent<Button>();

        // 토글 버튼을 찾아서 변수에 저장
        Q2_yes_no_Toggle = GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_2/yes_no_toggle").GetComponent<ToggleGroup>();
        Q3_yes_no_Toggle = GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_3/yes_no_toggle").GetComponent<ToggleGroup>();
        Q3_level_Toggle = GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_3/level_toggle").GetComponent<ToggleGroup>();

        // 기본값으로 input_IQ, level_toggle 비활성화
        GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_2/input_IQ").SetActive(false);
        GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_3/level_toggle").SetActive(false);

        // alert 창 비활성화
        Alert.SetActive(false);

        foreach (Toggle toggle in Q2_yes_no_Toggle.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(isOn =>
            {
                // "Yes" 버튼이 선택되면 IQ 입력창을 활성화합니다.
                if (toggle.name == "yes")
                {
                    GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_2/input_IQ").SetActive(true);
                }
                // "No" 버튼이 선택되면 IQ 입력창을 비활성화합니다.
                else
                {
                    GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_2/input_IQ").SetActive(false);
                }
            });
        }

        foreach (Toggle toggle in Q3_yes_no_Toggle.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(isOn =>
            {
                // "Yes" 버튼이 선택되면 level_toggle 토글을 활성화합니다.
                if (toggle.name == "yes")
                {
                    GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_3/level_toggle").SetActive(true);
                }
                // "No" 버튼이 선택되면 level_toggle 토글을 비활성화합니다.
                else
                {
                    GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_3/level_toggle").SetActive(false);
                }
            });
        }


        // 배열에 Question_1 ~ Question_8 배열 Questions에 오브젝트를 저장
        // 반복문을 이용한다
        Questions = new GameObject[8];
        for (int i = 0; i < 8; i++)
        {
            Questions[i] = GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_" + (i + 1));
            // Question_2 ~ Question_8 오브젝트 비활성화
            Questions[i].SetActive(false);
        }

        // 설문조사 진행 시 첫번째 질문 활성화, 이전 버튼 비활성화 후 투명하게 만들기
        Questions[0].SetActive(true);
        PrevButton.interactable = false;
        PrevButton.GetComponent<Image>().color = new Color(255, 255, 255, 0);

        // 완료 버튼 비활성화 후 투명하게 만들기
        FinishButton.interactable = false;
        FinishButton.GetComponent<Image>().color = new Color(255, 255, 255, 0);

        // 각 버튼에 리스너 추가
        NextButton.onClick.AddListener(NextQuestion);
        PrevButton.onClick.AddListener(PrevQuestion);
        FinishButton.onClick.AddListener(FinishSurvey);
    }

    // 다음 버튼을 누르면 실행되는 함수 선언
    public void NextQuestion()
    {
        // index 체크 후 Q1, Q2, Q3 만 체크해서 출생년도, IQ, 지적 레벨을 저장
        if (QuestionIndex == 0)
        {
            // 출생년도 입력값 null이거나 정수가 아니면 popup 창을 띄워줌
            if (GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_1/input_year").GetComponent<TMP_InputField>().text == "")
            {
                Alert.SetActive(true);
                StartCoroutine(AlertPopup());
                return;
            }
            else
            {
                birth_year = int.Parse(GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_1/input_year").GetComponent<TMP_InputField>().text);
            }
            // Q1 성별 체크
            if (GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_1/gender_toggle").GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault().name == "male")
            {
                SurveyResult[0] = 1;
            }
            else
            {
                SurveyResult[0] = 0;
            }

        }
        else if (QuestionIndex == 1)
        {
            // Q2 보유 여부 체크
            if (Q2_yes_no_Toggle.ActiveToggles().FirstOrDefault().name == "yes")
            {
                SurveyResult[1] = 1;
                // IQ 입력값 null이거나 정수가 아니면 popup 창을 띄워줌
                if (GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_2/input_IQ").GetComponent<TMP_InputField>().text == "" || !int.TryParse(GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_2/input_IQ").GetComponent<TMP_InputField>().text, out input_IQ))
                {
                    Alert.SetActive(true);
                    StartCoroutine(AlertPopup());
                    return;
                }
                else
                {
                    input_IQ = int.Parse(GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_2/input_IQ").GetComponent<TMP_InputField>().text);
                }
            }
            else
            {
                SurveyResult[1] = 0;
            }
        }
        else if (QuestionIndex == 2)
        {
            // Q3 보유 여부 체크
            if (Q3_yes_no_Toggle.ActiveToggles().FirstOrDefault().name == "yes")
            {
                SurveyResult[2] = 1;
                // 지적 레벨 체크
                if (Q3_level_Toggle.ActiveToggles().FirstOrDefault().name == "mild")
                {
                    level_toggle = 1;
                }
                else if (Q3_level_Toggle.ActiveToggles().FirstOrDefault().name == "moderate")
                {
                    level_toggle = 2;
                }
                else if (Q3_level_Toggle.ActiveToggles().FirstOrDefault().name == "severe")
                {
                    level_toggle = 3;
                }
                else
                {
                    level_toggle = 4;
                }
            }
            else
            {
                SurveyResult[2] = 0;
            }
            // 이전 버튼 활성화 후 투명도를 1로 만들기
            PrevButton.interactable = true;
            PrevButton.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        }
        // Q2 부턴 yes_no_toggle을 SurveyResult에 저장
        else if (QuestionIndex >= 3)
        {
            if (GameObject.Find("Main Camera/Canvas/SurveyPanel/bg_popup/Question_" + (QuestionIndex + 1) + "/yes_no_toggle").GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault().name == "yes")
            {
                SurveyResult[QuestionIndex] = 1;
            }
            else
            {
                SurveyResult[QuestionIndex] = 0;
            }
        }
        // 현재 활성화된 질문을 비활성화
        Questions[QuestionIndex].SetActive(false);
        // 다음 질문을 활성화
        Questions[++QuestionIndex].SetActive(true);

        // 마지막 문제로 넘어가는 상황인 경우, 완료 버튼 활성화 후 투명도를 1로 만들기
        // 다음 버튼 비활성화 후 투명하게 만들기
        if (QuestionIndex == 7)
        {
            FinishButton.interactable = true;
            FinishButton.GetComponent<Image>().color = new Color(255, 255, 255, 1);

            NextButton.interactable = false;
            NextButton.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }

    }

    // 이전 버튼을 누르면 실행되는 함수 선언
    public void PrevQuestion()
    {
        // 현재 활성화된 질문을 비활성화
        Questions[QuestionIndex].SetActive(false);
        // 이전 질문을 활성화
        Questions[--QuestionIndex].SetActive(true);

        // 다음 버튼 활성화 후 투명도를 1로 만들기
        NextButton.interactable = true;
        NextButton.GetComponent<Image>().color = new Color(255, 255, 255, 1);

        // 첫번째 질문이면 이전 버튼 비활성화 후 투명하게 만들기
        if (QuestionIndex == 0)
        {
            PrevButton.interactable = false;
            PrevButton.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }

        // 마지막 질문이 아니면 완료 버튼 비활성화 후 투명하게 만들기
        if (QuestionIndex != 7)
        {
            FinishButton.interactable = false;
            FinishButton.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
    }

    // 빈 칸을 모두 입력해달라는 popup창을 1초간 띄우는 함수 선언
    IEnumerator AlertPopup()
    {
        yield return new WaitForSeconds(1.0f);
        Alert.SetActive(false);
    }

    // 완료 버튼을 누르면 실행되는 함수 선언
    public void FinishSurvey()
    {
        // 설문 결과를 저장하는 SurveyResult 배열을 출력
        for (int i = 0; i < 8; i++)
        {
            Debug.Log("index : " + i + " value : " + SurveyResult[i]);
        }
        // 변수들 출력
        Debug.Log("birth_year : " + birth_year);
        Debug.Log("input_IQ : " + input_IQ);
        Debug.Log("level_toggle : " + level_toggle);

    }

}
