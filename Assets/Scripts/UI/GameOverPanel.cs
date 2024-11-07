using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    /// <summary>
    /// 재시작 버튼
    /// </summary>
    Button restartButton;

    /// <summary>
    /// 나가기 버튼
    /// </summary>
    Button ouitButton;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// 점수 텍스트
    /// </summary>
    TextMeshProUGUI scoreText;

    /// <summary>
    /// 캔버스 그룹
    /// </summary>
    CanvasGroup canvasGroup;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        canvasGroup = GetComponent<CanvasGroup>();

        // 처음 알파값을 0으로 설정
        canvasGroup.alpha = 0;

        Transform child = transform.GetChild(0);                        // 0번 Buttons

        restartButton = child.GetChild(0).GetComponent<Button>();       // Buttons의 0번째 자식
        restartButton.onClick.AddListener(OnReStart);

        ouitButton = child.GetChild(1).GetComponent<Button>();          // Buttons의 1번째 자식
        ouitButton.onClick.AddListener(OnOuit);

        child = transform.GetChild(1);                          // 1번 Score
        child = child.GetChild(1);                              // Score의 1번 텍스트
        scoreText = child.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        this.gameObject.SetActive(false);

        gameManager.onGameOver += ShowPanel;
    }

    /// <summary>
    /// 게임 매니저의 게임 오버 델리게이트를 받아서 패널을 활성화 시키는 함수
    /// </summary>
    public void ShowPanel(int score)
    {
        if (this != null && this.gameObject != null) // 인스턴스와 게임 오브젝트가 null이 아닌지 확인
        {
            this.gameObject.SetActive(true);
            StartCoroutine(FadeInCoroutine());
            scoreText.text = score.ToString("N0");
        }
        else
        {
            //Debug.LogWarning("GameOverPanel이 파괴되었습니다.");            
        }
    }

    IEnumerator FadeInCoroutine()
    {
        float duration = 2.0f; // 페이드 지속 시간
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // 2초에 걸쳐서 알파값 1로 조정
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration); // 0에서 1로 알파값 증가
            yield return null;
        }

        // 최종 알파값을 1로 조정
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// 재시작 버튼으로 게임을 재시작하는 함수(씬을 불러옴)
    /// </summary>
    private void OnReStart()
    {
        Debug.Log("재시작 버튼 클릭");

        this.gameObject.SetActive(false);
        gameManager.Money = 0;          // 돈 초기화
        gameManager.Jelly = 0;          // 젤리 초기화
        gameManager.score = 0;          // 점수 초기화
        gameManager.timeElapsed = 0;    // 달린 시간 초기화
        gameManager.score = 0;          // 점수 초기화
        gameManager.currentGroundMoveSpeed = gameManager.baseGroundMoveSpeed;       // 바닥 속도 초기화
        canvasGroup.alpha = 0;          // 캔버스 그룹으로 패널과 자식의 알파값 초기화

        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// 나가기 버튼으로 게임을 종료하는 함수
    /// </summary>
    private void OnOuit()
    {
        Debug.Log("나가기 버튼 클릭");
        Application.Quit();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;        // 에디터 모드에서 실행 중일 때 종료
    #endif
    }
}
