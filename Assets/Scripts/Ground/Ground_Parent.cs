using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Parent : MonoBehaviour
{
    public GameObject groundPrefabs;

    private void Awake()
    {
        // 자식이 동적으로 늘어나는데
        //Transform child = transform.GetChild(0);
    }

    private void Start()
    {
        SpawnFirstGround();
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
        // 땅 생성
        GameObject ground = Instantiate(groundPrefabs, transform.position, Quaternion.identity, transform);
        //Ground groundComponent = ground.GetComponent<Ground>();

        // 여기여 확률적으로 그냥 땅이 생성되는 것이 아니라 슬라이딩 땅이 생성되는 부분 필요할 듯(위에 땅 생성 부분까지 if문으로 묶어서)
    }
}
