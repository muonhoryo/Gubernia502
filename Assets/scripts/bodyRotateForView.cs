using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class bodyRotateForView : MonoBehaviour
{
    [SerializeField]
    protected float NeededDirectionAngle = 0f;
    public float neededDirectionAngle
    {
        get => NeededDirectionAngle;
        set
        {
            if (Gubernia502.differenceOf2Angle(value, rotatedBody.rotation.eulerAngles.y) > 0.5f)
            {
                NeededDirectionAngle = value;
                Gubernia502.foundSideRotation(value, out rotationSide, rotatedBody.rotation.eulerAngles.y);
                enableThisScript();
                minRotationAngle = value - rotationSpeed - 4f;
                maxRotationAngle = value + rotationSpeed + 4f;
            }
        }
    }
    public Transform rotatedBody;
    protected int rotationSide = 1;
    protected abstract float rotationSpeed 
    {
        get;
    }
    protected float minRotationAngle = -3f;
    protected float maxRotationAngle = 3f;
    protected abstract void enableThisScript(bool enable = true);
    protected void rotateBody()
    {
        if (rotatedBody.rotation.eulerAngles.y < minRotationAngle ||
            rotatedBody.rotation.eulerAngles.y > maxRotationAngle)
        {
            rotatedBody.Rotate(new Vector3(0f, rotationSpeed * rotationSide, 0));
        }
        else
        {
            rotatedBody.rotation = Quaternion.Euler(rotatedBody.rotation.eulerAngles.x, NeededDirectionAngle, rotatedBody.rotation.eulerAngles.z);
            enableThisScript(false);
        }
    }
    protected abstract void Awake();
    protected virtual void Update()
    {
        rotateBody();
    }
}
