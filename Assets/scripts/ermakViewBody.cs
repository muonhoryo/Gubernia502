using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermakViewBody : MonoBehaviour
{
    public float neededHeadDirection;

    private ermakLockControl ermakLockControl;
    public Transform ermakBody;
    public int foundAnimModiferResult;
    public int isMeleeWeapon = 1;
    public int sideRotation;
    public float foundHeadAngle()
    {
        float headAngle;
        if (sideRotation == 1 &&
           ermakBody.transform.rotation.eulerAngles.y > neededHeadDirection)
        {
            headAngle = 360f - ermakBody.transform.rotation.eulerAngles.y + neededHeadDirection;
        }
        else if (sideRotation == -1 &&
                ermakBody.transform.rotation.eulerAngles.y < neededHeadDirection)
        {
            headAngle = 22.5f-360f + neededHeadDirection - ermakBody.transform.rotation.eulerAngles.y;
        }
        else 
        {
            headAngle = (neededHeadDirection - ermakBody.transform.rotation.eulerAngles.y + 22.5f) % 360; 
        }
        if (headAngle > 45f) { headAngle = 45f; }
        else if (headAngle < 0f) { headAngle = 0f; }
        return headAngle;
}
    public void rotateHead()
    {
        float headAngle = foundHeadAngle();
        if (ermakLockControl.ermakAnim.GetFloat("HeadView") + Gubernia502.constData.ermakHeadRotationSpeed < headAngle)
        {
            ermakLockControl.ermakAnim.SetFloat("HeadView", ermakLockControl.ermakAnim.GetFloat("HeadView")
                + Gubernia502.constData.ermakHeadRotationSpeed);
            return;
        }
        else if(ermakLockControl.ermakAnim.GetFloat("HeadView") - Gubernia502.constData.ermakHeadRotationSpeed > headAngle)
        {
            ermakLockControl.ermakAnim.SetFloat("HeadView", ermakLockControl.ermakAnim.GetFloat("HeadView")
                - Gubernia502.constData.ermakHeadRotationSpeed);
            return;
        }
        ermakLockControl.ermakAnim.SetFloat("HeadView", headAngle);
    }
    public void foundAnimModifer()
    {
        if (neededHeadDirection < 22.5 ||
            neededHeadDirection > 337.5) 
        {
            foundAnimModiferResult = 0; return; 
        }
        else if (neededHeadDirection > 
            ermakLockControl.moveScript.animModifier * 45 + 22.5 + 2 * isMeleeWeapon ||
                neededHeadDirection <  
                ermakLockControl.moveScript.animModifier * 45-22.5 - 2* isMeleeWeapon)
        {
            foundAnimModiferResult = (int)(neededHeadDirection + 22.5) / 45;
        }
        //возвращает х из формулы [-π/8+π/4*x;π/8+π/4*x],определяет область взгляда
    }
    void LateUpdate()
    {
        Gubernia502.foundSideRotation(neededHeadDirection, 
                                      out sideRotation, ermakLockControl.viewBodyScript.ermakBody.transform.rotation.eulerAngles.y);
        foundAnimModifer();
        if (foundAnimModiferResult!= ermakLockControl.moveScript.animModifier)//проверка на смену области взгляда,потом поворот на нее
        {
            ermakLockControl.moveScript.animModifier = foundAnimModiferResult;//область взгляда
            ermakLockControl.bodyRotateScript.neededDirectionAngle = 45* ermakLockControl.moveScript.animModifier;//необходимый угол поворота туши
        }
        rotateHead();
    }
    private void Start()
    {
        ermakLockControl = GetComponent<ermakLockControl>();
    }
}
