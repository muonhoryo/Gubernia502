using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axe : hand
{
    public override void generateSound()
    {
        generateSound(NPCLockControl.Inventory.EquippedWeapons.weaponsItem.soundVolume);
    }
    public override void meleeHitEnabled()
    {
        NPCLockControl.meleeShoot.hitBox = leftHand;
        if (NPCLockControl.animator.GetInteger("punchNum") != 1)
        {
            NPCLockControl.animator.SetInteger("punchNum", 1);
        }
        else
        {
            NPCLockControl.animator.SetInteger("punchNum", 2);
        }
    }
    protected override void Start()
    {
        base.Start();
        leftHand.NPCLockControl = NPCLockControl;
    }
}
