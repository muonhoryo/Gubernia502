using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distantShootExit : distantShootExiShotgun
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPCLockControl.unlockCtrl();
        if (NPCLockControl.distantShoot.nonAnimatedMagazine != null)
        {
            NPCLockControl.distantShoot.nonAnimatedMagazine.SetActive(true);
        }
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
