using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    protected bool isHit = false;
    public int hitDmg;
    public float speed;
    public Vector3 moveTraectory;
    protected float moveDistant = 0f;
    public GameObject bulletOwner;
    protected float collRadius;
    [SerializeField]
    protected CapsuleCollider coll;
    [SerializeField]
    protected SpriteRenderer sprite;
    [SerializeField]
    protected TrailRenderer trail;
    protected virtual IEnumerator bulletIsHit(float delayTime)
    {
        enabled = false;
        Destroy(coll);
        Destroy(sprite);
        trail.emitting = false;
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
        yield break;
    }
    protected virtual void LateUpdate()
    {
        if (moveDistant >= Gubernia502.constData.bulletMaxMoveDistant) { Destroy(gameObject); }
        float newSpeed = speed*Time.deltaTime;
        RaycastHit[] hits=Physics.SphereCastAll(transform.position, collRadius, moveTraectory, newSpeed + coll.height,
             8960, QueryTriggerInteraction.Ignore);
        moveDistant += newSpeed;
        if ( hits.Length>0&&hits[0].collider.gameObject!=bulletOwner)
        {
            onHit(hits[0].collider,hits[0].distance);
        }
        else
        {
            gameObject.transform.position += moveTraectory * newSpeed;
        }
    }
    protected virtual void onHit(Collider other,float hitDistance)
    {
        if ( other.gameObject != bulletOwner && other.TryGetComponent(out hitPointSystem hpSystem))
        {
            gameObject.transform.position += moveTraectory * hitDistance;
            hpSystem.takeNormalDamage(hitDmg, transform.rotation.eulerAngles.y, transform.position);//передается направление полета пули
            StartCoroutine(bulletIsHit(trail.time));
        }
        else
        {
            gameObject.transform.position += moveTraectory * speed*Time.deltaTime;
        }
    }
    private void Awake()
    {
        collRadius = coll.radius * transform.localScale.y;
    }
}
