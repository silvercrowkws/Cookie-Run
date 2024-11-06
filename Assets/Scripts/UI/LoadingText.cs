using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    /// <summary>
    /// 로딩 뒷배경
    /// </summary>
    LoadingBackGround loadingBackGround;

    /// <summary>
    /// 로딩 텍스트
    /// </summary>
    TextMeshProUGUI loadingText;

    string baseText = "마녀를 피해 도망가는 중";

    private void Awake()
    {
        loadingText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        loadingBackGround = FindAnyObjectByType<LoadingBackGround>();
        loadingBackGround.onShake += OnShack;

        //StartCoroutine(DotCoroutine());             // 점 개수 늘리는 코루틴 시작
    }

    private void OnShack()
    {
        StartCoroutine(DotCoroutine());             // 점 개수 늘리는 코루틴 시작
    }

    IEnumerator DotCoroutine()
    {
        int dotCount = 0;       // 점의 갯수를 세는 변수

        Debug.Log("1번은 되고");
        while (loadingBackGround.shakeEnd)
        {
            Debug.Log("2번은 되고?");
            dotCount = (dotCount % 3) + 1;          // 1, 2, 3 반복

            // baseText 뒤에 dotCount 만큼 점을 추가
            loadingText.text = baseText + new string('.', dotCount);
            //dotCount++;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
