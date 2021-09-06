using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batrakWeaponHitBox : MonoBehaviour
{
    public batrakBehavior batrakBehavior;
    public Collider coll;
    public List<hitPointSystem> damagedHPSys = new List<hitPointSystem> { };
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != batrakBehavior.gameObject&&other.TryGetComponent(out hitPointSystem hitPointSystem)&&
            !batrakBehavior.meleeFrontHitBox.damagedHPSys.Contains(hitPointSystem))
        {
            hitPointSystem.takeNormalDamage(Gubernia502.constData.batrakSimpleAttackDmg,
                batrakBehavior.bodyRotateScript.rotatedBody.rotation.eulerAngles.y,transform.position);
            damagedHPSys.Add(hitPointSystem);
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
