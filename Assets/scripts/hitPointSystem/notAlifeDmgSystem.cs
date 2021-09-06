using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notAlifeDmgSystem : hitPointSystem
{
    [SerializeReference]
    protected bool isDead = false;
    [SerializeField]
    protected pathPoint[] pathPoints;
    private void takeDamage(int dmg, float bulletAngle, Vector3 hitPos)
    {
        hitPoint -= dmg;
        if (hitPoint <= 0&&!isDead)
        {
            death(bulletAngle);
        }
        else
        {
            spawnReflectedEffectOnCube(bulletAngle, hitPos, takingDamageObjData.onHitEffect[0]);
            spawnPerpendicularEffectOnCube(bulletAngle, hitPos, takingDamageObjData.onHitEffect[1]);
        }
    }
    protected override void death(float bulletAngle)
    {
        base.death(bulletAngle);
        Instantiate(takingDamageObjData.onDeathEffect, transform.position, Quaternion.Euler(transform.eulerAngles.x,bulletAngle,transform.eulerAngles.z));
        Destroy(gameObject);
        isDead = true;
    }
    public override void takeNormalDamage(int dmg,int rezist,float bulletAngle, Vector3 hitPos)
    {
        takeDamage((int)Mathf.Sqrt(10 * dmg / (rezist + 1)) + 1, bulletAngle, hitPos);
    }
    public override void takeNormalDamage(int dmg,float bulletAngle, Vector3 hitPos)
    {
        takeNormalDamage(dmg, takingDamageObjData.normalRezist, bulletAngle, hitPos);
    }
    public override void takeExplosiveDamage(int dmg, int rezist, float bulletAngle, Vector3 hitPos)
    {
        takeDamage((int)Mathf.Pow(dmg, (float)(1.3 - 0.009 * rezist)) + 1, bulletAngle, hitPos);
    }
    public override void takeExplosiveDamage(int dmg, float bulletAngle, Vector3 hitPos)
    {
        takeExplosiveDamage(dmg, takingDamageObjData.exploziveRezist, bulletAngle, hitPos);
    }
}
