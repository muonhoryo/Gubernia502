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
        if (other.gameObject != NPCLockControl.gameObject)
        {
            if (other.TryGetComponent(out hitPointSystem hpSystem) && !NPCLockControl.meleeFrontHitBox.damagedHPSys.Contains(hpSystem))
            {
                hpSystem.takeNormalDamage(hitDmg, NPCLockControl.transform.rotation.eulerAngles.y, transform.position);
                if (NPCLockControl.Inventory.hand.enabled == false)
                {
                    if (NPCLockControl.selectedWeaponScript.GetComponentInChildren<weapon>().takeDurabilityDmg())
                    {
                        NPCLockControl.animator.SetInteger("punchNum", -1);
                        NPCLockControl.isBreakWeapon(NPCLockControl.viewBodyScript.transfmoredBody.transform.rotation.eulerAngles.y);
                    }
                }
                damagedHPSys.Add(hpSystem);
            }
        }
    }
}
