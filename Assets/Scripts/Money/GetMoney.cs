using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMoney : MonoBehaviour
{
    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// 오디오 소스
    /// </summary>
    AudioSource audioSource;

    /// <summary>
    /// 스프라이트 렌더러
    /// </summary>
    SpriteRenderer spriteRenderer;

    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        player = GameManager.Instance.Player;
        player.onMagnet += OnMagnet;        // 활성화될 때마다 이벤트 구독
    }

    private void OnDisable()
    {
        player.onMagnet -= OnMagnet;        // 비활성화될 때 이벤트 해제
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 충돌했으면
        if (collision.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("Gold"))
            {
                gameManager.Money += 5;

                // 소리 재생
                audioSource.Play();
            }
            else if(this.gameObject.CompareTag("Silver"))
            {
                gameManager.Money += 1;

                // 소리 재생
                audioSource.Play();
            }

            // 알파 값을 0으로 변경하여 사라지게 함
            Color color = spriteRenderer.color;
            color.a = 0; // 알파 값을 0으로 설정
            spriteRenderer.color = color; // 색상 업데이트

            // 오브젝트 삭제            
            //Destroy(gameObject);
            // 소리가 재생되는 동안 약간의 대기 시간을 두고 삭제
            StartCoroutine(DestroyAfterSound());
        }
    }

    /// <summary>
    /// 소리가 끝날 때까지 대기한 후 삭제시키는 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyAfterSound()
    {
        // 소리가 끝날 때까지 대기
        yield return new WaitForSeconds(audioSource.clip.length);

        // 게임 오브젝트 삭제
        Destroy(gameObject);
    }

    /// <summary>
    /// 마그넷 코루틴을 실행시키는 함수
    /// </summary>
    /// <param name="itemDuration">플레이어가 Item_Magnet에서 받아온 아이템 지속 시간</param>
    private void OnMagnet(float itemDuration)
    {
        //Debug.Log("플레이어 델리게이트 받아서 함수 실행");
        StartCoroutine(MagnetCoroutine(itemDuration));
    }

    /// <summary>
    /// 돈이 플레이어의 위치를 향해 움직이게 하는 코루틴
    /// </summary>
    /// <param name="itemDuration">아이템 지속 시간</param>
    /// <returns></returns>
    IEnumerator MagnetCoroutine(float itemDuration)
    {
        float timeElapsed = 0;

        // 플레이어의 트랜스폼
        Transform targetObject = player.transform;

        while (timeElapsed < itemDuration)
        {
            // 현재 돈이 어디에 있던 플레이어를 쫒아오는게 문제인데..

            // 타겟 방향 계산
            Vector3 direction = (targetObject.position - transform.position).normalized;

            // 타겟 방향으로 이동
            transform.position = Vector3.MoveTowards(transform.position, targetObject.position, 2 * gameManager.currentGroundMoveSpeed * Time.deltaTime);

            // 경과 시간 업데이트
            timeElapsed += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 지속 시간이 끝난 후의 처리
        Debug.Log("아이템 효과 종료");
        Debug.LogWarning("MagnetCoroutine 코루틴 끝");
    }
}
