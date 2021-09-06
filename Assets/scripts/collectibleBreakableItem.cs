using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectibleBreakableItem :collectiblleItem
{
    [Range(1, 1000)]
    public int durability=1;
    public simpleItem item;
    public breakableItem breakItem;
    public override void OnMouseOver()
    {
        base.OnMouseOver();
        Gubernia502.playerController.ermakLockControl.iteractionScript.usedObj = gameObject;
        Gubernia502.playerController.ermakLockControl.iteractionScript.usedObjScript =this;
    }
    public override void addItem(ermakLockControl ermakLockControl)
    {
        ermakLockControl.ermakInventory.addBreakItem(this);
    }
}
