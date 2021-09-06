using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aprsControlShootExit : StateMachineBehaviour
{
    ermakLockControl ermakLockControl;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ermakLockControl.unlockCtrl();
        ermakLockControl.unlockRotate();
        ermakLockControl.setSeparratedAnim();
        ermakLockControl.ermakAnim.SetInteger("punchNum", 0);
        ermakLockControl.weaponDispersion.gameObject.SetActive(true);
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ermakLockControl = animator.GetComponent<distantShoot>().ermakLockControl;
    }
}
