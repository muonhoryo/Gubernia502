using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class serednyakMeleeShoot : meleeShoot
{
    public bool isNextAttack = false;
    public override void takeSignal2()
    {
        if (hitBox != null)
        {
            hitBox.setActiveHitBox(false);
        }
        ermakLockControl.meleeFrontHitBox.setActiveHitBox(false);
        if (isNextAttack)
        {
            isNextAttack = false;
            Gubernia502.playerController.setNeededHeadDirection();
            ermakLockControl.iteractionScript.selectedWeaponScript.shoot();
        }
        else
        {
            ermakLockControl.unlockCtrl();
            ermakLockControl.meleeLockRotation();
        }
    }
}
