using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDmgSystem : alifeDmgSystem
{
    [SerializeField]
    protected targetStatsCollector statsCollector;
    [SerializeField]
    protected NPCLockControl NPCLockControl;
    protected override void getStunned(float rotation, int stunType)
    {
        base.getStunned(rotation, stunType);
        NPCLockControl.weaponDispersion.gameObject.SetActive(false);
    }
    protected override void death(float rotation)
    {
        base.death(rotation);
        NPCLockControl.soundGenerator.disableSoundGen();
    }
    public override void becameTarget(hitPointSystem hunter)
    {
        if (hunters.Count == 0)
        {
            statsCollector.enabled = true;
        }
        base.becameTarget(hunter);
    }
    public override void huntEnd(hitPointSystem hunter)
    {
        base.huntEnd(hunter);
        if (hunters.Count == 0)
        {
            statsCollector.enabled = false;
        }
    }
}
