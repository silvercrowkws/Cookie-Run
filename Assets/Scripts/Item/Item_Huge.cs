using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Huge : ItemBase
{
    protected override void Awake()
    {
        base.Awake();
        //onItemUse += Huge;
    }

    protected override void Start()
    {
        base.Start();
        itemDuration = 10.0f;
    }
}
