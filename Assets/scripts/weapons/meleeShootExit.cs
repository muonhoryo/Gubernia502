using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeShootExit : StateMachineBehaviour
{
    public ermakLockControl ermakLockControl;
    public int nextPunchNum;
    public int currentPunchNum;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ermakLockControl.meleeShoot.enabled = false;
        if (ermakLockControl.meleeShoot.hitBox!=null&& ermakLockControl.meleeShoot.hitBox.enabled)
        {
            ermakLockControl.meleeShoot.hitBox.setActiveHitBox(false);
        }
        ermakLockControl.meleeFrontHitBox.setActiveHitBox(false);
        Gubernia502.mainCamera.changeToDefaultTrack();
        if (animator.GetInteger("stan") != 0)
        {
            animator.SetInteger("punchNum", 0);
            return;
        }
        int punchNum = animator.GetInteger("punchNum");
        if (punchNum != nextPunchNum&&stateInfo.shortNameHash!=animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
        {
            ermakLockControl.setSeparratedAnim();
            if (!Gubernia502.debugConsole.enabled)
            {
                ermakLockControl.unlockRotate();
                ermakLockControl.unlockCtrl();
            }
            ermakLockControl.bodyRotationSpeed = Gubernia502.constData.ermakBodyRotationSpeed;
            animator.SetInteger("punchNum", 0);
        }
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Gubernia502.mainCamera.changeMoveToTarget(MainCamera.targetType.isObject);
        ermakLockControl = animator.GetComponent<meleeShoot>().ermakLockControl;
        ermakLockControl.meleeShoot.enabled = true;
        ermakLockControl.meleeShoot.hitBox.setActiveHitBox(true);
    }
}