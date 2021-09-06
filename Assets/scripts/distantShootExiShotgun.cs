using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distantShootExiShotgun : StateMachineBehaviour
{
    protected ermakLockControl ermakLockControl;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("reload", false);
        Destroy(ermakLockControl.distantShoot.animatiedMagazine);
        ermakLockControl.weaponDispersion.leftLine.gameObject.SetActive(true);
        ermakLockControl.weaponDispersion.rightLine.gameObject.SetActive(true);
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ermakLockControl = animator.GetComponent<distantShoot>().ermakLockControl;
        ermakLockControl.weaponDispersion.leftLine.gameObject.SetActive(false);
        ermakLockControl.weaponDispersion.rightLine.gameObject.SetActive(false);
    }
}
