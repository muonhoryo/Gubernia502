using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batrakMeleeShootExit : StateMachineBehaviour
{
    public batrakBehavior batrakBehavior;
    public int nextPunchNum;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (batrakBehavior.meleeFrontHitBox.coll.enabled)
        {
            batrakBehavior.meleeFrontHitBox.coll.enabled = false;
            batrakBehavior.meleeShoot.hitBox.coll.enabled = false;
        }
        if (animator.GetInteger("punchNum") != nextPunchNum)
        {
            animator.SetInteger("punchNum", 0);
            if (batrakBehavior.bodyRotateScript.enabled)
            {
                batrakBehavior.bodyRotateScript.enabled = false;
            }
        }
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        batrakBehavior = animator.GetComponent<batrakMeleeShoot>().batrakBehavior;
    }
}
