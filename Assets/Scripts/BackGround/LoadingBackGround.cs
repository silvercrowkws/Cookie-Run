using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingBackGround : MonoBehaviour
{
    public float shakeDuration = 2.5f;  // 흔들리는 시간
    public float shakeAmount = 3.0f;    // 흔들림 강도
    public float shakeSpeed = 5.0f;     // 흔들림 속도

    private Vector2 originalPosition;   // 오브젝트의 초기 위치

    /// <summary>
    /// 로딩이 끝났다고 알리는 델리게이트
    /// </summary>
    public Action onLoadingEnd;

    /// <summary>
    /// 블랙 패널
    /// </summary>
    //BlackPanel blackPanel;
    
    /// <summary>
    /// 화면 흔드는 코루틴이 종료되었는지 확인하는 bool변수(true : 종료, false : 진행 중)
    /// </summary>
    public bool shakeEnd = false;

    /// <summary>
    /// 화면 진동 시작했다고 알리는 코루틴
    /// </summary>
    public Action onShake;

    private void Start()
    {
        originalPosition = transform.localPosition; // 처음 위치 저장

        StartCoroutine(ShakeCoroutine());           // 코루틴 시작
    }

    IEnumerator ShakeCoroutine()
    {
        shakeEnd = true;
        onShake?.Invoke();

        float elapsed = 0.0f;


        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            // Mathf.Sin을 사용하여 좌우로 흔들리게 만듦
            float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            transform.localPosition = originalPosition + new Vector2(offsetX, 0);

            yield return null;
        }

        // 흔들림 종료 후 원래 위치로 돌아감
        transform.localPosition = originalPosition;

        shakeEnd = false;

        onLoadingEnd?.Invoke();
        //SceneManager.LoadScene(2);
    }
}
