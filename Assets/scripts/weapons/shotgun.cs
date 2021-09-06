using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgun : automat
{
    delegate void shootAction();
    shootAction action=delegate() { };
    void setReload(bool mode)
    {
        ermakLockControl.weaponDispersion.leftLine.SetActive(!mode);
        ermakLockControl.weaponDispersion.rightLine.SetActive(!mode);
        ermakLockControl.ermakAnim.SetBool("reload", mode);
    }
    public override void reload()
    {
        if (ermakLockControl.ermakAnim.GetBool("reload"))
        {
            setReload(false);
        }
        else
        {
            if (ermakLockControl.ermakInventory.EquippedWeapons.ammoInMag <
                   ermakLockControl.ermakInventory.EquippedWeapons.maxAmmoInMag &&
                   ermakLockControl.ermakInventory.isHaveAmmo(ermakLockControl.ermakInventory.EquippedWeapons.currentAmmoId))
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
