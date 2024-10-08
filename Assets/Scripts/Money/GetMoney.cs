using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMoney : MonoBehaviour
{
    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 충돌했으면
        if (collision.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("Gold"))
            {
                gameManager.Money += 5;
            }
            else if(this.gameObject.CompareTag("Silver"))
            {
                gameManager.Money += 1;
            }

            // 여기에 소리 추가하면 됨
            Destroy(gameObject);
        }
    }
}
