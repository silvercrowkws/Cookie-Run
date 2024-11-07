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

    /// <summary>
    /// 오디오 소스
    /// </summary>
    AudioSource audioSource;

    private void Awake()
    {
        startButton = GetComponentInChildren<Button>();
        audioSource = GetComponentInChildren<AudioSource>();
        startButton.onClick.AddListener(GameStart);
    }

    /// <summary>
    /// 시작 씬에서 게임 시작 버튼으로 씬 전환하는 함수
    /// </summary>
    private void GameStart()
    {
        audioSource.Play();

        SceneManager.LoadScene(1);
    }
}
