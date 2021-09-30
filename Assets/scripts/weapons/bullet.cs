using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
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
    protected void FixedUpdate()
    {
        if (moveDistant >= Gubernia502.constData.bulletMaxMoveDistant) { Destroy(gameObject); }
        float newSpeed =Time.deltaTime * speed;
        RaycastHit hit; 
        Physics.SphereCast(transform.position, collRadius, moveTraectory, out hit, newSpeed+coll.height,
             8960, QueryTriggerInteraction.Collide);
        moveDistant += newSpeed;
        if (hit.collider!=null&&hit.collider.gameObject!=bulletOwner)
        {
            gameObject.transform.position += moveTraectory * hit.distance;
            OnTriggerEnter(hit.collider);
        }
        else
        {
            gameObject.transform.position += moveTraectory * newSpeed;
        }
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != bulletOwner && other.TryGetComponent(out hitPointSystem hpSystem))
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
