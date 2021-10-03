using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antimaterialBullet : bullet
{
    [SerializeField]
    protected ParticleSystem[] particles;
    List<hitPointSystem> dmgSystem = new List<hitPointSystem> { };
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
    protected override void onHit(Collider other,float hitDistance)
    {
        if (other.gameObject != bulletOwner && other.TryGetComponent(out hitPointSystem hpSystem))
        {
            if (!hpSystem.takingDamageObjData.isAlife)
            {
                gameObject.transform.position += moveTraectory * hitDistance;
                StartCoroutine(bulletIsHit(particles[0].main.startLifetimeMultiplier));
                hpSystem.takeNormalDamage(hitDmg, transform.rotation.eulerAngles.y, transform.position);
            }
            else if(!dmgSystem.Contains(hpSystem))
            {
                gameObject.transform.position += moveTraectory * speed*Time.deltaTime;
                other.GetComponent<alifeDmgSystem>().getNormalStunDmg(hitDmg, transform.rotation.eulerAngles.y, transform.position);
                dmgSystem.Add(hpSystem);
            }
            else
            {
                gameObject.transform.position += moveTraectory * speed * Time.deltaTime;
            }
        }
        else
        {
            gameObject.transform.position += moveTraectory * speed * Time.deltaTime;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out hitPointSystem hpSystem) && dmgSystem.Contains(hpSystem))
        {
            dmgSystem.Remove(hpSystem);
        }
    }
}
