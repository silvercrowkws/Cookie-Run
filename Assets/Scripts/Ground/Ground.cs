using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    /// <summary>
    /// 바닥의 콜라이더
    /// </summary>
    BoxCollider2D groundCollider;

    /// <summary>
    /// Ground_0 의 프리팹
    /// </summary>
    public GameObject Ground_0_Prefabs;

    /// <summary>
    /// Ground_Obstacle_0 의 프리팹
    /// </summary>
    public GameObject Ground_Obstacle_0_Prefabs;

    // 속도 정의 (초당 1씩 왼쪽으로 움직이도록 설정)
    public float speed = 1f;
    
    Vector2 groundSize = new Vector2(0, 1.583333f);

    /// <summary>
    /// 오브젝트 간격 설정 (1.65씩 떨어뜨림)
    /// </summary>
    public float objectSpacing = 1.65f;

    /*/// <summary>
    /// 자식 오브젝트
    /// </summary>
    Transform childGround;*/

    /// <summary>
    /// 생성된 오브젝트를 저장할 리스트
    /// </summary>
    private List<GameObject> spawnGroundList = new List<GameObject>();

    private void Start()
    {
        //childGround = transform.GetChild(0);

        //groundCollider = childGround.GetComponent<BoxCollider2D>();
        groundCollider = GetComponent<BoxCollider2D>();

        // 3 ~ 10 사이의 숫자 뽑기
        int randomNumber = Random.Range(3, 11);

        // 뽑은 숫자가 짝수이면
        if (randomNumber % 2 == 0)
        {
            // 음수 양수 반복해서 짝수일 때는 음수가 먼저 생성되기 때문에 오프셋 이동
            groundCollider.offset = new Vector2(-0.825f, 0);
            //Debug.Log("오프셋 이동");
        }

        // 뽑은 숫자만큼 Ground_0_Prefabs 생성
        for (int i = 0; i < randomNumber; i++)
        {
            // x축으로 얼만큼 이동할 지
            float xPosition;

            // i가 0일 때는 0, 1일 때는 1.65, 2일 때는 -1.65, 3일 때는 3.3, 4일 때는 -3.3
            if (i == 0)
            {
                xPosition = 0; // 첫 번째 오브젝트는 0
            }
            else
            {
                // 부호를 결정하기 위해 짝수/홀수 체크
                xPosition = (i + 1) / 2 * objectSpacing; // 1.65의 배수
                if (i % 2 == 1) // 홀수 인덱스일 경우 음수
                {
                    xPosition *= -1;
                }
            }


            // 10% 확률로 Ground_Obstacle_0_Prefabs 생성, 그렇지 않으면 Ground_0_Prefabs 생성
            GameObject prefabToInstantiate;
            if (Random.Range(0, 100) < 10) // 10% 확률
            {
                prefabToInstantiate = Ground_Obstacle_0_Prefabs;
            }
            else
            {
                prefabToInstantiate = Ground_0_Prefabs;
            }

            // 이쯤에 바닥 한 종류만 생성되는 것이 아니라 장애물이 있는 바닥도 확률적으로 생성되게 해야 할듯
            //Vector2 position = new Vector2(xPosition, transform.position.y);
            //Instantiate(Ground_0_Prefabs, position, Quaternion.identity, transform);

            // 위치에 프리팹 생성
            Vector2 position = new Vector2(xPosition, transform.position.y);
            //Instantiate(prefabToInstantiate, position, Quaternion.identity, childGround);
            GameObject spawnedObject = Instantiate(prefabToInstantiate, position, Quaternion.identity, transform);

            // 생성된 오브젝트 이름 변경
            spawnedObject.name = $"Ground_{i}"; //

            spawnGroundList.Add(spawnedObject); // 리스트에 추가
        }

        groundSize.x = (objectSpacing * randomNumber);
        groundCollider.size = groundSize;

        StartCoroutine(MoveLeftCoroutine());

        //GroundReCreat();
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
            transform.position += Vector3.left * speed * Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }
    }

    /// <summary>
    /// 리스트의 마지막과 그 전 것이 모두 통과했는지 확인하는 함수
    /// </summary>
    private void GroundReCreat()
    {
        // spawnGroundList의 길이 확인
        int listLength = spawnGroundList.Count;

        GameObject lastObject = spawnGroundList[listLength - 1].gameObject;         // 마지막 오브젝트
        Debug.Log($"마지막 오브젝트 이름 : {lastObject}");

        GameObject previousObject = spawnGroundList[listLength - 2].gameObject;     // 마지막 이전 오브젝트
        Debug.Log($"마지막 이전 오브젝트 이름 : {previousObject}");


    }

    // 왜 충돌이 안되지
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Debug.Log("데드존과 충돌");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Debug.Log("데드존과 충돌 끝");
        }
    }
}
