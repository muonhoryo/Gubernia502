using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invasiveBullet : antimaterialBullet
{
    public override void OnTriggerEnter(Collider other)
    {
        hitPointSystem hpSystem = other.GetComponent<hitPointSystem>();
        if (hpSystem != null && hpSystem.gameObject != bulletOwner)
        {
            hpSystem.takeNormalDamage(hitDmg,transform.rotation.eulerAngles.y,transform.position);
            if (hpSystem.takingDamageObjData.isAlife)
            {
                Gubernia502.spawnBullet(4, (int)(hitDmg / Gubernia502.constData.invasiveOnHitDmgPenalty), transform.position, transform.rotation.eulerAngles.y,
                                        Gubernia502.constData.invasiveOnHitDispersion, other.gameObject);
            }
            StartCoroutine(bulletIsHit(particles[0].main.startLifetimeMultiplier));
        }
    }
}
