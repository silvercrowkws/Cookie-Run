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
                moneyChange?.Invoke(currentMoney);
            }
        }
    }

    /// <summary>
    /// 돈이 변경되었음을 알리는 델리게이트(UI 수정용)
    /// </summary>
    public Action<float> moneyChange;

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
