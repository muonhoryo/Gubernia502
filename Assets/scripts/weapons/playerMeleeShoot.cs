using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMeleeShoot : meleeShoot//singltone
{
    static playerMeleeShoot singltone=null;
    public bool isNextAttack = false;
    public override void takeSignal2()
    {
        if (hitBox != null)
        {
            hitBox.setActiveHitBox(false);
        }
        NPCLockControl.meleeFrontHitBox.setActiveHitBox(false);
        if (isNextAttack)
        {
            isNextAttack = false;
            Gubernia502.playerController.setNeededHeadDirection();
            NPCLockControl.iteractionScript.selectedWeaponScript.shoot();
        }
        else
        {
            NPCLockControl.unlockCtrl();
            NPCLockControl.meleeLockRotation();
        }
    }
    private void Awake()
    {
        if (singltone == null)
        {
            singltone = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
