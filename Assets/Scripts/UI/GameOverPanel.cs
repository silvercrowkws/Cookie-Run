using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    /// <summary>
    /// 버튼
    /// </summary>
    Button restartButton;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    public GameManager gameManager;

    /// <summary>
    /// 점수 텍스트
    /// </summary>
    TextMeshProUGUI scoreText;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        Transform child = transform.GetChild(0);                // 0번 RestertButton
        restartButton = child.GetComponent<Button>();
        restartButton.onClick.AddListener(OnReStart);

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
            scoreText.text = score.ToString("N0");
        }
        else
        {
            //Debug.LogWarning("GameOverPanel이 파괴되었습니다.");            
        }
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

        SceneManager.LoadScene(0);
    }
}
