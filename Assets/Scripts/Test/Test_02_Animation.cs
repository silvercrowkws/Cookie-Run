using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_02_Animation : TestBase
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Run", true);      // 초기 세팅은 Run true 상태
    }    

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Time.timeScale = 1.0f;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Time.timeScale = 0.1f;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Debug.Log("3번 키 누름");
        Debug.Log("슬라이딩");

        animator.SetBool("Run", false);
        animator.SetBool("Booster", false);
        animator.SetBool("Sliding", true);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Debug.Log("4번 키 누름");
        Debug.Log("점프");

        animator.SetBool("Run", false);         // 점프 중에 바닥에 닿기 전까지는 끄고 닿으면 키고
        animator.SetBool("Booster", false);
        animator.SetTrigger("Jump");

        // 바닥에 다시 닿기 전에는 점프가 안눌리게 해야할듯
        // 더블 점프는 눌리게
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        Debug.Log("5번 키 누름");
        Debug.Log("더블 점프");

        animator.SetTrigger("DoubleJump");

        // 더블 점프는 점프가 눌리기 전에는 안되게 해야됨
    }


}
