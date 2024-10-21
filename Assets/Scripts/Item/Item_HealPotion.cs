using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_HealPotion : ItemBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        itemDuration = 30.0f;       // 힐 포션에서는 지속시간이 아니라 회복량이 될 듯
    }
}
