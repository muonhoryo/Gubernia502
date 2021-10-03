using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NPCSelectedWeapon : MonoBehaviour
{
    public NPCLockControl NPCLockControl;
    public Transform parentObj;
    protected virtual void LateUpdate()
    {
        gameObject.transform.position = parentObj.position;
        gameObject.transform.rotation = parentObj.rotation;
    }
    protected virtual void Start()
    {
        parentObj = NPCLockControl.hands[0].transform;
    }
}
