using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class GazePointScript : MonoBehaviour
{
    public GameObject gazePoint;     // UI element
    public GameObject targetObject;  // World space object
    public GameObject msg_congrate;
    public string nextSceneName; // 전환할 씬 이름
    public string panelName;
    public int level;
    
    public AudioSource backgroundMusicSource;

    public AudioClip endSound; // 애니메이션 종료 시 재생할 음성
    private AudioSource audioSource;

    private RectTransform gazeRectTransform;
    private Collider2D targetCollider;
    private Animator targetAnimator;
    private bool isAnimationEnded;

    // 초기 시간, 종료 시간을 저장 할 변수
    private int startTime;
    private float endTime;

    void Start()
    {
        startTime = int.Parse(DateTime.Now.ToString("HHmmss"));

        msg_congrate = GameObject.Find("msg_congrate");

        msg_congrate.SetActive(false);

        // Get the RectTransform of the gazePoint UI element
        gazeRectTransform = gazePoint.GetComponent<RectTransform>();

        // Get the Collider2D and Animator components of the target object
        targetCollider = targetObject.GetComponent<Collider2D>();
        targetAnimator = targetObject.GetComponent<Animator>();
        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();

        GameObject bgmObject = GameObject.FindWithTag("AudioManager");
        backgroundMusicSource = bgmObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check if the gazePoint is active before proceeding
        if (!gazePoint.activeInHierarchy)
        {
            targetAnimator.enabled = false;
            return;
        }

        // Convert the gazePoint's position to world space
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(gazeRectTransform, gazeRectTransform.position, Camera.main, out worldPoint))
        {
            // Check if the world point is within the bounds of the target's collider
            if (targetCollider.OverlapPoint(worldPoint))
            {
                targetAnimator.enabled = true;
                Debug.Log("Gaze point is on target");
            }
            else
            {
                targetAnimator.enabled = false;
                Debug.Log("Gaze point is not on target");
            }
        }
        else
        {
            targetAnimator.enabled = false;
            Debug.Log("Failed to convert gaze point to world point");
        }

         // 애니메이션 상태를 검사
        if (targetAnimator.GetCurrentAnimatorStateInfo(0).IsName("penguinMove")||
        targetAnimator.GetCurrentAnimatorStateInfo(0).IsName("penguinMove2")||
        targetAnimator.GetCurrentAnimatorStateInfo(0).IsName("sealMove"))        {
            if (targetAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                if (!isAnimationEnded)
                {
                    isAnimationEnded = true;
                    OnAnimationEnd();
                }
            }
            else
            {
                isAnimationEnded = false;
            }
        }
    }

    // 애니메이션 종료 시 호출할 함수
    private void OnAnimationEnd()
    {
        endTime = int.Parse(DateTime.Now.ToString("HHmmss"));

        ProgressScoreManager.Instance.CalculateProgressScore("vm", level, startTime, endTime, 1);

        Debug.Log("Animation End");

        // 기존 배경음악 정지
        backgroundMusicSource.Stop();

        // 음성 재생
        audioSource.clip = endSound;
        audioSource.Play();
        msg_congrate.SetActive(true);

        // 음성이 끝날 때까지 대기하고 씬 전환
        StartCoroutine(WaitForSoundAndLoadScene());
    }

    private IEnumerator WaitForSoundAndLoadScene()
    {
        // 음성이 끝날 때까지 대기
        while (audioSource.isPlaying)
        {
            yield return null; // 다음 프레임까지 대기
        }

        // 씬 로드 완료 시 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 씬 전환
        SceneManager.LoadScene(nextSceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 로드 완료 시 이벤트 구독 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        // 코루틴을 통해 패널 활성화 시도
        StartCoroutine(ActivatePanel());
    }

    private IEnumerator ActivatePanel()
    {
        // 패널을 찾을 때까지 반복 시도
        GameObject panel = null;
        while (panel == null)
        {
            panel = GameObject.Find(panelName);
            if (panel == null)
            {
                Debug.LogWarning("Panel not found: " + panelName + ". Retrying...");
                yield return null; // 다음 프레임까지 대기
            }
        }

        // 패널 활성화
        panel.SetActive(true);
        Debug.Log("Panel activated: " + panelName);
    }
    
}