using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batrakDmgSystem : alifeDmgSystem
{
    batrakBehavior batrakBehavior;
    protected override void onTakeDmg(float hitAngle)
    {
        batrakBehavior.currentState.onTakeDamage(batrakBehavior,(hitAngle + 180) % 360);//передается направление получения урона
    }
    protected override void death(float rotation)
    {
        batrakBehavior.currentState.onDeath(batrakBehavior,(rotation + 180) % 360);
    }
    protected override void getShieldStunned(float rotation)
    {
        batrakBehavior.currentState.onGetStuned(batrakBehavior,(rotation + 180) % 360);
    }
    private void Start()
    {
        batrakBehavior = GetComponent<batrakBehavior>();
    }
    public override void targetEliminated()
    {
        batrakBehavior.onEliminateTarget();
    }
}
