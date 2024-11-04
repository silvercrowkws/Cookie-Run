using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        currentMoney = 0;

        // 초기 속도 설정
        currentGroundMoveSpeed = baseGroundMoveSpeed;

        player.onPlayerDie += SpeedZero;
    }

    /// <summary>
    /// 플레이어의 사망으로 속도를 0으로 만드는 함수
    /// </summary>
    private void SpeedZero()
    {
        currentGroundMoveSpeed = 0;
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
