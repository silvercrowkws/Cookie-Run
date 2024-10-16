using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Rush : ItemBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        itemDuration = 15.0f;
    }
}
