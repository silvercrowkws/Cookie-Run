using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    /// <summary>
    /// 슬라이더
    /// </summary>
    Slider slider;

    /// <summary>
    /// Fill 부분의 이미지
    /// </summary>
    Image fillImage;
    
    /// <summary>
    /// 노란색(1 ~ 0.5 구간)
    /// </summary>
    private Color yellowColor = Color.yellow;               // 노란색

    /// <summary>
    /// 주황색(0.5 ~ 0.3 구간)
    /// </summary>
    private Color orangeColor = new Color(1f, 0.5f, 0f);    // 주황색

    /// <summary>
    /// 빨간색(0.3 ~ 0 구간)
    /// </summary>
    private Color redColor = Color.red;                     // 빨간색

    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 1;                                   // 시작 시 슬라이더 1로 설정

        Transform child = transform.GetChild(1);            // 1번째 자식 Fill Area
        child = child.GetChild(0);                          // Fill Area의 0번째 자식 Fill
        fillImage = child.GetComponent<Image>();            // Fill의 이미지
    }

    void Start()
    {
        player = GameManager.Instance.Player;
        player.onHPChange += OnHPChange;

        // Slider 값이 변경될 때마다 OnSliderValueChanged 호출
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        OnSliderValueChanged(slider.value);                 // 초기 값 설정
    }

    /// <summary>
    /// HP 변경에 따라 슬라이더 값을 변경하는 함수
    /// </summary>
    /// <param name="currentHP">남은 체력</param>
    private void OnHPChange(float currentHP)
    {
        slider.value = currentHP / 100;
    }

    /// <summary>
    /// Slider 값에 따라 색상을 변경하는 함수
    /// </summary>
    /// <param name="value">슬라이더의 값</param>
    void OnSliderValueChanged(float value)
    {
        if (value > 0.5f)
        {
            // 1부터 0.5까지는 노란색
            fillImage.color = yellowColor;
        }
        else if (value > 0.3f)
        {
            // 0.5부터 0.3까지는 주황색
            fillImage.color = orangeColor;
        }
        else
        {
            // 0.3부터 0까지는 빨간색
            fillImage.color = redColor;
        }
    }
}
