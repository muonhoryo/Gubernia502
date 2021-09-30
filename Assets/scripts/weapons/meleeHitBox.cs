using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeHitBox : MonoBehaviour
{
    public ermakLockControl ermakLockControl;
    public int hitDmg=1;
    [SerializeField]
    private Collider hitBox;
    public List<hitPointSystem> damagedHPSys = new List<hitPointSystem> { };
    public virtual void setActiveHitBox(bool isActive)
    {
        hitBox.enabled = isActive;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject!=ermakLockControl.gameObject) 
        {
            if (other.TryGetComponent(out hitPointSystem hpSystem)&&!ermakLockControl.meleeShoot.hitBox.damagedHPSys.Contains(hpSystem))
            {
                hpSystem.takeNormalDamage(hitDmg, ermakLockControl.transform.rotation.eulerAngles.y, ermakLockControl.transform.position);
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
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out hitPointSystem hpSystem)&&damagedHPSys.Contains(hpSystem))
        {
            damagedHPSys.Remove(hpSystem);
        }
    }
}
