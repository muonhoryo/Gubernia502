using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invasiveBullet : bullet
{
    [SerializeField]
    protected ParticleSystem[] particles;
    protected override IEnumerator bulletIsHit(float delayTime)
    {
        for (int i = 0; i < particles.Length; i++)
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
    protected override void onHit(Collider other,float hitDistance)
    {
        if (other.gameObject != bulletOwner && other.TryGetComponent(out hitPointSystem hpSystem))
        {
            gameObject.transform.position += moveTraectory * hitDistance;
            hpSystem.takeNormalDamage(hitDmg,transform.rotation.eulerAngles.y,transform.position);
            if (hpSystem.takingDamageObjData.isAlife)
            {
                Gubernia502.spawnBullet(4, (int)(hitDmg / Gubernia502.constData.invasiveOnHitDmgPenalty), transform.position, transform.rotation.eulerAngles.y,
                                        Gubernia502.constData.invasiveOnHitDispersion, other.gameObject);
            }
            StartCoroutine(bulletIsHit(particles[0].main.startLifetimeMultiplier));
        }
        else
        {
            gameObject.transform.position += moveTraectory * speed * Time.deltaTime;
        }
    }
}
