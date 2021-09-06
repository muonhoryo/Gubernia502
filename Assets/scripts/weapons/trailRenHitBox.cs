using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trailRenHitBox : meleeHitBox
{
    [SerializeField]
    private TrailRenderer trailRenderer;
    IEnumerator disactiveEffect(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        trailRenderer.enabled = false;
        yield break;
    }
    public override void setActiveHitBox(bool isActive)
    {
        base.setActiveHitBox(isActive);
        if (isActive)
        {
            trailRenderer.emitting = true;
            StopAllCoroutines();
            trailRenderer.enabled = true;
        }
        else
        {
            trailRenderer.emitting = false;
            StartCoroutine(disactiveEffect(trailRenderer.time));
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != ermakLockControl.gameObject)
        {
            if (other.TryGetComponent(out hitPointSystem hpSystem) && !ermakLockControl.meleeFrontHitBox.damagedHPSys.Contains(hpSystem))
            {
                hpSystem.takeNormalDamage(hitDmg, ermakLockControl.transform.rotation.eulerAngles.y, transform.position);
                if (ermakLockControl.ermakInventory.hand.enabled == false)
                {
                    if (ermakLockControl.ermakSelectedWeapon.GetComponentInChildren<weapon>().takeDurabilityDmg())
                    {
                        ermakLockControl.ermakAnim.SetInteger("punchNum", -1);
                        ermakLockControl.isBreakWeapon(ermakLockControl.viewBodyScript.ermakBody.transform.rotation.eulerAngles.y);
                    }
                }
                damagedHPSys.Add(hpSystem);
            }
        }
    }
}
