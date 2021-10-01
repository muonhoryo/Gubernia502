using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeShoot : MonoBehaviour
{
    public NPCLockControl NPCLockControl;
    public trailRenHitBox hitBox;
    [SerializeReference]
    private Vector3 animMoveVector;
    public float AnimMoveSpeed=0;
    public int cantMoveLayers = 8960;
    public void takeSignal1()
    {
        NPCLockControl.meleeFrontHitBox.setActiveHitBox(true);
        NPCLockControl.iteractionScript.selectedWeaponScript.generateSound();
    }
    public virtual void takeSignal2()
    {
        NPCLockControl.unlockCtrl();//анлок управления
        if (hitBox != null)
        {
            hitBox.setActiveHitBox(false);
        }
        NPCLockControl.meleeFrontHitBox.setActiveHitBox(false);
        NPCLockControl.meleeLockRotation();
    }
    public void takeSignal3()
    {
        NPCLockControl.animator.SetInteger("punchNum", 0);
    }
    public void takeSignal4()
    {
        //ermakLockControl.setSeparratedAnim();
    }
    private void Update()
    {
        if (AnimMoveSpeed != 0)
        {
            animMoveVector=Gubernia502.directionFromAngle(NPCLockControl.viewBodyScript.transfmoredBody.transform.rotation.eulerAngles.y);
            Vector3 rayStartPos = new Vector3(NPCLockControl.transform.position.x ,
                                             0f,
                                             NPCLockControl.transform.position.z );
            float moveSpeed = AnimMoveSpeed * NPCLockControl.animator.GetCurrentAnimatorStateInfo(0).speed;
            RaycastHit hit;
            if(Physics.SphereCast(rayStartPos, NPCLockControl.moveScript.colliderRadius,
                                                      new Vector3(animMoveVector.x, 0, animMoveVector.z),out hit,
                                                      moveSpeed, cantMoveLayers, QueryTriggerInteraction.Ignore))
            {
                if (hit.distance > Gubernia502.constData.mobMoveMinHitDistance)
                {
                    NPCLockControl.transform.Translate(animMoveVector * (hit.distance- Gubernia502.constData.mobMoveMinHitDistance)*
                        NPCLockControl.animator.GetCurrentAnimatorStateInfo(0).speed);
                }
            }
            else
            {
                NPCLockControl.transform.Translate(animMoveVector * moveSpeed);
            }
        }
    }
}
