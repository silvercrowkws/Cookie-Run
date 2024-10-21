using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.Unicode;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    Rigidbody2D playerRigid;

    /// <summary>
    /// 콜라이더
    /// </summary>
    BoxCollider2D playerCollider;

    /// <summary>
    /// 스프라이트 렌더러
    /// </summary>
    SpriteRenderer playerSpriteRenderer;

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

    /// <summary>
    /// 점프 파워
    /// </summary>
    public float jumpPower = 7.5f;

    /// <summary>
    /// 더블 점프 파워
    /// </summary>
    public float doublejumpPower = 5.0f;

    Vector2 jumpOffset = new Vector2(0.25f, -0.65f);
    Vector2 defaultOffset = new Vector2(0.25f, -0.9f);
    Vector2 slidingOffset = new Vector2(0, -1.347176f);

    Vector2 slidingSize = new Vector2(1, 1.005647f);
    Vector2 defaultSize = new Vector2(1, 1.9f);

    // 중력
    float defaultGravity = 0.75f;
    float changeGravity = 3.0f;

    /// <summary>
    /// 플레이어의 남은 체력
    /// </summary>
    float currentHP = 100;

    /// <summary>
    /// 플레이어의 체력 프로퍼티
    /// </summary>
    public float HP
    {
        get => currentHP;
        set
        {
            if (currentHP != value)     // 현재 체력이 변경되면
            {
                currentHP = Mathf.Clamp(value, 0, 100);     // 체력 클램프 0 ~ 100
                //Debug.Log($"남은 체력 : {currentHP}");
                onHPChange?.Invoke(currentHP);
            }
        }
    }

    /// <summary>
    /// 체력이 변경되었음을 알리는 델리게이트
    /// </summary>
    public Action<float> onHPChange;

    /// <summary>
    /// HP 감소 간격
    /// </summary>
    float hpMinusInterval = 1.0f;


    // 아이템 관련 --------------------------------------------------------------------------------

    /// <summary>
    /// 아이템 베이스
    /// </summary>
    ItemBase itemBase;

    /// <summary>
    /// 슬라이딩 예약 변수 (공중에서 슬라이딩 키를 눌렀을 때 true로 설정)
    /// </summary>
    bool slideReady = false;

    /// <summary>
    /// 거대화 스케일
    /// </summary>
    Vector3 hugeScale = new Vector3(3.5f, 3.5f, 1);

    /// <summary>
    /// 기본 스케일
    /// </summary>
    Vector3 defaultScale = new Vector3(1, 1, 1);

    /// <summary>
    /// 아이템 발동에 걸리는 시간
    /// </summary>
    float hugeelTime = 1.0f;

    /// <summary>
    /// 알파값이 0인 컬러
    /// </summary>
    Color zeroAlphaColor;

    /// <summary>
    /// 알파값이 1인 컬러
    /// </summary>
    Color oneAlphaColor;

    /// <summary>
    /// 아이템 사용 종료 깜빡이는 시간
    /// </summary>
    float blinkTime = 3.0f;

    /// <summary>
    /// 아이템 중복 사용 방지용 bool 변수
    /// </summary>
    bool itemAble = true;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        playerRigid = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
        animator.SetBool("Run", true);      // 초기 세팅은 Run true 상태
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Move.Jump.performed += Jump;
        inputActions.Move.Sliding.started += StartSliding;      // 슬라이딩을 눌렀을 때 시작
        //inputActions.Move.Sliding.performed += StartSliding;      // 슬라이딩을 눌렀을 때 시작
        inputActions.Move.Sliding.canceled += CancelSliding;    // 슬라이딩을 땠을 때 끝
    }

    private void OnDisable()
    {
        inputActions.Move.Sliding.canceled -= CancelSliding;    // 슬라이딩을 땠을 때 끝
        inputActions.Move.Sliding.started -= StartSliding;
        //inputActions.Move.Sliding.performed -= StartSliding;
        inputActions.Move.Jump.performed -= Jump;
        inputActions.Disable();
    }

    private void Start()
    {
        zeroAlphaColor = playerSpriteRenderer.color;
        zeroAlphaColor.a = 0;

        oneAlphaColor = playerSpriteRenderer.color;
        oneAlphaColor.a = 1;

        playerSpriteRenderer.color = oneAlphaColor;           // 생성될 때는 알파값 1로 설정

        StartCoroutine(HPChangeCoroutine());
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
            playerRigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

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
            playerRigid.AddForce(Vector2.up * doublejumpPower, ForceMode2D.Impulse);

            colFix();
        }
    }

    /// <summary>
    /// 키보드용 슬라이딩 시작 함수(컨트롤과 연결)
    /// </summary>
    /// <param name="context"></param>
    private void StartSliding(InputAction.CallbackContext context)
    {
        playerRigid.gravityScale = changeGravity;

        if (!slidingAble && !animator.GetBool("Sliding"))
        {
            // 공중에서 슬라이딩을 누르면 슬라이딩 예약만 하고 중력만 변경
            Debug.Log("슬라이딩 준비 중 (공중에서)");
            slideReady = true;
            return;
        }


        if (slidingAble)
        {
            //onGetBool();        //슬라이딩 전에 무슨 상태였는지 기록

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
        playerRigid.gravityScale = defaultGravity;

        slideReady = false;  // 슬라이딩 예약 취소


        // 공중에서 이 함수가 호출되어도 상관은 없음
        Debug.Log("슬라이딩 끝");

        animator.SetBool("Sliding", false);
        animator.SetBool("Run", isRun);                 // 바닥에서 떨어지기 전에 Run 상태였으면 Run 활성화
        animator.SetBool("Booster", isBooster);         // 바닥에서 떨어지기 전에 Booster 상태였으면 Booster 활성화

        colFix();
    }

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

            colFix();

            // 착지 시 모든 점프 관련 상태 초기화
            jumpAble = true;         // 점프 가능하게 설정
            slidingAble = true;      // 슬라이딩 가능하게 설정
            doublejumpAble = false;  // 더블 점프 불가하게 설정 (착지 후 다시 초기화 필요)


            // 슬라이딩 예약이 되어 있을 경우 슬라이딩 시작
            if (slideReady)
            {
                Debug.Log("땅에 닿았으므로 슬라이딩 시작");
                StartSliding(new InputAction.CallbackContext());
                slideReady = false;  // 슬라이딩 예약 해제
            }
            else
            {
                if (!animator.GetBool("Sliding"))                   // 슬라이딩 상태가 아니면
                {
                    animator.SetBool("Run", isRun);                 // 바닥에서 떨어지기 전에 Run 상태였으면 Run 활성화
                    animator.SetBool("Booster", isBooster);         // 바닥에서 떨어지기 전에 Booster 상태였으면 Booster 활성화
                }

                //StartCoroutine(JumpDelayCoroutine());

                // 애니메이션 트리거 초기화
                animator.ResetTrigger("Jump");      // Jump 애니메이션 트리거를 초기화
                animator.ResetTrigger("DoubleJump"); // DoubleJump 애니메이션 트리거를 초기화
            }


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

            onGetBool();                                    // 바닥에서 떨어지기 전에 무슨 상태였는지 확인

            slidingAble = false;                            // 바닥에서 떨어졌을 때는 슬라이딩 안되게


            // 슬라이딩 중 땅에서 떨어져서 런과 부스터가 안이어지는 버그 있음
            // 점프 중에 바닥에 닿기 전까지는 꺼짐
            animator.SetBool("Run", false);
            animator.SetBool("Booster", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Debug.Log("아이템 충돌 확인");
            itemBase = collision.GetComponent<ItemBase>();      // 충돌한 오브젝트의 아이템 베이스를 가져옴

            if (itemBase != null)
            {
                if(itemBase is Item_Huge itemHuge)
                {
                    //Debug.Log("itemHuge 찾음");
                    Huge();
                }
                else if(itemBase is Item_Rush itemRush)
                {
                    //Debug.Log("itemRush 찾음");
                    Rush();
                }
                else if(itemBase is Item_Magnet itemMagnet)
                {
                    Magnet();
                }
                else if(itemBase is Item_HealPotion itemHealPotion)
                {
                    HealPotion();
                }
            }
            else
            {
                Debug.LogWarning("ItemBase를 찾지 못함");
            }
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

    /// <summary>
    /// 콜라이더의 오프셋과 사이즈를 변경하는 함수
    /// </summary>
    private void colFix()
    {
        if (animator.GetBool("Jump"))
        {
            playerCollider.offset = jumpOffset;            // 점프 했을 때는 콜라이더 Offset 수정
            playerCollider.size = defaultSize;             // 크기 디폴트
            Debug.Log("점프로 인한 크기 조절");
        }
        else if (animator.GetBool("Sliding"))
        {
            playerCollider.offset = slidingOffset;
            playerCollider.size = slidingSize;
            Debug.Log("슬라이딩으로 인한 크기 조절");
        }
        else
        {
            playerCollider.offset = defaultOffset;         // 점프 이외의 상황에는 콜라이더 Offset 원래대로
            playerCollider.size = defaultSize;             // 크기 디폴트
            Debug.Log("점프이외 인한 크기 조절");
        }
    }

    /// <summary>
    /// 플레이어의 체력을 감소시키는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator HPChangeCoroutine()
    {
        while (HP > 0)
        {
            // 초당 1씩 감소하도록, 매 프레임마다 감소량을 계산
            HP -= hpMinusInterval * Time.deltaTime;
            yield return null;
            //yield return new WaitForSeconds(1);
        }
    }


    // 아이템 관련 --------------------------------------------------------------------------------

    /// <summary>
    /// 거대화 코루틴을 실행시키는 함수
    /// </summary>
    private void Huge()
    {
        if (itemAble)
        {
            StartCoroutine(HugeCoroutine(hugeelTime));
        }
        else
        {
            Debug.LogWarning("이미 아이템 사용중");
        }
    }

    /// <summary>
    /// 플레이어를 거대화 시키는 코루틴
    /// </summary>
    /// <param name="hugeelTime">거대화에 걸리는 시간</param>
    /// <returns></returns>
    IEnumerator HugeCoroutine(float hugeelTime)
    {
        Debug.Log($"거대화 아이템의 지속시간 : {itemBase.itemDuration}");
        Debug.Log("거대화는 10초가 정상");

        itemAble = false;
        Debug.Log("HugeCoroutine 코루틴 시작");

        // Obstacle과 충돌 무시
        IgnoreObstacleCollision(true);

        // 서서히 크기를 키우기
        float timeElapsed = 0f;

        while (timeElapsed < hugeelTime)
        {
            transform.localScale = Vector3.Lerp(defaultScale, hugeScale, timeElapsed / hugeelTime);
            timeElapsed += Time.deltaTime;
            yield return null;                      // 프레임마다 업데이트
        }
        transform.localScale = hugeScale;           // 최종적으로 hugeScale로 설정
        
        yield return new WaitForSeconds(itemBase.itemDuration - blinkTime);     // itemDuration - blinkTime 초에 깜빡임 시작 => 7초 후

        timeElapsed = 0f;

        // 3초 반복
        while (timeElapsed < blinkTime)
        {
            playerSpriteRenderer.color = zeroAlphaColor;        // 플레이어의 알파값 0으로 조정
            yield return new WaitForSeconds(0.2f);              // 0.2초 기다리고
            playerSpriteRenderer.color = oneAlphaColor;         // 플레이어의 알파값 1로 조정
            yield return new WaitForSeconds(1.0f);              // 1초 기다리고

            // 경과 시간 계산
            timeElapsed += 1.2f;                    // 0.2초 + 1.0초 대기 시간

            //yield return null;
        }

        // 지속시간 후 원래 크기로 되돌리기
        timeElapsed = 0f;

        while (timeElapsed < hugeelTime)
        {
            transform.localScale = Vector3.Lerp(hugeScale, defaultScale, timeElapsed / hugeelTime);
            timeElapsed += Time.deltaTime;
            yield return null;                      // 프레임마다 업데이트
        }
        transform.localScale = defaultScale;        // 최종적으로 defaultScale로 설정

        // Obstacle과 충돌 무시 해제
        IgnoreObstacleCollision(false);
        itemAble = true;
    }

    /// <summary>
    /// Obstacle 태그를 가진 오브젝트와의 충돌을 무시하거나 복구하는 함수
    /// </summary>
    /// <param name="ignore">true면 충돌 무시, false면 충돌 복구</param>
    void IgnoreObstacleCollision(bool ignore)
    {
        // ObstacleLayer 레이어와의 충돌을 무시
        int playerLayer = LayerMask.NameToLayer("PlayerLayer");
        int obstacleLayer = LayerMask.NameToLayer("ObstacleLayer");
        
        if (playerLayer < 0 || playerLayer > 31 || obstacleLayer < 0 || obstacleLayer > 31)
        {
            Debug.LogError("레이어 번호가 범위를 벗어났다.");
            return;
        }

        // 레이어 간의 충돌 무시 설정
        Physics2D.IgnoreLayerCollision(playerLayer, obstacleLayer, ignore);

        //Debug.Log($"Obstacle 레이어와의 충돌 무시 : {ignore}");
    }

    /// <summary>
    /// 러쉬 코루틴을 실행시키는 함수
    /// </summary>
    private void Rush()
    {
        if (itemAble)
        {
            StartCoroutine(RushCoroutine());
        }
        else
        {
            Debug.LogWarning("이미 아이템 사용중");
        }
    }

    /// <summary>
    /// 플레이어를 러쉬 시키는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator RushCoroutine()
    {
        Debug.Log($"러쉬 아이템의 지속시간 : {itemBase.itemDuration}");
        Debug.Log("러쉬는 15초가 정상");

        itemAble = false;
        Debug.Log("RushCoroutine 코루틴 시작");

        // Obstacle과 충돌 무시
        IgnoreObstacleCollision(true);

        ResetTrigger();

        animator.SetBool("Run", false);         // 달리기 비활성화
        animator.SetBool("Sliding", false);     // 슬라이딩 비활성화
        animator.SetBool("Booster", true);      // 부스터 활성화

        //yield return new WaitForSeconds(itemBase.itemDuration);
        yield return new WaitForSeconds(itemBase.itemDuration - blinkTime);     // itemDuration - blinkTime 초에 깜빡임 시작 => 7초 후

        float timeElapsed = 0f;

        // 3초 반복
        while (timeElapsed < blinkTime)
        {
            playerSpriteRenderer.color = zeroAlphaColor;        // 플레이어의 알파값 0으로 조정
            yield return new WaitForSeconds(0.2f);              // 0.2초 기다리고
            playerSpriteRenderer.color = oneAlphaColor;         // 플레이어의 알파값 1로 조정
            yield return new WaitForSeconds(1.0f);              // 1초 기다리고

            // 경과 시간 계산
            timeElapsed += 1.2f;                    // 0.2초 + 1.0초 대기 시간

            //yield return null;
        }

        animator.SetBool("Booster", false);     // 부스터 비활성화
        animator.SetBool("Sliding", false);     // 슬라이딩 비활성화
        animator.SetBool("Run", true);          // 달리기 비활성화

        // Obstacle과 충돌 해제
        IgnoreObstacleCollision(false);
        itemAble = true;
    }

    /// <summary>
    /// 자석 아이템을 먹었는지 확인하는 bool 변수
    /// </summary>
    public bool isMagnet = false;

    public Action onMagnet;

    /// <summary>
    /// 자석 아이템을 먹었을 때 실행되는 함수
    /// </summary>
    private void Magnet()
    {
        StartCoroutine(MagnetCoroutine());
        onMagnet?.Invoke();
    }

    /// <summary>
    /// 일정시간 동안 isMagnet 변수를 true로 바꾸는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator MagnetCoroutine()
    {
        Debug.Log($"마그넷 아이템의 지속시간 : {itemBase.itemDuration}");
        Debug.Log("마그넷은 30초가 정상");

        isMagnet = true;

        yield return new WaitForSeconds(itemBase.itemDuration);

        isMagnet = false;
    }

    /// <summary>
    /// HP 회복 포션으로 HP가 회복되는 함수
    /// </summary>
    private void HealPotion()
    {
        HP += itemBase.itemDuration;
        Debug.Log($"힐포션 아이템의 회복량 : {itemBase.itemDuration}");
        Debug.Log("힐포션은 30이 정상");
    }

    /// <summary>
    /// 트리거 초기화 함수
    /// </summary>
    public void ResetTrigger()
    {
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Booster");
        animator.ResetTrigger("Sliding");
    }
}
