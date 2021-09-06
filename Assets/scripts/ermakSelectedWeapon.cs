using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ermakSelectedWeapon : MonoBehaviour
{
    public ermakLockControl ermakLockControl;
    public Transform parentObj;
    protected virtual void LateUpdate()
    {
        gameObject.transform.position = parentObj.position;
        gameObject.transform.rotation = parentObj.rotation;
    }
    protected virtual void Start()
    {
        parentObj = ermakLockControl.hands[0].transform;
    }
}
