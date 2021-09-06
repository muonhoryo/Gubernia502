using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temporalEffect : MonoBehaviour
{
    [SerializeField]
    private float delayTime;
    IEnumerator destroyOnNextFrame()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
        yield break;
    }
    IEnumerator destroyOnDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
        yield break;
    }
    public enum destroyMode
    {
        nextFrame=0,
        delayTime=1
    };
    public destroyMode mode;
    private void Start()
    {
        switch ((int)mode)
        {
            case 0:
                StartCoroutine(destroyOnNextFrame());
                break;
            case 1:
                StartCoroutine(destroyOnDelay(delayTime));
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
