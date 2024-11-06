using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    /// <summary>
    /// 게임시작 버튼
    /// </summary>
    Button startButton;

    private void Awake()
    {
        startButton = GetComponentInChildren<Button>();
        startButton.onClick.AddListener(GameStart);
    }

    /// <summary>
    /// 시작 씬에서 게임 시작 버튼으로 씬 전환하는 함수
    /// </summary>
    private void GameStart()
    {
        // 로딩 좀 추가하고 씬 넘어가는게 좋을 듯
        SceneManager.LoadScene(1);
    }
}
