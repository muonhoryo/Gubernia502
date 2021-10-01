using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobMeleeShootExit : StateMachineBehaviour
{
    public mobBehavior mobBehavior;
    public int nextPunchNum;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mobBehavior.meleeFrontHitBox.coll.enabled)
        {
            mobBehavior.meleeFrontHitBox.coll.enabled = false;
            mobBehavior.meleeShoot.disableHitBox();
        }
        if (animator.GetInteger("punchNum") != nextPunchNum)
        {
            animator.SetInteger("punchNum", 0);
            if (mobBehavior.bodyRotateScript.enabled)
            {
                mobBehavior.bodyRotateScript.enabled = false;
            }
        }
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mobBehavior = animator.GetComponent<mobMeleeShoot>().mobBehavior;
    }
}
