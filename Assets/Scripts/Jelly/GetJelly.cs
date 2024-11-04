using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetJelly : MonoBehaviour
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
    /// 박스 콜라이더 2D
    /// </summary>
    BoxCollider2D boxCollider;

    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        player = GameManager.Instance.Player;
        boxCollider.enabled = true;

        if (player != null)
        {
            player.onMagnet += OnMagnet;

            /*// 플레이어가 자석 아이템을 먹었으면
            if (player.isMagnet)
            {
                StartCoroutine(MagnetCoroutine());
            }*/
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MagnetZone"))
        {
            // 플레이어가 자석 아이템을 먹었으면
            if (player.isMagnet)
            {
                StartCoroutine(MagnetCoroutine());
            }
        }

        // 플레이어와 충돌했으면
        if (collision.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("Jelly_Normal"))
            {
                gameManager.Jelly += 1;
                boxCollider.enabled = false;

                // 소리 재생
                // audioSource.Play();
            }
            else if (this.gameObject.CompareTag("Jelly_Red"))
            {
                gameManager.Jelly += 3;
                boxCollider.enabled = false;
            }
            else if (this.gameObject.CompareTag("Jelly_BigYellow"))
            {
                gameManager.Jelly += 10;
                boxCollider.enabled = false;
            }
            else if (this.gameObject.CompareTag("Jelly_Star"))
            {
                gameManager.Jelly += 15;
                boxCollider.enabled = false;
            }
            else if (this.gameObject.CompareTag("Jelly_Angel"))
            {
                gameManager.Jelly += 30;
                boxCollider.enabled = false;
            }

            // 소리 재생
            audioSource.Play();

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
    /// 이미 생성되어 있는 젤리는 따로 델리게이트로 추적 활성화 시킴
    /// </summary>
    private void OnMagnet()
    {
        StartCoroutine(MagnetCoroutine());
    }

    /// <summary>
    /// 젤리가 플레이어의 위치를 향해 움직이게 하는 코루틴
    /// </summary>
    /// <param name="itemDuration">아이템 지속 시간</param>
    /// <returns></returns>
    IEnumerator MagnetCoroutine()
    {
        //float timeElapsed = 0;

        // 플레이어의 트랜스폼
        Transform targetObject = player.transform;

        while (boxCollider.enabled)
        {
            // 타겟 방향 계산(슬라이딩 시 돈이 바로 안먹어지고 위에 쌓여서 0.5 내림)
            //Vector3 direction = (targetObject.position - transform.position).normalized;
            Vector3 targetPosition = new Vector3(targetObject.position.x, targetObject.position.y - 0.5f, targetObject.position.z);
            Vector3 direction = (targetPosition - transform.position).normalized;

            // 타겟 방향으로 이동
            //transform.position = Vector3.MoveTowards(transform.position, targetObject.position, 2 * gameManager.currentGroundMoveSpeed * Time.deltaTime);
            transform.position += direction * 2 * gameManager.currentGroundMoveSpeed * Time.deltaTime;

            // 경과 시간 업데이트
            //timeElapsed += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 지속 시간이 끝난 후의 처리
        //Debug.Log("아이템 효과 종료");
        //Debug.LogWarning("MagnetCoroutine 코루틴 끝");
    }
}
