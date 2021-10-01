using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBodyRotateForView : bodyRotateForView
{
    [SerializeField]
    private NPCLockControl NPCLockControl;
    protected override float rotationSpeed
    {
        get => NPCLockControl.bodyRotationSpeed;
    }
    protected override void enableThisScript(bool enable = true)
    {
        enabled = enable;
    }
    protected override void Awake()
    {
    }
}
