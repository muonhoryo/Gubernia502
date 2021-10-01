using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeHitBox : MonoBehaviour
{
    public NPCLockControl NPCLockControl;
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
        if ( other.gameObject!=NPCLockControl.gameObject) 
        {
            if (other.TryGetComponent(out hitPointSystem hpSystem)&&!NPCLockControl.meleeShoot.hitBox.damagedHPSys.Contains(hpSystem))
            {
                hpSystem.takeNormalDamage(hitDmg, NPCLockControl.transform.rotation.eulerAngles.y, NPCLockControl.transform.position);
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
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out hitPointSystem hpSystem)&&damagedHPSys.Contains(hpSystem))
        {
            damagedHPSys.Remove(hpSystem);
        }
    }
}
