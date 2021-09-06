using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batrakBodyRotateForView : bodyRotateForView
{
    private float onAttackRotationSpeed=2f;

    [SerializeField]
    batrakBehavior batrakBehavior;
    public Gubernia502.simpleFun rotateMode;
    private Gubernia502.simpleFun onRotationDone;
    private Gubernia502.simpleFun onDisable;
    [SerializeField]
    private float RotationSpeed;
    protected override float rotationSpeed
    {
        get => RotationSpeed;
    }
    public void setPeacefulSpeed()
    {
        RotationSpeed = Gubernia502.constData.batrakPassiveRotationSpeed;
    }
    public void setAggressiveSpeed()
    {
        RotationSpeed = Gubernia502.constData.batrakActiveRotationSpeed;
    }
    public void setAttackSpeed()
    {
        RotationSpeed = onAttackRotationSpeed;
    }
    protected override void enableThisScript(bool enable = true)
    {
        bool isDesactiveMove = false;
        if (enable==false)
        {
            onRotationDone();
        }
        if (Gubernia502.differenceOf2Angle(NeededDirectionAngle, rotatedBody.rotation.eulerAngles.y) > Gubernia502.constData.batrakSlowedMoveRotationAngle)
        {
            isDesactiveMove = true;
        }
        enabled = enable;
        batrakBehavior.moveScript.setSpeed(isDesactiveMove);
    }
    protected override void Awake()
    {
        onDisable = defaultOnDisable;
        onRotationDone = delegate() { };
        rotateMode = rotateWithMove;
    }
    public void setTrackTarget()
    {
        enableThisScript();
        rotateMode = rotateToTarget;
        onRotationDone = delegate () { };
        setAttackSpeed();
        onDisable = disableOnTrackTarget;
    }
    public void setOnlyRotate()
    {
        rotateMode = rotateWithOutMove;
        onRotationDone = rotationDone;
    }
    public void setRotateForMove()
    {
        rotateMode = rotateWithMove;
        onRotationDone = delegate() { };
    }
    private void rotationDone()
    {
        batrakBehavior.onRotateMoveDone();
    }
    private void defaultOnDisable()
    {
        batrakBehavior.moveScript.setSpeed();
        setRotateForMove();
    }
    private void disableOnTrackTarget()
    {
        defaultOnDisable();
        setAggressiveSpeed();
        onDisable = defaultOnDisable;
    }
    protected void OnDisable()
    {
        onDisable();
    }
    private void rotateWithMove()
    {
        base.Update();
        batrakBehavior.moveScript.directionAngle = rotatedBody.rotation.eulerAngles.y;
    }
    private void rotateWithOutMove()
    {
        base.Update();
    }
    private void rotateToTarget()
    {
        if (batrakBehavior.targetEnemy != null)
        {
            neededDirectionAngle = Gubernia502.angleFromDirection(batrakBehavior.targetEnemy.transform.position - transform.position);
            if (rotatedBody.rotation.eulerAngles.y < minRotationAngle ||
                rotatedBody.rotation.eulerAngles.y > maxRotationAngle)
            {
                rotatedBody.Rotate(new Vector3(0f, rotationSpeed * rotationSide, 0));
            }
            else
            {
                rotatedBody.rotation = Quaternion.Euler(rotatedBody.rotation.eulerAngles.x, NeededDirectionAngle, rotatedBody.rotation.eulerAngles.z);
            }
        }
        else
        {
            OnDisable();
        }
    }
    
    protected override void Update()
    {
        rotateMode();
    }
}
