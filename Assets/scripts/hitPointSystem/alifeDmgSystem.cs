using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alifeDmgSystem : hitPointSystem
{
    [SerializeField]
    protected ParticleSystem destroyedShieldEffect;
    public bool isDead { get; private set; } = false;
    public Gubernia502.fraction Fraction;
    public regenData regenData;
    private int regenCoolDown;
    public Animator anim;
    IEnumerator regenDelay()
    {
        yield return new WaitForSeconds(regenData.regenDelay);
        enabled = true;
        regenCoolDown = 0;
        yield break;
    }
    protected override void death(float rotation)
    {
        base.death(rotation);
        StopAllCoroutines();
        getStunned(rotation, 3);
    }
    protected virtual void getSimpleStunned(float rotation)
    {
        getStunned(rotation, 2);
    }
    protected virtual void getStunned(float rotation, int stunType)
    {
        anim.SetInteger("stan", stunType);
        anim.transform.rotation = Quaternion.Euler(0f, rotation, 0f);
    }
    protected virtual void onTakeDmg(float hitAngle) { }
    protected virtual void takeDamage(int dmgResult,float hitAngle, Vector3 hitPos)
    {
            enabled = false;
        StopAllCoroutines();
           StartCoroutine(regenDelay());
            if (shieldDurability > 0)
            {
                if (dmgResult >= shieldDurability)
                {
                    shieldDurability = 0;
                    getSimpleStunned(hitAngle);
                    destroyedShieldEffect.Play();
                }
                else
                {
                    onTakeDmg(hitAngle);
                    shieldDurability -= dmgResult;
                    anim.SetTrigger("takeDmg");
                    spawnReflectedEffectOnCapsule(hitAngle, (hitPos + transform.position) / 2, takingDamageObjData.onHitEffect[0]);
                }
            }
            else
            {
                hitPoint -= dmgResult;
                if (hitPoint <= 0 && !isDead)
                {
                    death(hitAngle);
                Instantiate(takingDamageObjData.onDeathEffect, transform.position, Quaternion.Euler(transform.eulerAngles.x, (hitAngle+180)%360, transform.eulerAngles.z));
                    isDead = true;
                }
            }
    }
    public override void takeExplosiveDamage(int dmg,int rezist, float hitAngle, Vector3 hitPos)
    {
        takeDamage((int)Mathf.Pow(dmg, (float)((4.9 - 0.02 * rezist) / 5)) + 1,hitAngle,hitPos);
    }
    public override void takeNormalDamage(int dmg, int rezist, float hitAngle, Vector3 hitPos)
    {
        takeDamage((int)Mathf.Pow(dmg, (float)(2 / (2 + (float)rezist * 0.05))) + 1, hitAngle,hitPos);
    }
    public override void takeExplosiveDamage(int dmg, float hitAngle, Vector3 hitPos)
    {
        takeExplosiveDamage(dmg, takingDamageObjData.exploziveRezist,hitAngle,hitPos);
    }
    public override void takeNormalDamage(int dmg, float hitAngle, Vector3 hitPos)
    {
        takeExplosiveDamage(dmg, takingDamageObjData.normalRezist,hitAngle,hitPos);
    }
    private void FixedUpdate()
    {
        if (regenCoolDown >= regenData.regenCoolDownTime)
        {
            if (shieldDurability < takingDamageObjData.maxShieldDuability)
            {
                shieldDurability += 1;
                regenCoolDown = 0;
                return;
            }
            else
            {
                enabled=false;
            }
        }
        regenCoolDown += 1;
    }
}
