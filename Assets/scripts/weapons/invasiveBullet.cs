using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invasiveBullet : antimaterialBullet
{
    protected override void onHit(Collider other)
    {
        if (other.gameObject != bulletOwner && other.TryGetComponent(out hitPointSystem hpSystem))
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
