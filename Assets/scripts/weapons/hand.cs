using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand : weapon
{
    public trailRenHitBox rightHand = null;
    public trailRenHitBox leftHand =null;
    public override void generateSound()
    {
        NPCLockControl.soundGenerator.soundLevel = Gubernia502.constData.NPCHandsSoundVolume;
    }
    public virtual void meleeHitEnabled()
    {
        if (NPCLockControl.animator.GetInteger("punchNum") != 1)
        {
            NPCLockControl.meleeShoot.hitBox = rightHand;
            NPCLockControl.animator.SetInteger("punchNum", 1);
        }
        else
        {
            NPCLockControl.meleeShoot.hitBox = leftHand;
            NPCLockControl.animator.SetInteger("punchNum", 2);
        }
    }
    public override void shoot()
    {
        NPCLockControl.meleeLock();
        NPCLockControl.bodyRotationSpeed = Gubernia502.constData.NPCMeleeBodyRotSpeed;
        NPCLockControl.bodyRotateScript.neededDirectionAngle = NPCLockControl.viewBodyScript.neededHeadDirection;
        NPCLockControl.setFullAnim();
        NPCLockControl.animator.SetFloat("move", 8);
        NPCLockControl.animator.SetTrigger("shoot");
        meleeHitEnabled();
    }
    protected override void Start()
    {
        if (rightHand != null)
        {
            rightHand.hitDmg = Gubernia502.constData.NPCHandsDmg;
        }
        if (leftHand != null)
        {
            leftHand.hitDmg = Gubernia502.constData.NPCHandsDmg;
        }
    }
}
