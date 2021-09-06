using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermakIteraction : MonoBehaviour
{
    private ermakLockControl ermakLockControl;
    public GameObject usedObj;
    public MonoBehaviour usedObjScript;
    public weapon selectedWeaponScript;
    public bool isActiveIteraction = true;
    public void changeFireMode()
    {
        selectedWeaponScript.changeShootMode();
    }
    public void firstIteraction()
    {
        if (usedObj != null) 
        { 
            usedObjScript.BroadcastMessage("Use", SendMessageOptions.DontRequireReceiver);
        }
    }
    public void fire1()
    {
        if (selectedWeaponScript.coolDown <= 0)
        {
            selectedWeaponScript.shoot();
        }
    }
    public void reload()
    {
        selectedWeaponScript.reload();
    }
    public void changeAmmo()
    {
        if (ermakLockControl.ermakInventory.EquippedWeapons.weaponsItem.ammotTypes.Count > 1)
        {
            selectedWeaponScript.changeAmmoType();
        }
    }
    private void Start()
    {
        ermakLockControl = GetComponent<ermakLockControl>();
        ermakLockControl.ermakInventory.selectWeapon(0);
    }
}
