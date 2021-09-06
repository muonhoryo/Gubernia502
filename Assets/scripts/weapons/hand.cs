using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand : weapon
{
    public trailRenHitBox rightHand = null;
    public trailRenHitBox leftHand =null;
    public override void generateSound()
    {
        ermakLockControl.soundGenerator.soundLevel = Gubernia502.constData.ermakHandsSoundVolume;
    }
    public virtual void meleeHitEnabled()
    {
        if (ermakLockControl.ermakAnim.GetInteger("punchNum") != 1)
        {
            ermakLockControl.meleeShoot.hitBox = rightHand;
            ermakLockControl.ermakAnim.SetInteger("punchNum", 1);
        }
        else
        {
            ermakLockControl.meleeShoot.hitBox = leftHand;
            ermakLockControl.ermakAnim.SetInteger("punchNum", 2);
        }
    }
    public override void shoot()
    {
        ermakLockControl.meleeLock();
        ermakLockControl.bodyRotationSpeed = Gubernia502.constData.ermakMeleeBodyRotSpeed;
        ermakLockControl.bodyRotateScript.neededDirectionAngle = ermakLockControl.viewBodyScript.neededHeadDirection;
        ermakLockControl.setFullAnim();
        ermakLockControl.ermakAnim.SetFloat("move", 8);
        ermakLockControl.ermakAnim.SetTrigger("shoot");
        meleeHitEnabled();
    }
    protected override void Start()
    {
        if (rightHand != null)
        {
            rightHand.hitDmg = Gubernia502.constData.ermakHandsDmg;
        }
        if (leftHand != null)
        {
            leftHand.hitDmg = Gubernia502.constData.ermakHandsDmg;
        }
    }
}
