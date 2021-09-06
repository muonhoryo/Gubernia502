using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeShoot : MonoBehaviour
{
    public ermakLockControl ermakLockControl;
    public trailRenHitBox hitBox;
    [SerializeReference]
    private Vector3 animMoveVector;
    public float AnimMoveSpeed=0;
    public int cantMoveLayers = 8960;
    public void takeSignal1()
    {
        ermakLockControl.meleeFrontHitBox.setActiveHitBox(true);
        ermakLockControl.iteractionScript.selectedWeaponScript.generateSound();
    }
    public virtual void takeSignal2()
    {
        ermakLockControl.unlockCtrl();//анлок управления
        if (hitBox != null)
        {
            hitBox.setActiveHitBox(false);
        }
        ermakLockControl.meleeFrontHitBox.setActiveHitBox(false);
        ermakLockControl.meleeLockRotation();
    }
    public void takeSignal3()
    {
        ermakLockControl.ermakAnim.SetInteger("punchNum", 0);
    }
    public void takeSignal4()
    {
        //ermakLockControl.setSeparratedAnim();
    }
    private void Update()
    {
        if (AnimMoveSpeed != 0)
        {
            animMoveVector=Gubernia502.directionFromAngle(ermakLockControl.viewBodyScript.ermakBody.transform.rotation.eulerAngles.y);
            Vector3 rayStartPos = new Vector3(ermakLockControl.transform.position.x ,
                                             0f,
                                             ermakLockControl.transform.position.z );
            float moveSpeed = AnimMoveSpeed * ermakLockControl.ermakAnim.GetCurrentAnimatorStateInfo(0).speed;
            RaycastHit hit;
            if(Physics.SphereCast(rayStartPos, ermakLockControl.moveScript.colliderRadius,
                                                      new Vector3(animMoveVector.x, 0, animMoveVector.z),out hit,
                                                      moveSpeed, cantMoveLayers, QueryTriggerInteraction.Ignore))
            {
                if (hit.distance > Gubernia502.constData.moveMinHitDistance)
                {
                    ermakLockControl.transform.Translate(animMoveVector * (hit.distance- Gubernia502.constData.moveMinHitDistance)*
                        ermakLockControl.ermakAnim.GetCurrentAnimatorStateInfo(0).speed);
                }
            }
            else
            {
                ermakLockControl.transform.Translate(animMoveVector * moveSpeed);
            }
        }
    }
}
