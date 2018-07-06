using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleEnemy : Enemy {

    public override void InitializeItemDrops()
    {
        base.InitializeItemDrops();

        itemDrops.Add("HealthDrop_Small", 1.0f);
    }
}
