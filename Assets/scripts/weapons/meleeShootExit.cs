using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeShootExit : StateMachineBehaviour
{
    public NPCLockControl NPCLockControl;
    public int nextPunchNum;
    public int currentPunchNum;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPCLockControl.meleeShoot.enabled = false;
        if (NPCLockControl.meleeShoot.hitBox!=null&& NPCLockControl.meleeShoot.hitBox.enabled)
        {
            NPCLockControl.meleeShoot.hitBox.setActiveHitBox(false);
        }
        NPCLockControl.meleeFrontHitBox.setActiveHitBox(false);
        Gubernia502.mainCamera.changeToDefaultTrack();
        if (animator.GetInteger("stan") != 0)
        {
            animator.SetInteger("punchNum", 0);
            return;
        }
        int punchNum = animator.GetInteger("punchNum");
        if (punchNum != nextPunchNum&&stateInfo.shortNameHash!=animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
        {
            NPCLockControl.setSeparratedAnim();
            if (!Gubernia502.debugConsole.enabled)
            {
                NPCLockControl.unlockRotate();
                NPCLockControl.unlockCtrl();
            }
            NPCLockControl.bodyRotationSpeed = Gubernia502.constData.NPCBodyRotationSpeed;
            animator.SetInteger("punchNum", 0);
        }
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Gubernia502.mainCamera.changeMoveToTarget(MainCamera.targetType.isObject);
        NPCLockControl = animator.GetComponent<meleeShoot>().NPCLockControl;
        NPCLockControl.meleeShoot.enabled = true;
        NPCLockControl.meleeShoot.hitBox.setActiveHitBox(true);
    }
}