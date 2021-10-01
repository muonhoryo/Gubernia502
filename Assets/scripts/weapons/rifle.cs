using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rifle : pistol
{
    public override void reload()
    {

        if (NPCLockControl.Inventory.EquippedWeapons.ammoInMag <
               NPCLockControl.Inventory.EquippedWeapons.maxAmmoInMag &&
               NPCLockControl.Inventory.isHaveAmmo(NPCLockControl.Inventory.EquippedWeapons.currentAmmoId))
        {
            NPCLockControl.distantShoot.nonAnimatedMagazine = nonAnimatedMagazine;
            NPCLockControl.animator.SetBool("reload",true);
            NPCLockControl.lockOtherCtrl();
            GetComponent<Animator>().SetTrigger("reload");
        }
    }
}
