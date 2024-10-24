using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSpawner : MonoBehaviour
{
    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// 백그라운드0
    /// </summary>
    public GameObject BackGround_0;

    /// <summary>
    /// 이전에 스폰된 배경의 위치
    /// </summary>
    private Transform lastSpawnedBackground;

    /// <summary>
    /// 이전 배경과 벌어질 거리
    /// </summary>
    Vector2 spawnPosition = new Vector2(21.77f, 0);

    private void Awake()
    {
        
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        FirstSpawnBackGround();
    }

    /// <summary>
    /// 게임 시작 시 첫번째 뒷배경을 스폰하는 함수
    /// </summary>
    public void FirstSpawnBackGround()
    {
        GameObject background = Instantiate(BackGround_0, transform.position, Quaternion.identity, transform);

        // 마지막으로 스폰된 배경의 위치를 저장
        lastSpawnedBackground = background.transform;
    }

    /// <summary>
    /// 뒷배경을 스폰하는 함수
    /// </summary>
    public void SpawnBackGround()
    {
        // 마지막 배경의 끝 지점에서 새로운 배경을 스폰하도록 위치 계산
        Vector2 newPosition = new Vector2(lastSpawnedBackground.position.x + 21.77f, lastSpawnedBackground.position.y);

        // 새로운 배경 생성
        GameObject background = Instantiate(BackGround_0, newPosition, Quaternion.identity, transform);

        // 마지막 배경 위치 업데이트
        lastSpawnedBackground = background.transform;
    }
}
