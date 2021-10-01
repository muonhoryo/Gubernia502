using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobWeaponHitBox : MonoBehaviour
{
    public mobBehavior mobBehavior;
    public Collider coll;
    public List<hitPointSystem> damagedHPSys = new List<hitPointSystem> { };
    protected void dmgHPsystem(Collider other,hitPointSystem hitPointSystem)
    {
        if (mobBehavior.isStunnedDmg && other.TryGetComponent(out alifeDmgSystem alifeDmgSystem))
        {
            alifeDmgSystem.getNormalStunDmg(Gubernia502.constData.mobSimpleAttackDmg,
            mobBehavior.bodyRotateScript.rotatedBody.rotation.eulerAngles.y,mobBehavior.transform.position);
        }
        else
        {
            hitPointSystem.takeNormalDamage(Gubernia502.constData.mobSimpleAttackDmg,
                mobBehavior.bodyRotateScript.rotatedBody.rotation.eulerAngles.y, mobBehavior.transform.position);
        }
        damagedHPSys.Add(hitPointSystem);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != mobBehavior.gameObject&&
            other.TryGetComponent(out hitPointSystem hitPointSystem)&&
            !mobBehavior.meleeFrontHitBox.damagedHPSys.Contains(hitPointSystem) &&
            !Gubernia502.constData.mobsFriendFractions.Contains(hitPointSystem.Fraction))
        {
            dmgHPsystem(other, hitPointSystem);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out hitPointSystem hitPointSystem) && damagedHPSys.Contains(hitPointSystem))
        {
            damagedHPSys.Remove(hitPointSystem);
        }
    }
}
