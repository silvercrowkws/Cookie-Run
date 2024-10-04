using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Parent : MonoBehaviour
{
    /// <summary>
    /// 기본 속도
    /// </summary>
    public float baseGroundMoveSpeed = 3.0f;

    /// <summary>
    /// 현재 속도
    /// </summary>
    public float currentGroundMoveSpeed;

    /// <summary>
    /// 땅 프리팹
    /// </summary>
    public GameObject groundPrefabs;

    /// <summary>
    /// 현재 사이클
    /// </summary>
    public int cycle = 0;

    private void Awake()
    {
        // 초기 속도 설정
        currentGroundMoveSpeed = baseGroundMoveSpeed;
    }

    private void Start()
    {
        SpawnFirstGround();
    }

    /// <summary>
    /// 바닥 속도 증가 함수
    /// </summary>
    /// <param name="increment"></param>
    public void IncreaseSpeed(float increment)
    {
        currentGroundMoveSpeed += increment;
        Debug.Log($"현재 바닥 속도: {currentGroundMoveSpeed}");
    }

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
        IncreaseSpeed(0.25f);

        // 땅 생성
        GameObject ground = Instantiate(groundPrefabs, transform.position, Quaternion.identity, transform);
        //Ground groundComponent = ground.GetComponent<Ground>();

        // 여기여 확률적으로 그냥 땅이 생성되는 것이 아니라 슬라이딩 땅이 생성되는 부분 필요할 듯(위에 땅 생성 부분까지 if문으로 묶어서)
    }
}
