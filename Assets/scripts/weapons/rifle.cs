using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rifle : pistol
{
    public override void reload()
    {

        if (ermakLockControl.ermakInventory.EquippedWeapons.ammoInMag <
               ermakLockControl.ermakInventory.EquippedWeapons.maxAmmoInMag &&
               ermakLockControl.ermakInventory.isHaveAmmo(ermakLockControl.ermakInventory.EquippedWeapons.currentAmmoId))
        {
            ermakLockControl.distantShoot.nonAnimatedMagazine = nonAnimatedMagazine;
            ermakLockControl.ermakAnim.SetBool("reload",true);
            ermakLockControl.lockOtherCtrl();
            GetComponent<Animator>().SetTrigger("reload");
        }
    }
}
