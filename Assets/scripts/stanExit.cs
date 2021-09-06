using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stanExit : breakItemExit
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        if (ermakLockControl.ermakInventory.selectedWeaponIndex >0)
        {
            ermakLockControl.weaponDispersion.gameObject.SetActive(ermakLockControl.ermakInventory.EquippedWeapons.weaponsItem.isDistant);
        }
    }
}
