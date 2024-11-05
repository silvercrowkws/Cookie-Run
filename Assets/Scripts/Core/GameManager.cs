using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    public Player Player
    {
        get
        {
            if (player == null)
                player = FindAnyObjectByType<Player>();
            return player;
        }
    }

    /// <summary>
    /// 현재 가지고 있는 돈
    /// </summary>
    float currentMoney;

    /// <summary>
    /// 돈 프로퍼티
    /// </summary>
    public float Money
    {
        get => currentMoney;
        set
        {
            if (currentMoney != value)
            {
                //currentMoney = value;
                currentMoney = Mathf.Clamp(value, 0, 999);
                Debug.Log($"남은 돈 : {currentMoney}");
                onMoneyChange?.Invoke(currentMoney);
            }
        }
    }

    /// <summary>
    /// 돈이 변경되었음을 알리는 델리게이트(UI 수정용)
    /// </summary>
    public Action<float> onMoneyChange;

    /// <summary>
    /// 현재 가지고 있는 젤리
    /// </summary>
    float currentJelly;

    /// <summary>
    /// 젤리 프로퍼티
    /// </summary>
    public float Jelly
    {
        get => currentJelly;
        set
        {
            if (currentJelly != value)
            {
                //currentJelly = value;
                currentJelly = Mathf.Clamp(value, 0, 999);
                Debug.Log($"남은 젤리 : {currentJelly}");
                onJellyChange?.Invoke(currentJelly);
            }
        }
    }

    /// <summary>
    /// 젤리가 변경되었음을 알리는 델리게이트(UI 수정용)
    /// </summary>
    public Action<float> onJellyChange;

    /// <summary>
    /// 기본 속도
    /// </summary>
    public float baseGroundMoveSpeed = 3.0f;

    /// <summary>
    /// 현재 속도
    /// </summary>
    public float currentGroundMoveSpeed;

    /// <summary>
    /// 게임이 종료되었다고 알리는 델리게이트
    /// </summary>
    public Action<int> onGameOver;

    /// <summary>
    /// 게임 오버 시 최종 점수
    /// </summary>
    public int score = 0;

    private void Start()
    {
        currentMoney = 0;
        currentJelly = 0;
        score = 0;

        // 초기 속도 설정
        currentGroundMoveSpeed = baseGroundMoveSpeed;

        player.onPlayerDie += GameOver;

        // 씬이 로드될 때 호출될 이벤트에 메서드 구독
        SceneManager.sceneLoaded += OnSceneLoaded;

        StartCoroutine(TimeCoroutine());
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("씬 전환 성공?");
        player = FindAnyObjectByType<Player>();
        player.onPlayerDie += GameOver;
        StartCoroutine(TimeCoroutine());
    }

    /// <summary>
    /// 플레이한 시간
    /// </summary>
    public float timeElapsed = 0;

    /// <summary>
    /// 플레이 시간을 누적시키는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeCoroutine()
    {
        while (!player.gameOver)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// 플레이어의 사망으로 바닥의 이동 속도를 0으로 만드는 함수
    /// </summary>
    private void SpeedZero()
    {
        currentGroundMoveSpeed = 0;
    }

    /// <summary>
    /// 게임이 오버되었을때 실행되는 함수
    /// </summary>
    public void GameOver()
    {
        Debug.Log("게임 오버");

        SpeedZero();

        // 점수 = (달린 시간 * 10) + 돈 + 젤리
        Debug.Log($"달린 시간 : {timeElapsed}");
        score = (Mathf.FloorToInt(timeElapsed) * 10) + (int)Money + (int)Jelly;

        onGameOver?.Invoke(score);
    }

    /// <summary>
    /// 바닥 속도 증가 함수
    /// </summary>
    /// <param name="increment"></param>
    public void IncreaseSpeed(float increment)
    {
        currentGroundMoveSpeed += increment;
        Debug.Log($"현재 바닥 속도: {currentGroundMoveSpeed}");
    }
}
