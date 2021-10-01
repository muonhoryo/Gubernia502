using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobMeleeFrontHitBox : mobWeaponHitBox
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != mobBehavior.gameObject && 
            other.TryGetComponent(out hitPointSystem hitPointSystem) &&
            !mobBehavior.meleeShoot.hitBox.damagedHPSys.Contains(hitPointSystem)&&
            !Gubernia502.constData.mobsFriendFractions.Contains(hitPointSystem.Fraction))
        {
            dmgHPsystem(other, hitPointSystem);
        }
    }
}
