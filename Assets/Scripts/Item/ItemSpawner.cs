using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // 아이템 프리팹
    public GameObject item_Huge_Prefabs;
    public GameObject item_Rush_Prefabs;
    public GameObject item_Magnet_Prefabs;
    public GameObject item_HealPotion_Prefabs;

    /// <summary>
    /// 아이템 스폰 간격
    /// </summary>
    public float spawninterval = 20.0f;

    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;
        StartCoroutine(ItemSpawnCoroutine(spawninterval));
    }

    IEnumerator ItemSpawnCoroutine(float spawninterval)
    {
        // 플레이어의 HP가 0보다 크고, 게임이 진행중이면
        while (player.HP > 0 && !player.gameOver)
        {
            int spawnItemNumber = UnityEngine.Random.Range(0, 4);       // 0,1,2,3 뽑기

            switch (spawnItemNumber)
            {
                case 0:
                    Instantiate(item_Huge_Prefabs, transform.position, Quaternion.identity, transform);
                    break;
                case 1:
                    Instantiate(item_Rush_Prefabs, transform.position, Quaternion.identity, transform);
                    break;
                case 2:
                    Instantiate(item_Magnet_Prefabs, transform.position, Quaternion.identity, transform);
                    break;
                case 3:
                    Instantiate(item_HealPotion_Prefabs, transform.position, Quaternion.identity, transform);
                    break;
            }
            yield return new WaitForSeconds(spawninterval);
        }
    }
}
