using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Parent : MonoBehaviour
{
    /*/// <summary>
    /// 기본 속도
    /// </summary>
    public float baseGroundMoveSpeed = 3.0f;

    /// <summary>
    /// 현재 속도
    /// </summary>
    public float currentGroundMoveSpeed;*/

    /// <summary>
    /// 땅 프리팹
    /// </summary>
    public GameObject groundPrefabs;

    /// <summary>
    /// 슬라이딩 장애물 프리팹
    /// </summary>
    public GameObject upObstaclePrefabs;

    /// <summary>
    /// 현재 사이클
    /// </summary>
    public int cycle = 0;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    private void Awake()
    {
        // 초기 속도 설정
        //currentGroundMoveSpeed = baseGroundMoveSpeed;
    }

    private void Start()
    {
        SpawnFirstGround();
        gameManager = GameManager.Instance;
    }

    /*/// <summary>
    /// 바닥 속도 증가 함수
    /// </summary>
    /// <param name="increment"></param>
    public void IncreaseSpeed(float increment)
    {
        currentGroundMoveSpeed += increment;
        Debug.Log($"현재 바닥 속도: {currentGroundMoveSpeed}");
    }*/

    /// <summary>
    /// 게임 생성 시 첫 번째 땅을 생성하는 함수
    /// </summary>
    private void SpawnFirstGround()
    {
        // 땅 생성
        GameObject ground = Instantiate(groundPrefabs, transform.GetChild(0).position, Quaternion.identity, transform);
        //Ground groundComponent = ground.GetComponent<Ground>();
    }

    /// <summary>
    /// 땅을 생성하는 함수
    /// </summary>
    public void SpawnGround()
    {
        gameManager.IncreaseSpeed(0.25f);

        int randomGround = UnityEngine.Random.Range(0, 10);     // 0 ~ 10 사이 숫자 뽑기

        if(randomGround < 5)        // 0 or 1이니까 20% 확률로
        {
            // 슬라이딩 땅 생성
            GameObject ground = Instantiate(upObstaclePrefabs, transform.position, Quaternion.identity, transform);
        }
        else
        {
            // 일반 땅 생성
            GameObject ground = Instantiate(groundPrefabs, transform.position, Quaternion.identity, transform);
        }
        //Ground groundComponent = ground.GetComponent<Ground>();
    }
}
