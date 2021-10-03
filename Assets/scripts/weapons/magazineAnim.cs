using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magazineAnim : NPCSelectedWeapon
{
    public int hand = 0;
    protected override void LateUpdate()
    {
        gameObject.transform.position = parentObj.position;
        gameObject.transform.rotation = parentObj.rotation;
    }
    protected override void Start()
    {
        parentObj = NPCLockControl.hands[hand].transform;
    }
    public void signal(int num)
    {

        Debug.Log(num);
    }
}
