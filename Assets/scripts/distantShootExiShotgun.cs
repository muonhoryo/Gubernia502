using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distantShootExiShotgun : StateMachineBehaviour
{
    protected NPCLockControl NPCLockControl;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("reload", false);
        Destroy(NPCLockControl.distantShoot.animatiedMagazine);
        NPCLockControl.weaponDispersion.leftLine.gameObject.SetActive(true);
        NPCLockControl.weaponDispersion.rightLine.gameObject.SetActive(true);
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPCLockControl = animator.GetComponent<distantShoot>().NPCLockControl;
        NPCLockControl.weaponDispersion.leftLine.gameObject.SetActive(false);
        NPCLockControl.weaponDispersion.rightLine.gameObject.SetActive(false);
    }
}
