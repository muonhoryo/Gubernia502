using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDeathExit : breakItemExit
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (NPCLockControl == Gubernia502.playerController.NPCLockControl)
        {
            Gubernia502.mainCamera.changeToStayOnTarget(NPCLockControl.transform.position);
        }
        Destroy(NPCLockControl.gameObject);
        saveSystem.loadMainMenu();
    }
}
