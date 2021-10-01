using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakItemExit : StateMachineBehaviour
{
    public NPCLockControl NPCLockControl;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPCLockControl.meleeShoot.hitBox = null;
        NPCLockControl.setSeparratedAnim();
        NPCLockControl.unlockCtrl();
        NPCLockControl.unlockRotate();
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPCLockControl = animator.GetComponent<meleeShoot>().NPCLockControl;
        NPCLockControl.animator.SetInteger("stan", 0);
        NPCLockControl.setFullAnim();
        NPCLockControl.lockCtrl();
    }
}
