using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectibleSimpleItem : collectiblleItem
{
    ~collectibleSimpleItem()
    {
        Gubernia502.items.Remove(this);
    }
    private void OnDestroy()
    {
        Gubernia502.items.Remove(this);
    }
    public simpleItem item;
    public override void OnMouseOver()
    {
        base.OnMouseOver();
        Gubernia502.playerController.NPCLockControl.iteractionScript.usedObj = gameObject;
        Gubernia502.playerController.NPCLockControl.iteractionScript.usedObjScript =this;
    }
    public override void addItem(NPCLockControl ermakLockControl)
    {
        ermakLockControl.Inventory.addItem(this);
    }
    private void Awake()
    {
        Gubernia502.items.Add(this);
    }
}
