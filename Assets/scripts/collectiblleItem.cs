using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class collectiblleItem : MonoBehaviour
{
    [Range(1,simpleItem.maxCount)]
    public int count = 1;
    [SerializeField]
    MeshRenderer itemMesh;
    public virtual void OnMouseOver()
    {
        itemMesh.materials[0].SetFloat("selectBloomLevel", 1);
    }
    public virtual void OnMouseExit()
    {
        itemMesh.materials[0].SetFloat("selectBloomLevel", 0);
        Gubernia502.playerController.ermakLockControl.iteractionScript.usedObj = null;
        Gubernia502.playerController.ermakLockControl.iteractionScript.usedObjScript = null;
    }
    public virtual void addItem(ermakLockControl ermakLockControl)
    {
    }
    public void Use()
    {
        Use(Gubernia502.playerController.ermakLockControl);
        Gubernia502.playerController.ermakLockControl.iteractionScript.usedObj = null;
        Gubernia502.playerController.ermakLockControl.iteractionScript.usedObjScript = null;
    }
    public void shadowUse()
    {
        addItem(Gubernia502.playerController.ermakLockControl);
    }
    public virtual void Use(ermakLockControl ermakLockControl)
    {
        addItem(ermakLockControl);
        if (!ermakLockControl.ermakAnim.GetBool("reload"))
        {
            ermakLockControl.pickUpItem();
        }
    }
}
