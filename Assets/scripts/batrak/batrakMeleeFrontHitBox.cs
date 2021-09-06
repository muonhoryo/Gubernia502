using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batrakMeleeFrontHitBox : batrakWeaponHitBox
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != batrakBehavior.gameObject && other.TryGetComponent(out hitPointSystem hitPointSystem) &&
            !batrakBehavior.meleeShoot.hitBox.damagedHPSys.Contains(hitPointSystem))
        {
            hitPointSystem.takeNormalDamage(Gubernia502.constData.batrakSimpleAttackDmg,
                batrakBehavior.bodyRotateScript.rotatedBody.rotation.eulerAngles.y, transform.position);
            damagedHPSys.Add(hitPointSystem);
        }
    }
}
