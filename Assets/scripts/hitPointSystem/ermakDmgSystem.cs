using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermakDmgSystem : alifeDmgSystem
{
    [SerializeField]
    ermakLockControl ermakLockControl;
    protected override void getStunned(float rotation, int stunType)
    {
        base.getStunned(rotation, stunType);
        ermakLockControl.weaponDispersion.gameObject.SetActive(false);
    }
    protected override void death(float rotation)
    {
        base.death(rotation);
        ermakLockControl.weaponDispersion.gameObject.SetActive(false);
        Gubernia502.mainCamera.changeToTargetTracking(gameObject);
        ermakLockControl.soundGenerator.disableSoundGen();
    }
}
