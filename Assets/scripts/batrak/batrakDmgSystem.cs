using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batrakDmgSystem : alifeDmgSystem
{
    batrakBehavior batrakBehavior;
    protected override void onTakeDmg(float hitAngle)
    {
        batrakBehavior.onTakeDamage((hitAngle + 180) % 360);//передается направление получения урона
    }
    protected override void death(float rotation)
    {
        batrakBehavior.onDeath((rotation + 180) % 360);
    }
    protected override void getSimpleStunned(float rotation)
    {
        batrakBehavior.onGetStuned((rotation + 180) % 360);
    }
    protected override void getStunned(float rotation, int stunType)
    {
        base.getStunned(rotation, stunType);
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
