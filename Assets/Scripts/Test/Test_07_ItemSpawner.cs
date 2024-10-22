using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_07_ItemSpawner : TestBase
{
    /// <summary>
    /// 아이템 스포너
    /// </summary>
    ItemSpawner itemSpawner;

    public GameObject itemRushPrefabs;

    private void Start()
    {
        itemSpawner = FindAnyObjectByType<ItemSpawner>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Instantiate(itemRushPrefabs, itemSpawner.transform.position, Quaternion.identity);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Player player;
        player = GameManager.Instance.Player;

        player.HP -= 30;
    }
}
