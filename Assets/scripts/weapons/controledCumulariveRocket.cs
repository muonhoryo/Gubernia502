using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controledCumulariveRocket : cumulativeRocket
{
    private void OnDestroy()
    {
        bulletOwner.GetComponent<NPCLockControl>().animator.SetInteger("punchNum", 1);
        Gubernia502.mainCamera.changeToDefaultTrack();
    }
    protected override void Start()
    {
        base.Start();
        GetComponent<rocketCtrl>().enabled = false;
    }
    protected override void turnOnEngine()
    {
        base.turnOnEngine();
        GetComponent<rocketCtrl>().enabled = true;
    }
    protected override void hitNotAlife()
    {
        base.hitNotAlife();
        Gubernia502.mainCamera.changeToDefaultTrack();
    }
}
