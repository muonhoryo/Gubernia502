using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobBodyRotateForView : bodyRotateForView
{
    [SerializeField]
    mobBehavior mobBehavior;
    public Gubernia502.simpleFun rotateMode;
    private Gubernia502.simpleFun onRotationDone;
    private Gubernia502.simpleFun onDisable=delegate() { };
    protected override float rotationSpeed
    {
        get => mobBehavior.currentState.rotationSpeed();
    }
    protected override void enableThisScript( bool enable = true)
    {
        if (!enable)
        {
            onRotationDone();
        }
        else
        {
            bool isHighAngle = Gubernia502.differenceOf2Angle(NeededDirectionAngle, rotatedBody.rotation.eulerAngles.y) >
                Gubernia502.constData.mobSlowedMoveRotationAngle;
            if (isHighAngle)
            {
                onRotationDone += rotationDoneOnHighAngle;
                onDisable += rotationDoneOnHighAngle;
            }
            mobBehavior.moveScript.setSpeed(isHighAngle);
        }
        enabled = enable;
    }
    protected override void Awake()
    {
        setDefaultRotate();
    }
    private void DISonTrackTarget()
    {
        mobBehavior.moveScript.setSpeed();
        rotateMode = rotateWithMove;
    }
    private void DISonlyRotate()
    {
        rotateMode = rotateWithMove;
        onRotationDone = delegate () { };
    }
    public void setTrackTarget()
    {
        OnDisable();
        enabled = true;
        rotateMode = rotateToTarget;
        onDisable += DISonTrackTarget;
    }
    public void setOnlyRotate()
    {
        onDisable();
        enabled = true;
        rotateMode = rotateWithOutMove;
        onRotationDone = rotationDone;
        onDisable += DISonlyRotate;
    }
    public void setRotateForMove()
    {
        onDisable();
        enableThisScript();
    }
    public void setDefaultRotate()
    {
        OnDisable();
        rotateMode = rotateWithMove;
        onRotationDone = delegate () { };
    }
    private void rotationDone()
    {
        mobBehavior.onRotateMoveDone();
    }
    private void rotationDoneOnHighAngle()
    {
        mobBehavior.moveScript.setSpeed();
        onRotationDone -= rotationDoneOnHighAngle;
    }
    protected void OnDisable()
    {
        onDisable();
        onDisable = delegate () { };
    }
    private void rotateWithMove()
    {
        base.Update();
        mobBehavior.moveScript.directionAngle = rotatedBody.rotation.eulerAngles.y;
    }
    private void rotateWithOutMove()
    {
        base.Update();
    }
    private void rotateToTarget()
    {
        if (mobBehavior.targetEnemy != null)
        {
            neededDirectionAngle = Gubernia502.angleFromDirection(mobBehavior.targetEnemy.transform.position - transform.position);
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
