using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellySpawner : MonoBehaviour
{
    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    // jelly 모양 프리팹
    public GameObject Jelly_V;
    public GameObject Jelly_Circle;
    public GameObject Jelly_Candy;
    public GameObject Jelly_OneAngel;

    // Jelly_V 의 최대최소 위치
    float jellyVMin = -2.5f;
    float jellyVMax = -0.5f;

    // Jelly_Circle 의 최대최소 위치
    float jellyCircleMin = -0.5f;
    float jellyCircleMax = 2.5f;

    // Jelly_Candy 의 최대최소 위치
    float jellyCandyMin = -0.5f;
    float jellyCandyMax = 2.5f;

    // Jelly_OneAngel 의 최대최소 위치
    float jellyOneAngelMin = -2f;
    float jellyOneAngelMax = 3.5f;


    /*
    Jelly_V         = y값이 -2.5 ~ -0.5 사이
    Jelly_Circle    = y값이 -0.5 ~ 2.5 사이
    Jelly_Candy     = y값이 -0.5 ~ 2.5 사이
    */

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        SpawnJelly();
    }

    /// <summary>
    /// 돈을 스폰시키는 함수
    /// </summary>
    public void SpawnJelly()
    {
        int randomMoneyShape = UnityEngine.Random.Range(0, 4);

        // 스폰 위치의 Y 값을 랜덤으로 결정하는 변수
        float randomY = 0;

        // 스폰 위치 업데이트 (현재 X 값은 유지, Y 값만 랜덤으로 변경)
        Vector2 spawnPosition = new Vector2(0, 0);       // = new Vector2(transform.position.x, randomY);

        // money 변수 선언
        GameObject jelly = null;

        switch (randomMoneyShape)
        {
            case 0:
                randomY = UnityEngine.Random.Range(jellyVMin, jellyVMax);
                spawnPosition = new Vector2(transform.position.x, randomY);
                jelly = Instantiate(Jelly_V, spawnPosition, Quaternion.identity, transform);
                break;
            case 1:
                randomY = UnityEngine.Random.Range(jellyCircleMin, jellyCircleMax);
                spawnPosition = new Vector2(transform.position.x, randomY);
                jelly = Instantiate(Jelly_Circle, spawnPosition, Quaternion.identity, transform);
                break;
            case 2:
                randomY = UnityEngine.Random.Range(jellyCandyMin, jellyCandyMax);
                spawnPosition = new Vector2(transform.position.x, randomY);
                jelly = Instantiate(Jelly_Candy, spawnPosition, Quaternion.identity, transform);
                break;
            case 3:
                randomY = UnityEngine.Random.Range(jellyOneAngelMin, jellyOneAngelMax);
                spawnPosition = new Vector2(transform.position.x, randomY);
                jelly = Instantiate(Jelly_OneAngel, spawnPosition, Quaternion.identity, transform);
                break;
        }
    }
}
