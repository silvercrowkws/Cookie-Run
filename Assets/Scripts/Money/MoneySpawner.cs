using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoneySpawner : MonoBehaviour
{
    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    // money 모양 프리팹
    public GameObject Money_Gold_Plus;
    public GameObject Money_Gold_Stairs;
    public GameObject Money_Gold_Line;
    public GameObject Money_Silver_Line;
    public GameObject Money_Silver_Arrow;
    public GameObject Money_Silver_Heart;

    // Money_Gold_Plus 의 최대최소 위치
    int goldPlusMin = -2;
    int goldPlusMax = 4;

    // Money_Gold_Stairs 의 최대최소 위치
    int goldStairsMin = -3;
    int goldStairsMax = 0;

    // Money_Gold_Line 의 최대최소 위치
    int goldLineMin = -2;
    int goldLineMax = 5;

    // Money_Silver_Line 의 최대최소 위치
    int silverLineMin = -3;
    int silverLineMax = 5;

    // Money_Silver_Arrow 의 최대최소 위치
    int silverArrowMin = -1;
    int silverArrowMax = 3;

    // Money_Silver_Heart 의 최대최소 위치
    int silverSmileMin = 2;
    int silverSmileMax = 4;

    /*
    Money_Gold_Plus = y값이 -2 ~ 3 사이
    Money_Gold_Stairs = y값이 - 3 ~ -1 사이
    Money_Gold_Line = y값이 -2 ~ 4 사이
    Money_Silver_Line = y값이 - 3 ~ 4 사이
    Money_Silver_Arrow = y값이 - 1 ~ 2 사이
    Money_Silver_Heart = y값이 0 ~ 3 사이
    */

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        SpawnMoney();
    }

    /// <summary>
    /// 돈을 스폰시키는 함수
    /// </summary>
    public void SpawnMoney()
    {
        int randomMoneyShape = UnityEngine.Random.Range(0, 6);

        // 스폰 위치의 Y 값을 랜덤으로 결정하는 변수
        int randomY = 0;

        // 스폰 위치 업데이트 (현재 X 값은 유지, Y 값만 랜덤으로 변경)
        Vector2 spawnPosition = new Vector2(0,0);       // = new Vector2(transform.position.x, randomY);

        // money 변수 선언
        GameObject money = null;

        switch (randomMoneyShape)
        {
            case 0:
                randomY = UnityEngine.Random.Range(goldPlusMin, goldPlusMax);
                spawnPosition = new Vector2(transform.position.x, randomY);
                money = Instantiate(Money_Gold_Plus, spawnPosition, Quaternion.identity, transform);
                break;
            case 1:
                randomY = UnityEngine.Random.Range(goldStairsMin, goldStairsMax);
                spawnPosition = new Vector2(transform.position.x, randomY);
                money = Instantiate(Money_Gold_Stairs, spawnPosition, Quaternion.identity, transform);
                break;
            case 2:
                randomY = UnityEngine.Random.Range(silverLineMin, silverLineMax);
                spawnPosition = new Vector2(transform.position.x, randomY);
                money = Instantiate(Money_Silver_Line, spawnPosition, Quaternion.identity, transform);
                break;
            case 3:
                randomY = UnityEngine.Random.Range(silverArrowMin, silverArrowMax);
                spawnPosition = new Vector2(transform.position.x, randomY);
                money = Instantiate(Money_Silver_Arrow, spawnPosition, Quaternion.identity, transform);
                break;
            case 4:
                randomY = UnityEngine.Random.Range(goldLineMin, goldLineMax);
                spawnPosition = new Vector2(transform.position.x, randomY);
                money = Instantiate(Money_Gold_Line, spawnPosition, Quaternion.identity, transform);
                break;
            case 5:
                randomY = UnityEngine.Random.Range(silverSmileMin, silverSmileMax);
                spawnPosition = new Vector2(transform.position.x, randomY);
                money = Instantiate(Money_Silver_Heart, spawnPosition, Quaternion.identity, transform);
                break;
        }        

        //GameObject money = Instantiate(Money_Gold_Plus, spawnPosition, Quaternion.identity, transform);
    }
}
