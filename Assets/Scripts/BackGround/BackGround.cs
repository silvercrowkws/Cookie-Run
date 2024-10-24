using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// 뒷배경 스포너
    /// </summary>
    BackGroundSpawner backgroundSpawner;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        backgroundSpawner = FindAnyObjectByType<BackGroundSpawner>();

        StartCoroutine(MoveLeftCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Debug.Log("배경이 데드존과 충돌");

            // 새로운 배경 생성 해야 됨
            backgroundSpawner.SpawnBackGround();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Debug.Log("배경이 데드존과 충돌 끝");

            Destroy(gameObject);
        }
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
            transform.position += Vector3.left * 0.5f * gameManager.currentGroundMoveSpeed * Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }
    }
}
