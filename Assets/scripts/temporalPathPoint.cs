using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temporalPathPoint : MonoBehaviour
{
    public Vector3 position { get; protected set; }
    public List<string>test=new List<string> { };
    public List<Gubernia502.oneSidePathWay> ways=new List<Gubernia502.oneSidePathWay> { };
    protected virtual void OnDestroy()
    {
        while (ways.Count > 0)
        {
            ways[0].destroyWay();
        }
    }
    protected virtual void OnEnable()
    {
        position = gameObject.transform.position;
    }
    /*protected void Update()
    {
        test = new List<string> { };
        if (ways.Count > 0)
        {
            for (int i = 0; i < ways.Count; i++)
            {
                test.Add(ways[i].endPoint.gameObject.name);
            }
        }
    }*/
}
