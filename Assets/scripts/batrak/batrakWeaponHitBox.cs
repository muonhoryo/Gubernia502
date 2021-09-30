using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batrakWeaponHitBox : MonoBehaviour
{
    public batrakBehavior batrakBehavior;
    public Collider coll;
    public List<hitPointSystem> damagedHPSys = new List<hitPointSystem> { };
    protected void dmgHPsystem(Collider other,hitPointSystem hitPointSystem)
    {
        if (batrakBehavior.isStunnedDmg && other.TryGetComponent(out alifeDmgSystem alifeDmgSystem))
        {
            alifeDmgSystem.getNormalStunDmg(Gubernia502.constData.batrakSimpleAttackDmg,
            batrakBehavior.bodyRotateScript.rotatedBody.rotation.eulerAngles.y,batrakBehavior.transform.position);
        }
        else
        {
            hitPointSystem.takeNormalDamage(Gubernia502.constData.batrakSimpleAttackDmg,
                batrakBehavior.bodyRotateScript.rotatedBody.rotation.eulerAngles.y, batrakBehavior.transform.position);
        }
        damagedHPSys.Add(hitPointSystem);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != batrakBehavior.gameObject&&
            other.TryGetComponent(out hitPointSystem hitPointSystem)&&
            !batrakBehavior.meleeFrontHitBox.damagedHPSys.Contains(hitPointSystem) &&
            !Gubernia502.constData.batrakFriendFractions.Contains(hitPointSystem.Fraction))
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
