using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ground_Obstacle : MonoBehaviour
{
    /// <summary>
    /// 땅에서 위로 올라올 장애물
    /// </summary>
    Transform obstacle;

    /// <summary>
    /// 초당 8의 속도
    /// </summary>
    float speed = 8.0f;

    private void Awake()
    {
        //Transform childGround = transform.GetChild(0);
        obstacle = transform.GetChild(0);
    }

    private void Start()
    {
        //StartCoroutine(ObstacleUP());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RiseZone"))
        {
            StartCoroutine(ObstacleUP());       // 손 올라오는 코루틴 시작
        }
    }

    /// <summary>
    /// 장애물을 위로 올리는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator ObstacleUP()
    {
        Debug.Log("ObstacleUP 코루틴 시작");

        // 부모 오브젝트 기준으로 2까지 올라갈 때 까지 반복
        while (obstacle.localPosition.y < 2.0f)
        {
            obstacle.localPosition += Vector3.up * speed * Time.deltaTime;
            yield return null;  // 다음 프레임까지 대기
        }
    }
}
