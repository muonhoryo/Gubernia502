using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobDmgSystem : alifeDmgSystem
{
    mobBehavior mobBehavior;
    protected override void onTakeDmg(float hitAngle)
    {
        mobBehavior.currentState.onTakeDamage(mobBehavior,(hitAngle + 180) % 360);//передается направление получения урона
    }
    protected override void death(float rotation)
    {
        mobBehavior.currentState.onDeath(mobBehavior,(rotation + 180) % 360);
    }
    protected override void getShieldStunned(float rotation)
    {
        mobBehavior.currentState.onGetStuned(mobBehavior,(rotation + 180) % 360);
    }
    private void Start()
    {
        mobBehavior = GetComponent<mobBehavior>();
    }
    public override void targetEliminated()
    {
        mobBehavior.onEliminateTarget();
    }
}
