using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batrakStanExit : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("stan", 0);
    }
}
