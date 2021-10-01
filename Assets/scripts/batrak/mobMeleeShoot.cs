using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobMeleeShoot : MonoBehaviour
{
    public mobWeaponHitBox hitBox;
    public mobBehavior mobBehavior;
    private Vector3 animMoveVector;
    public float AnimMoveSpeed = 0;
    public int cantMoveLayers = 8960;
    [SerializeField]
    private TrailRenderer trailRenderer;
    IEnumerator disactiveEffect(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        trailRenderer.enabled = false;
        yield break;
    }
    public void disableHitBox()
    {
        if (trailRenderer.emitting)
        {
            trailRenderer.emitting = false;
            StartCoroutine(disactiveEffect(trailRenderer.time));
        }
        hitBox.enabled = false;
    }
    private void disableTrailHitBox()
    {
        trailRenderer.emitting = false;
        StartCoroutine(disactiveEffect(trailRenderer.time));
    }
    void enableTrailHitBox()
    {
        trailRenderer.emitting = true;
        StopAllCoroutines();
        trailRenderer.enabled = true;
    }
    public void takeSignal1()
    {
        if (hitBox != null)
        {
            hitBox.coll.enabled = true;
        }
        mobBehavior.meleeFrontHitBox.coll.enabled = true;
    }
    public void takeJerkSignal1()
    {
        takeSignal1();
        enableTrailHitBox();
    }
    public void takeSignal2()
    {
        if (hitBox != null)
        {
            hitBox.coll.enabled = false;
        }
        mobBehavior.meleeFrontHitBox.coll.enabled = false;
        if (mobBehavior.targetEnemy != null)
        {
            if (Vector3.Distance(mobBehavior.transform.position, mobBehavior.targetEnemy.transform.position) >
                Gubernia502.constData.mobMaxSimpleCombatDistance)
            {
                mobBehavior.currentState = mobBehavior.mobBehaviorStateMachine.BShunt;
            }
            else
            {
                mobBehavior.currentState.onStateEnter(mobBehavior);
            }
        }
    }
    public void takeJerkSignal2()
    {
        if (hitBox != null)
        {
            hitBox.coll.enabled = false;
        }
        mobBehavior.meleeFrontHitBox.coll.enabled = false;
        if (mobBehavior.targetEnemy != null)
        {
            if (Vector3.Distance(mobBehavior.transform.position, mobBehavior.targetEnemy.transform.position) >
                Gubernia502.constData.mobMaxSimpleCombatDistance)
            {
                mobBehavior.currentState = mobBehavior.mobBehaviorStateMachine.BShunt;
            }
            else
            {
                mobBehavior.currentState=mobBehavior.mobBehaviorStateMachine.BSsimpleAttack;
            }
        }
        disableTrailHitBox();
    }
    public void takeSignal3()
    {
        mobBehavior.animator.SetInteger("punchNum", 0);
        mobBehavior.bodyRotateScript.enabled = false;
    }
    public void takeSignal4()
    {
        mobBehavior.onRotateMoveDone();
    }
    public void Update()
    {
        if (AnimMoveSpeed != 0)
        {
            animMoveVector = Gubernia502.directionFromAngle(mobBehavior.bodyRotateScript.rotatedBody.transform.rotation.eulerAngles.y);
            Vector3 rayStartPos = new Vector3(mobBehavior.transform.position.x,
                                             0,
                                             mobBehavior.transform.position.z);
            float moveSpeed = AnimMoveSpeed * mobBehavior.animator.GetCurrentAnimatorStateInfo(0).speed;
            RaycastHit hit;
            if (Physics.SphereCast( rayStartPos, mobBehavior.moveScript.sphereCastRadius,
                                                      new Vector3(animMoveVector.x, 0, animMoveVector.z), out hit,
                                                      moveSpeed, cantMoveLayers, QueryTriggerInteraction.Ignore))
            {
                if (hit.distance > Gubernia502.constData.mobMoveMinHitDistance)
                {
                    mobBehavior.transform.Translate(animMoveVector * (hit.distance- Gubernia502.constData.mobMoveMinHitDistance) *
                        mobBehavior.animator.GetCurrentAnimatorStateInfo(0).speed);
                }
            }
            else
            {
                mobBehavior.transform.Translate(animMoveVector * moveSpeed);
            }
        }
    }
}
