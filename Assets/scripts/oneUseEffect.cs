using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oneUseEffect : MonoBehaviour
{
    public float lifeTime=1;
    IEnumerator deathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        yield break;
    }
    private void Start()
    {
        StartCoroutine(deathDelay());
    }
}
