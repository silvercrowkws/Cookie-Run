using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlackPanel : MonoBehaviour
{
    LoadingBackGround LoadingBackGround;

    /// <summary>
    /// 이미지(알파값 변경을 위함)
    /// </summary>
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();

        // 초기 알파값을 0으로 설정하여 시작 시 투명하게 만듦
        Color color = image.color;
        color.a = 0f;
        image.color = color;
    }

    private void Start()
    {
        LoadingBackGround = FindAnyObjectByType<LoadingBackGround>();
        LoadingBackGround.onLoadingEnd += OnLoadingEnd;
    }

    /// <summary>
    /// 알파값을 조절하는 코루틴을 실행시키는 함수
    /// </summary>
    private void OnLoadingEnd()
    {
        StartCoroutine(AlpahChange());
    }

    IEnumerator AlpahChange()
    {
        Debug.Log("알파 조절 코루틴 실행");
        float duration = 1.0f; // 페이드 지속 시간
        float elapsed = 0.0f;

        Color color = image.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration); // 0에서 1로 알파값 증가
            color.a = alpha;
            image.color = color;
            yield return null;
        }

        SceneManager.LoadScene(2);
    }
}
