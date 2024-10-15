using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Huge : ItemBase
{
    protected override void Awake()
    {
        base.Awake();
        //onItemUse += Huge;
    }

    protected override void Start()
    {
        base.Start();
        itemDuration = 10.0f;
    }

    /*/// <summary>
    /// 플레이어를 일정 시간동안 거대화 시키는 코루틴을 실행시키는 함수
    /// </summary>
    private void Huge()
    {
        StartCoroutine(HugeCoroutine(hugeelTime));
    }

    /// <summary>
    /// 플레이어를 거대화 시키는 코루틴
    /// </summary>
    /// <param name="hugeelTime">거대화 지속시간</param>
    /// <returns></returns>
    IEnumerator HugeCoroutine(float hugeelTime)
    {
        // 서서히 크기를 키우기
        float timeElapsed = 0f;

        while (timeElapsed < hugeelTime)
        {
            playerObject.localScale = Vector3.Lerp(defaultScale, hugeScale, timeElapsed / hugeelTime);
            timeElapsed += Time.deltaTime;
            yield return null; // 프레임마다 업데이트
        }
        playerObject.localScale = hugeScale; // 최종적으로 hugeScale로 설정

        yield return new WaitForSeconds(itemDuration);

        // 지속시간 후 원래 크기로 되돌리기
        timeElapsed = 0f;

        while (timeElapsed < hugeelTime)
        {
            playerObject.localScale = Vector3.Lerp(hugeScale, defaultScale, timeElapsed / hugeelTime);
            timeElapsed += Time.deltaTime;
            yield return null; // 프레임마다 업데이트
        }
        playerObject.localScale = defaultScale; // 최종적으로 defaultScale로 설정
    }*/
}
