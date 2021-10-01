using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgun : automat
{
    delegate void shootAction();
    shootAction action=delegate() { };
    void setReload(bool mode)
    {
        NPCLockControl.weaponDispersion.leftLine.SetActive(!mode);
        NPCLockControl.weaponDispersion.rightLine.SetActive(!mode);
        NPCLockControl.animator.SetBool("reload", mode);
    }
    public override void reload()
    {
        if (NPCLockControl.animator.GetBool("reload"))
        {
            setReload(false);
        }
        else
        {
            if (NPCLockControl.Inventory.EquippedWeapons.ammoInMag <
                   NPCLockControl.Inventory.EquippedWeapons.maxAmmoInMag &&
                   NPCLockControl.Inventory.isHaveAmmo(NPCLockControl.Inventory.EquippedWeapons.currentAmmoId))
            {
                setReload(true);
                action = delegate () { setReload(false); action = delegate () { }; };
            }
        }
    }
    public override void shoot()
    {
        action();
        base.shoot();
    }
}
