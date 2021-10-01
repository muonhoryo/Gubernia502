using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stanExit : breakItemExit
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        if (NPCLockControl.Inventory.selectedWeaponIndex >0)
        {
            NPCLockControl.weaponDispersion.gameObject.SetActive(NPCLockControl.Inventory.EquippedWeapons.weaponsItem.isDistant);
        }
    }
}
