using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.Unicode;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 플레이어 인풋 액션
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// 리지드바디
    /// </summary>
    Rigidbody2D rigid;

    /// <summary>
    /// 콜라이더
    /// </summary>
    BoxCollider2D col;

    /// <summary>
    /// 바닥에서 떨어지기 전에 Run 상태였는지 확인하는 bool 변수
    /// 처음에는 Run 상태이기 때문에 true
    /// </summary>
    bool isRun = true;

    /// <summary>
    /// 바닥에서 떨어지기 전에 Booster 상태였는지 확인하는 bool 변수
    /// </summary>
    bool isBooster;

    /// <summary>
    /// 현재 점프가 가능한지 확인하는 bool 변수
    /// </summary>
    bool jumpAble = true;

    /// <summary>
    /// 현재 더블 점프가 가능한지 확인하는 bool 변수
    /// </summary>
    bool doublejumpAble = false;

    /// <summary>
    /// 현재 슬라이딩이 가능한지 확인하는 bool 변수
    /// </summary>
    bool slidingAble = false;

    public float jumpPower = 5.0f;
    public float doublejumpPower = 2.5f;

    Vector2 jumpOffset = new Vector2(0, -0.55f);
    Vector2 defaultOffset = new Vector2(0, -0.65f);
    Vector2 slidingOffset = new Vector2(0, -0.952394f);

    Vector2 slidingSize = new Vector2(1, 0.6952119f);
    Vector2 defaultSize = new Vector2(1, 1.3f);

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        animator = GetComponent<Animator>();
        animator.SetBool("Run", true);      // 초기 세팅은 Run true 상태
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Move.Jump.performed += Jump;
        inputActions.Move.Sliding.started += StartSliding;      // 슬라이딩을 눌렀을 때 시작
        inputActions.Move.Sliding.canceled += CancelSliding;    // 슬라이딩을 땠을 때 끝
    }

    private void OnDisable()
    {
        inputActions.Move.Sliding.canceled -= CancelSliding;    // 슬라이딩을 땠을 때 끝
        inputActions.Move.Sliding.started -= StartSliding;
        inputActions.Move.Jump.performed -= Jump;
        inputActions.Disable();
    }

    private void Start()
    {
        
    }

    /// <summary>
    /// 키보드용 점프 함수(스페이스바와 연결)
    /// </summary>
    /// <param name="context"></param>
    private void Jump(InputAction.CallbackContext context)
    {
        // 현재 애니메이터 상태가 Run 애니메이션인지 확인
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetCurrentAnimatorStateInfo(0).IsName("Booster"))
        //if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !animator.GetCurrentAnimatorStateInfo(0).IsName("DoubleJump"))
        if (jumpAble)
        {
            Debug.Log("점프");

            jumpAble = false;           // 점프 이후에는 점프가 불가능하게 설정
            animator.SetTrigger("Jump");
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            colFix();

            // doublejumpAble을 여기서 바로 true로 하지 말고, 일정 시간 후 활성화
            StartCoroutine(EnableDoubleJumpCoroutine());
        }

        // 현재 애니메이터 상태가 Jump 애니메이션인지 확인
        if (doublejumpAble && animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            Debug.Log("더블 점프");

            doublejumpAble = false;     // 더블 점프 후에는 다시 불가능
            animator.SetTrigger("DoubleJump");
            rigid.AddForce(Vector2.up * doublejumpPower, ForceMode2D.Impulse);

            colFix();
        }
    }

    /// <summary>
    /// 키보드용 슬라이딩 시작 함수(컨트롤과 연결)
    /// </summary>
    /// <param name="context"></param>
    private void StartSliding(InputAction.CallbackContext context)
    {
        if (slidingAble)
        {
            onGetBool();        //슬라이딩 전에 무슨 상태였는지 기록

            Debug.Log("슬라이딩 시작");

            animator.SetBool("Run", false);
            animator.SetBool("Booster", false);
            animator.SetBool("Sliding", true);

            colFix();
        }
    }

    /// <summary>
    /// 키보드용 슬라이딩 종료 함수(컨트롤과 연결)
    /// </summary>
    /// <param name="context"></param>
    private void CancelSliding(InputAction.CallbackContext context)
    {
        // 공중에서 이 함수가 호출되어도 상관은 없음
        Debug.Log("슬라이딩 끝");

        animator.SetBool("Sliding", false);
        animator.SetBool("Run", isRun);                 // 바닥에서 떨어지기 전에 Run 상태였으면 Run 활성화
        animator.SetBool("Booster", isBooster);         // 바닥에서 떨어지기 전에 Booster 상태였으면 Booster 활성화

        colFix();
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            animator.SetBool("Run", isRun);                 // 바닥에서 떨어지기 전에 Run 상태였으면 Run 활성화
            animator.SetBool("Booster", isBooster);         // 바닥에서 떨어지기 전에 Booster 상태였으면 Booster 활성화
        }
    }*/

    /// <summary>
    /// 충돌 중일 때
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("땅과 충돌 중");

            // 현재 재생중인 애니메이션이 Run 또는 Booster이면
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || animator.GetCurrentAnimatorStateInfo(0).IsName("Booster"))
            {
                if (!jumpAble)      // 점프가 불가능 했을 경우에만 코루틴 실행
                {
                    StartCoroutine(JumpDelayCoroutine());
                    // 코루틴 실행 후에는 jumpAble = true
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("땅과 충돌");

            // 착지 시 모든 점프 관련 상태 초기화
            jumpAble = true;         // 점프 가능하게 설정
            slidingAble = true;      // 슬라이딩 가능하게 설정
            doublejumpAble = false;  // 더블 점프 불가하게 설정 (착지 후 다시 초기화 필요)

            if (!animator.GetBool("Sliding"))                   // 슬라이딩 상태가 아니면
            {
                animator.SetBool("Run", isRun);                 // 바닥에서 떨어지기 전에 Run 상태였으면 Run 활성화
                animator.SetBool("Booster", isBooster);         // 바닥에서 떨어지기 전에 Booster 상태였으면 Booster 활성화
            }

            //StartCoroutine(JumpDelayCoroutine());

            // 애니메이션 상태 초기화
            animator.ResetTrigger("Jump");      // Jump 애니메이션 트리거를 초기화
            animator.ResetTrigger("DoubleJump"); // DoubleJump 애니메이션 트리거를 초기화
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isRun = animator.GetBool("Run");               // 바닥에서 떨어지기 전에 무슨 상태였는지 확인
            isBooster = animator.GetBool("Booster");       // 바닥에서 떨어지기 전에 무슨 상태였는지 확인

            // 점프 중에 바닥에 닿기 전까지는 꺼짐
            animator.SetBool("Run", false);
            animator.SetBool("Booster", false);
        }
    }*/

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("땅에서 떨어짐");
            //isRun = animator.GetBool("Run");              // 바닥에서 떨어지기 전에 무슨 상태였는지 확인
            //isBooster = animator.GetBool("Booster");      // 바닥에서 떨어지기 전에 무슨 상태였는지 확인

            onGetBool();

            slidingAble = false;                            // 바닥에서 떨어졌을 때는 슬라이딩 안되게


            // 슬라이딩 중 땅에서 떨어져서 런과 부스터가 안이어지는 버그 있음
            // 점프 중에 바닥에 닿기 전까지는 꺼짐
            animator.SetBool("Run", false);
            animator.SetBool("Booster", false);
        }
    }

    /// <summary>
    /// 다시 점프 가능하게 만드는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator JumpDelayCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        //yield return new WaitForEndOfFrame();
        jumpAble = true;
        //slidingAble = true;
        //doublejumpAble = true;
    }

    /// <summary>
    /// 이전에 무슨 상태였는지 기록용 함수
    /// </summary>
    private void onGetBool()
    {
        isRun = animator.GetBool("Run");               // 바닥에서 떨어지기 전에 무슨 상태였는지 확인        
        isBooster = animator.GetBool("Booster");       // 바닥에서 떨어지기 전에 무슨 상태였는지 확인

        Debug.Log($"isRun = {isRun}");
        Debug.Log($"isBooster = {isBooster}");
    }

    // 점프 후 일정 시간 후에 더블 점프 가능하도록 조정
    IEnumerator EnableDoubleJumpCoroutine()
    {
        yield return new WaitForSeconds(0.2f); // 조금의 딜레이 후에 더블 점프 가능하게 설정
        doublejumpAble = true;
    }

    IEnumerator delayCoroutine()
    {
        yield return new WaitForSeconds(0.35f);
    }

    /// <summary>
    /// 콜라이더의 오프셋과 사이즈를 변경하는 함수
    /// </summary>
    private void colFix()
    {
        if (animator.GetBool("Jump"))
        {
            col.offset = jumpOffset;            // 점프 했을 때는 콜라이더 Offset 수정
            col.size = defaultSize;             // 크기 디폴트
            Debug.Log("점프로 인한 크기 조절");
        }
        else if (animator.GetBool("Sliding"))
        {
            col.offset = slidingOffset;
            col.size = slidingSize;
            Debug.Log("슬라이딩으로 인한 크기 조절");

            // 물리 엔진 강제 업데이트
        }
        else
        {
            col.offset = defaultOffset;         // 점프 이외의 상황에는 콜라이더 Offset 원래대로
            col.size = defaultSize;             // 크기 디폴트
            Debug.Log("점프이외 인한 크기 조절");
        }
    }
}
