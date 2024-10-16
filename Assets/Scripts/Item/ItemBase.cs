using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    /// <summary>
    /// 아이템의 이동속도
    /// </summary>
    protected float itemMoveSpeed = 3.0f;

    /// <summary>
    /// 아이템의 지속 시간
    /// </summary>
    public float itemDuration;

    /// <summary>
    /// 아이템을 사용하라고 알리는 델리게이트
    /// </summary>
    public Action onItemUse;

    /// <summary>
    /// 콜라이더
    /// </summary>
    CircleCollider2D circleCollider2D;
    
    /// <summary>
    /// 스프라이트 렌더러
    /// </summary>
    SpriteRenderer spriteRenderer;

    /// <summary>
    /// 알파값이 0인 컬러
    /// </summary>
    Color zeroAlphaColor;

    /// <summary>
    /// 알파값이 1인 컬러
    /// </summary>
    Color oneAlphaColor;

    /// <summary>
    /// 플레이어
    /// </summary>
    //protected Player player;

    protected virtual void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        zeroAlphaColor = spriteRenderer.color;
        zeroAlphaColor.a = 0;

        oneAlphaColor = spriteRenderer.color;
        oneAlphaColor.a = 1;
        spriteRenderer.color = oneAlphaColor;           // 생성될 때는 알파값 1로 설정

        circleCollider2D.enabled = true;               // 콜라이더를 활성화
    }

    protected virtual void Start()
    {
        //player = GameManager.Instance.Player;
        StartCoroutine(MoveLeftItemCoroutine());
    }

    /// <summary>
    /// 충돌했을 때
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 충돌했을 때
        if (collision.CompareTag("Player"))
        {
            Debug.Log("아이템과 충돌");
            circleCollider2D.enabled = false;           // 콜라이더를 비활성화
            spriteRenderer.color = zeroAlphaColor;      // 안보이게 알파값 0으로 변경
        }

        // 데드존과 충돌했을 때
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            Debug.Log("아이템이 데드존과 충돌");
            //Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 아이템을 왼쪽으로 이동시키는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveLeftItemCoroutine()
    {
        // 무한 루프
        while (true)
        {
            transform.position += Vector3.left * itemMoveSpeed * Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }
    }
}
