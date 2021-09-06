using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermakColliderMove : MonoBehaviour
{
    [SerializeField]
    private ermakLockControl ermakLockControl;
    [SerializeField]
    private Vector3 colliderOffset;
    public CapsuleCollider colliderObj;
    public Transform colliderPos;
    void Update()
    {
        colliderObj.center = colliderPos.position-transform.position+colliderOffset;
        /*colliderObj.center = new Vector3(colliderPos.localPosition.x + Gubernia502.xOffset(colliderOffset,
                                                                   ermakLockControl.viewBodyScript.ermakBody.rotation.eulerAngles.y),
                                       colliderPos.localPosition.y,
                                       colliderPos.localPosition.z + Gubernia502.zOffset(colliderOffset,
                                                                   ermakLockControl.viewBodyScript.ermakBody.rotation.eulerAngles.y));*/
    }
}
