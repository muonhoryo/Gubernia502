using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermakWeaponDispersion : MonoBehaviour
{
    [SerializeField]
    private ermakLockControl ermakLockControl;
    public GameObject leftLine;
    public GameObject rightLine;
    public float dispersion = 1;
    public float rotateAngle=0;
    private void LateUpdate()
    {
        rotateAngle = ermakLockControl.viewBodyScript.ermakBody.rotation.eulerAngles.y + ermakLockControl.ermakAnim.GetFloat("HeadView") - 22.5f;
        gameObject.transform.rotation = Quaternion.Euler(0f,
                                                    rotateAngle,
                                                    0f);
        if (ermakLockControl.iteractionScript.selectedWeaponScript.gameObject != null)
        {
            gameObject.transform.position = ermakLockControl.iteractionScript.selectedWeaponScript.bulletStart.transform.position ;
        }
        else
        {
            gameObject.transform.localPosition=Vector3.zero;
        }
    }
    public void setDispersion(float newDispersion)
    {
        if (newDispersion < 1)
        {
            newDispersion = 1;
        }
        leftLine.transform.localRotation = Quaternion.Euler(0f, -newDispersion / 2, 0f);
        rightLine.transform.localRotation = Quaternion.Euler(0f, newDispersion / 2, 0f);
        dispersion = newDispersion;
    }
}
