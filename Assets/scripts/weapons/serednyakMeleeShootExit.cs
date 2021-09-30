using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class serednyakMeleeShootExit : meleeShootExit//singltone
{
    static serednyakMeleeShootExit singltone = null;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Gubernia502.playerController.meleeShoot.isNextAttack)
        {
            Gubernia502.playerController.meleeShoot.isNextAttack = false;
        }
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
    private void Awake()
    {
        if (singltone == null)
        {
            singltone = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
