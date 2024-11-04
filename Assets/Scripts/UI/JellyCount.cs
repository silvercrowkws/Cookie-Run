using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JellyCount : MonoBehaviour
{
    /// <summary>
    /// 1000의 자리 텍스트
    /// </summary>
    TextMeshProUGUI Jelly_1000;

    /// <summary>
    /// 100의 자리 텍스트
    /// </summary>
    TextMeshProUGUI Jelly_100;

    /// <summary>
    /// 10의 자리 텍스트
    /// </summary>
    TextMeshProUGUI Jelly_10;

    /// <summary>
    /// 1의 자리 텍스트
    /// </summary>
    TextMeshProUGUI Jelly_1;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        InitializeNumbers();
    }

    private void InitializeNumbers()
    {
        Jelly_1000 = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        Jelly_100 = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        Jelly_10 = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        Jelly_1 = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onJellyChange += OnJellyChange;
    }

    /// <summary>
    /// 게임매니저의 젤리가 변경되었을을 알리는 델리게이트로 연결되어 UI를 갱신하는 함수
    /// </summary>
    /// <param name="jelly"></param>
    private void OnJellyChange(float jelly)
    {
        // money를 정수로 변환
        int coin = Mathf.FloorToInt(jelly);

        // 1000의 자리
        int coin_1000 = (coin / 1000) % 10;
        Jelly_1000.text = coin_1000.ToString();

        // 100의 자리
        int coin_100 = (coin / 100) % 10;
        Jelly_100.text = coin_100.ToString();

        // 10의 자리
        int coin_10 = (coin / 10) % 10;
        Jelly_10.text = coin_10.ToString();

        // 10의 자리
        int coin_1 = coin % 10;
        Jelly_1.text = coin_1.ToString();
    }
}
