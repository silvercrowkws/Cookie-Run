using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_06_HP : TestBase
{
    Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.HP -= 10;
        Debug.Log($"플레이어의 남은 체력 : {player.HP}");
    }
}
