using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("트리거 데드존 스크립트에서 그라운드 충돌 검출");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("트리거 데드존 스크립트에서 그라운드 충돌 해제 검출");
        }
    }
}
