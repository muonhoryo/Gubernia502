using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermakViewBody : MonoBehaviour
{
    public float neededHeadDirection;

    private NPCLockControl NPCLockControl;
    public Transform transfmoredBody;
    public int foundAnimModiferResult;
    public int isMeleeWeapon = 1;
    public int sideRotation;
    public float foundHeadAngle()
    {
        float headAngle;
        if (sideRotation == 1 &&
           transfmoredBody.transform.rotation.eulerAngles.y > neededHeadDirection)
        {
            headAngle = 360f - transfmoredBody.transform.rotation.eulerAngles.y + neededHeadDirection;
        }
        else if (sideRotation == -1 &&
                transfmoredBody.transform.rotation.eulerAngles.y < neededHeadDirection)
        {
            headAngle = 22.5f-360f + neededHeadDirection - transfmoredBody.transform.rotation.eulerAngles.y;
        }
        else 
        {
            headAngle = (neededHeadDirection - transfmoredBody.transform.rotation.eulerAngles.y + 22.5f) % 360; 
        }
        if (headAngle > 45f) { headAngle = 45f; }
        else if (headAngle < 0f) { headAngle = 0f; }
        return headAngle;
}
    public void rotateHead()
    {
        float headAngle = foundHeadAngle();
        if (NPCLockControl.animator.GetFloat("HeadView") + Gubernia502.constData.NPCHeadRotationSpeed < headAngle)
        {
            NPCLockControl.animator.SetFloat("HeadView", NPCLockControl.animator.GetFloat("HeadView")
                + Gubernia502.constData.NPCHeadRotationSpeed);
            return;
        }
        else if(NPCLockControl.animator.GetFloat("HeadView") - Gubernia502.constData.NPCHeadRotationSpeed > headAngle)
        {
            NPCLockControl.animator.SetFloat("HeadView", NPCLockControl.animator.GetFloat("HeadView")
                - Gubernia502.constData.NPCHeadRotationSpeed);
            return;
        }
        NPCLockControl.animator.SetFloat("HeadView", headAngle);
    }
    public void foundAnimModifer()
    {
        if (neededHeadDirection < 22.5 ||
            neededHeadDirection > 337.5) 
        {
            foundAnimModiferResult = 0; return; 
        }
        else if (neededHeadDirection > 
            NPCLockControl.moveScript.animModifier * 45 + 22.5 + 2 * isMeleeWeapon ||
                neededHeadDirection <  
                NPCLockControl.moveScript.animModifier * 45-22.5 - 2* isMeleeWeapon)
        {
            foundAnimModiferResult = (int)(neededHeadDirection + 22.5) / 45;
        }
        //возвращает х из формулы [-π/8+π/4*x;π/8+π/4*x],определяет область взгляда
    }
    void LateUpdate()
    {
        Gubernia502.foundSideRotation(neededHeadDirection, 
                                      out sideRotation, NPCLockControl.viewBodyScript.transfmoredBody.transform.rotation.eulerAngles.y);
        foundAnimModifer();
        if (foundAnimModiferResult!= NPCLockControl.moveScript.animModifier)//проверка на смену области взгляда,потом поворот на нее
        {
            NPCLockControl.moveScript.animModifier = foundAnimModiferResult;//область взгляда
            NPCLockControl.bodyRotateScript.neededDirectionAngle = 45* NPCLockControl.moveScript.animModifier;//необходимый угол поворота туши
        }
        rotateHead();
    }
    private void Start()
    {
        NPCLockControl = GetComponent<NPCLockControl>();
    }
}
