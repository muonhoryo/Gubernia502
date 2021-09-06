using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermakBodyRotateForView : bodyRotateForView
{
    [SerializeField]
    private ermakLockControl ermakLockControl;
    protected override float rotationSpeed
    {
        get => ermakLockControl.bodyRotationSpeed;
    }
    protected override void enableThisScript(bool enable = true)
    {
        enabled = enable;
    }
    protected override void Awake()
    {
    }
}
