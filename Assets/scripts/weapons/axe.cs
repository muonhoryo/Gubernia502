using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axe : hand
{
    public override void generateSound()
    {
        generateSound(ermakLockControl.ermakInventory.EquippedWeapons.weaponsItem.soundVolume);
    }
    public override void meleeHitEnabled()
    {
        ermakLockControl.meleeShoot.hitBox = leftHand;
        if (ermakLockControl.ermakAnim.GetInteger("punchNum") != 1)
        {
            ermakLockControl.ermakAnim.SetInteger("punchNum", 1);
        }
        else
        {
            ermakLockControl.ermakAnim.SetInteger("punchNum", 2);
        }
    }
    protected override void Start()
    {
        base.Start();
        leftHand.ermakLockControl = ermakLockControl;
    }
}
