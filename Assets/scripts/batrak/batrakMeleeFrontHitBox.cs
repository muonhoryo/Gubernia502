using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batrakMeleeFrontHitBox : batrakWeaponHitBox
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != batrakBehavior.gameObject && 
            other.TryGetComponent(out hitPointSystem hitPointSystem) &&
            !batrakBehavior.meleeShoot.hitBox.damagedHPSys.Contains(hitPointSystem)&&
            !Gubernia502.constData.batrakFriendFractions.Contains(hitPointSystem.Fraction))
        {
            dmgHPsystem(other, hitPointSystem);
        }
    }
}
