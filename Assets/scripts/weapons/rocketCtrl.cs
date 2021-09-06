using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketCtrl : MonoBehaviour
{
    controledCumulariveRocket rocketEng;
    [SerializeField]
    int sideRotation;
    [SerializeField]
    float neededRotation;
    [SerializeField]
    Vector3 cursorPos;
    private float rotationSpeed;
    private void LateUpdate()
    {
        cursorPos = Gubernia502.mainCamera.cursorPos;
        if (cursorPos.z == transform.position.z)
        {
            if(cursorPos.z < transform.position.z)
            {
                neededRotation = 270;
            }
            else
            {
                neededRotation = 90;
            }
        }
        else
        {
            neededRotation = 180 * Mathf.Atan((cursorPos.x - transform.position.x) / (cursorPos.z - transform.position.z)) / Mathf.PI;
        }
        if (cursorPos.z < transform.position.z)
        {
            neededRotation += 180;
        }
        if (neededRotation < 0)
        {
            neededRotation += 360;
        }
        if (transform.rotation.eulerAngles.y>neededRotation+1||
           transform.rotation.eulerAngles.y < neededRotation - 1)
        {
            Gubernia502.foundSideRotation(neededRotation, out sideRotation,transform.rotation.eulerAngles.y);
        }
        if (neededRotation + rotationSpeed+1f < transform.rotation.eulerAngles.y ||
           neededRotation - rotationSpeed+1f > transform.rotation.eulerAngles.y)
        {
            transform.Rotate(new Vector3(0f, rotationSpeed * sideRotation, 0f));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, neededRotation,0f );
        }
        rocketEng.moveTraectory = new Vector3(Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.PI / 180),
                                            0f,
                                            Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.PI / 180));
    }
    private void Start()
    {
        rocketEng = GetComponent<controledCumulariveRocket>();
        rotationSpeed = Gubernia502.constData.ctrlRocketRotationSpeed/rocketEng.speed;
    }
}
