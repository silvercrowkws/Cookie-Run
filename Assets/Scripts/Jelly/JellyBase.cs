using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyBase : MonoBehaviour
{
    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    JellySpawner jellySpawner;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        jellySpawner = FindAnyObjectByType<JellySpawner>();
        StartCoroutine(MoveLeftCoroutine());
    }

    /// <summary>
    /// 초당 n 씩 왼쪽으로 움직이는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveLeftCoroutine()
    {
        // 무한 루프: 코루틴에서 매 프레임마다 오브젝트를 왼쪽으로 이동
        while (true)
        {
            // 왼쪽으로 이동
            //transform.position += Vector3.left * groundMoveSpeed * Time.deltaTime;

            // 부모의 현재 속도를 사용해 바닥 이동
            transform.position += Vector3.left * gameManager.currentGroundMoveSpeed * Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Debug.Log("젤리가 데드존과 충돌");

            // 새로운 젤리 생성 해야 됨
            jellySpawner.SpawnJelly();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Debug.Log("젤리가 데드존과 충돌 끝");

            Destroy(gameObject);
        }
    }
}
