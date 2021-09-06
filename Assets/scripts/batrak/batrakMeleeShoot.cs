using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batrakMeleeShoot : MonoBehaviour
{
    public batrakWeaponHitBox hitBox;
    public batrakBehavior batrakBehavior;
    private Vector3 animMoveVector;
    public float AnimMoveSpeed = 0;
    public int cantMoveLayers = 8960;
    public void takeSignal1()
    {
        if (hitBox != null)
        {
            hitBox.coll.enabled = true;
        }
        batrakBehavior.meleeFrontHitBox.coll.enabled = true; ;
    }
    public void takeSignal2()
    {
        if (hitBox != null)
        {
            hitBox.coll.enabled = false;
        }
        batrakBehavior.meleeFrontHitBox.coll.enabled = false;
        if (batrakBehavior.targetEnemy != null)
        {
            if (Vector3.Distance(transform.position, batrakBehavior.targetEnemy.transform.position) > 
                Gubernia502.constData.batrakMaxSimpleCombatDistance)
            {
                batrakBehavior.newAction(1);
            }
            else
            {
                batrakBehavior.newAction(2);
            }
        }
        else
        {
                batrakBehavior.newAction(3);
        }
    }
    public void takeSignal3()
    {
        batrakBehavior.batrakAnim.SetInteger("punchNum", 0);
        batrakBehavior.bodyRotateScript.enabled = false;
    }
    public void takeSignal4()
    {
        batrakBehavior.onRotateMoveDone();
    }
    public void Update()
    {
        if (AnimMoveSpeed != 0)
        {
            animMoveVector = Gubernia502.directionFromAngle(batrakBehavior.bodyRotateScript.rotatedBody.transform.rotation.eulerAngles.y);
            Vector3 rayStartPos = new Vector3(batrakBehavior.transform.position.x,
                                             0,
                                             batrakBehavior.transform.position.z);
            float moveSpeed = AnimMoveSpeed * batrakBehavior.batrakAnim.GetCurrentAnimatorStateInfo(0).speed;
            RaycastHit hit;
            if (Physics.SphereCast( rayStartPos, batrakBehavior.moveScript.sphereCastRadius,
                                                      new Vector3(animMoveVector.x, 0, animMoveVector.z), out hit,
                                                      moveSpeed, cantMoveLayers, QueryTriggerInteraction.Ignore))
            {
                if (hit.distance > Gubernia502.constData.moveMinHitDistance)
                {
                    batrakBehavior.transform.Translate(animMoveVector * (hit.distance- Gubernia502.constData.moveMinHitDistance) *
                        batrakBehavior.batrakAnim.GetCurrentAnimatorStateInfo(0).speed);
                }
            }
            else
            {
                batrakBehavior.transform.Translate(animMoveVector * moveSpeed);
            }
        }
    }
}
