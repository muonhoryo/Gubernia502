using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antimaterialBullet : bullet
{
    [SerializeField]
    protected ParticleSystem[] particles;
    protected override IEnumerator bulletIsHit(float delayTime)
    {
        for(int i = 0; i < particles.Length; i++)
        { 
            ParticleSystem currentParticle = particles[i];
            particles[i].Emit(currentParticle.transform.position,
                              Gubernia502.directionFromAngle(currentParticle.transform.eulerAngles.y),
                              currentParticle.main.startSizeMultiplier,
                              currentParticle.main.startLifetimeMultiplier,
                              currentParticle.main.startColor.colorMin);
            particles[i].Stop();
        }
        return base.bulletIsHit(delayTime);
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != bulletOwner && other.TryGetComponent(out hitPointSystem hpSystem))
        {
            if (!hpSystem.takingDamageObjData.isAlife)
            {
                StartCoroutine(bulletIsHit(particles[0].main.startLifetimeMultiplier));
                hpSystem.takeNormalDamage(hitDmg, transform.rotation.eulerAngles.y, transform.position);
            }
            else
            {
                GetComponent<alifeDmgSystem>().getNormalStunDmg(hitDmg, transform.rotation.eulerAngles.y, transform.position);
            }
        }
    }
}
