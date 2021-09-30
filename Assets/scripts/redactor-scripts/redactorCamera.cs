using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redactorCamera : MonoBehaviour//singltone
{
    static redactorCamera singltone=null;
    [SerializeField]
    [Range(0.1f,250)]
    private float redactorCameraHeight=30;
    [SerializeReference]
    private Vector2 mouseInputStart;
    [SerializeReference]
    private Vector2 mouseInputFinish;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mouseInputStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButton(1))
        {
            float speedMod = Gubernia502.constData.redactorCameraMoveSpeedMod * redactorCameraHeight * 0.1f;
            mouseInputFinish = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 moveStep = mouseInputFinish - mouseInputStart;
            transform.position = new Vector3(transform.position.x + (moveStep.x * speedMod),
                                           redactorCameraHeight,
                                           transform.position.z + (moveStep.y * speedMod));
        }
    }
    private void Awake()
    {
        if (singltone == null)
        {
            singltone = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
