using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cumulativeRocket : bullet
{
    public float explosionRadius;
    delegate void collisionAct(Collider other);
    private collisionAct action;
    private IEnumerator startRocket()
    {
        yield return null;
        GetComponent<Rigidbody>().AddForce(moveTraectory*600f , ForceMode.Impulse);
        yield break;
    }
    protected virtual void turnOnEngine()//used in animationEvent
    {
        action = onEngineCollision;
        GetComponent<cumulativeRocket>().enabled = true;
    }
    private void onEngineCollision(Collider other)
    {
            GameObject explosion = Instantiate(Gubernia502.constData.explosions[0], transform.position, transform.rotation);
            explosion.GetComponent<explosion>().dmg = hitDmg;
            Destroy(gameObject);
    }
    protected virtual void hitNotAlife()
    {
        StopAllCoroutines();
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Collider>().isTrigger = false;
        Destroy(GetComponent<Animator>());
    }
    private void offEngineCollision(Collider other)
    {
        if (other.gameObject != bulletOwner)
        {
            if (other.GetComponent<notAlifeDmgSystem>() != null)
            {
                hitNotAlife();
            }
            else if (other.GetComponent<alifeDmgSystem>() != null)
            {
                other.GetComponent<alifeDmgSystem>().takeNormalDamage(5, 0,transform.rotation.eulerAngles.y,transform.position);
            }
        }
    }
    protected override void onHit(Collider other)
    {
            action(other);
    }
    protected virtual void Start()
    {
        GetComponent<cumulativeRocket>().enabled = false;
        action = offEngineCollision;
        StartCoroutine(startRocket());
    }
}
