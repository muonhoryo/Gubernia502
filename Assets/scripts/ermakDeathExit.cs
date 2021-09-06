using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermakDeathExit : breakItemExit
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ermakLockControl == Gubernia502.playerController.ermakLockControl)
        {
            Gubernia502.mainCamera.changeToStayOnTarget(ermakLockControl.transform.position);
        }
        Destroy(ermakLockControl.gameObject);
    }
}
