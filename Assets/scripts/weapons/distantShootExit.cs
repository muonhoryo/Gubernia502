using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distantShootExit : distantShootExiShotgun
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ermakLockControl.unlockCtrl();
        if (ermakLockControl.distantShoot.nonAnimatedMagazine != null)
        {
            ermakLockControl.distantShoot.nonAnimatedMagazine.SetActive(true);
        }
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
