using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectibleWeaponsItem : collectiblleItem
{
    ~collectibleWeaponsItem()
    {
        Gubernia502.weapons.Remove(this);
    }
    private void OnDestroy()
    {
        Gubernia502.weapons.Remove(this);
    }
    public simpleItem item;
    public breakableItem breakItem;
    public weaponsItem weaponItem;
    [SerializeField]
    MeshRenderer magMesh;
    //buffs
    [Range(0,weaponsItem.maxDamageBuff)]
    public int damageBuff=0;
    [Range(0, weaponsItem.maxSpeedBuff)]
    public int speedBuff=0;
    [Range(0, weaponsItem.maxMagSizeBuff)]
    public int magSizeBuff=0;
    [Range(0, weaponsItem.maxAccuracyBuff)]
    public int accuracyBuff=0;
    [Range(0, weaponsItem.maxDurabilityBuff)]
    public int durabilityBuff=0;
    [Range(0, weaponsItem.maxQuality)]
    public int quality=0;
    //ammotype
    [Range(0,8)]
    public int ammoId=0;
    //ammo
    [Range(0,2000)]
    public int ammoInMag=0;
    [Range(0,2000)]
    public int durability=1;
    public override void OnMouseOver()
    {
        base.OnMouseOver();
        Gubernia502.playerController.ermakLockControl.iteractionScript.usedObj = gameObject;
        Gubernia502.playerController.ermakLockControl.iteractionScript.usedObjScript = this;
        if (magMesh != null)
        {
            magMesh.materials[0].SetFloat("selectBloomLevel", 1);
        }
    }
    public override void OnMouseExit()
    {
        base.OnMouseExit();
        if (magMesh != null)
        {
            magMesh.materials[0].SetFloat("selectBloomLevel", 0);
        }
    }
    public override void addItem(ermakLockControl ermakLockControl)
    {
        ermakLockControl.ermakInventory.addWeapon(this);
    }
    private void Awake()
    {
        Gubernia502.weapons.Add(this);
    }
}
