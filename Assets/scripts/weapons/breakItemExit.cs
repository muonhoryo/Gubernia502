using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakItemExit : StateMachineBehaviour
{
    public ermakLockControl ermakLockControl;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ermakLockControl.meleeShoot.hitBox = null;
        ermakLockControl.setSeparratedAnim();
        ermakLockControl.unlockCtrl();
        ermakLockControl.unlockRotate();
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ermakLockControl = animator.GetComponent<meleeShoot>().ermakLockControl;
        ermakLockControl.ermakAnim.SetInteger("stan", 0);
        ermakLockControl.setFullAnim();
        ermakLockControl.lockCtrl();
    }
}
