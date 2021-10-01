using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobStanExit : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("stan", 0);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<mobMeleeShoot>().mobBehavior.onRotateMoveDone();
    }
}
