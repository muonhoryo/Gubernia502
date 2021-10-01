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
    float collRadius;
    [SerializeField]
    CapsuleCollider coll;
    [SerializeField]
    SpriteRenderer sprite;
    [SerializeField]
    TrailRenderer trail;
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
    protected void LateUpdate()
    {
        if (moveDistant >= Gubernia502.constData.bulletMaxMoveDistant) { Destroy(gameObject); }
        float newSpeed = speed*Time.deltaTime;
        RaycastHit[] hits=Physics.SphereCastAll(transform.position, collRadius, moveTraectory, newSpeed + coll.height,
             8960, QueryTriggerInteraction.Collide);
        moveDistant += newSpeed;
        if ( hits.Length>0&&hits[0].collider.gameObject!=bulletOwner)
        {
            gameObject.transform.position += moveTraectory * hits[0].distance;
            onHit(hits[0].collider);
        }
        else
        {
            gameObject.transform.position += moveTraectory * newSpeed;
        }
    }
    protected virtual void onHit(Collider other)
    {
        if ( other.gameObject != bulletOwner && other.TryGetComponent(out hitPointSystem hpSystem))
        {
            hpSystem.takeNormalDamage(hitDmg, transform.rotation.eulerAngles.y, transform.position);//передается направление полета пули
            StartCoroutine(bulletIsHit(trail.time));
        }
    }
    private void Awake()
    {
        collRadius = coll.radius * transform.localScale.y;
    }
}
