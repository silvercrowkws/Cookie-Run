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


    private void Awake()
    {
        gameManager = GameManager.Instance;

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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
}
