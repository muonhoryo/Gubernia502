using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public int dmg;
    IEnumerator endExplosion()
    {
        yield return null;
        Destroy(gameObject);
        yield break;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<hitPointSystem>() != null)
        {
            if (Physics.Raycast(new Vector3(transform.position.x, 0.4f, transform.position.z),
                               new Vector3(other.transform.position.x, 0.4f, other.transform.position.z), 100f, 9)) 
            {
                other.GetComponent<hitPointSystem>().takeExplosiveDamage(dmg, Gubernia502.constData.beyondExplRezist, transform.rotation.eulerAngles.y,transform.position);
            }
            else
            {
                other.GetComponent<hitPointSystem>().takeExplosiveDamage(dmg,transform.rotation.eulerAngles.y,transform.position);
            }
        }
    }
    private void Start()
    {
        StartCoroutine(endExplosion());
    }
}
