using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aprsControlShootExit : StateMachineBehaviour
{
    NPCLockControl NPCLockControl;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPCLockControl.unlockCtrl();
        NPCLockControl.unlockRotate();
        NPCLockControl.setSeparratedAnim();
        NPCLockControl.animator.SetInteger("punchNum", 0);
        NPCLockControl.weaponDispersion.gameObject.SetActive(true);
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPCLockControl = animator.GetComponent<distantShoot>().NPCLockControl;
    }
}
