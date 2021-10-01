using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour//singltone
{
    static MainCamera singltone=null;
    public enum cameraMode
    {
        smoothTrack,
        stayOnTarget,
        moveTrack,
        targetTrack
    }
    public enum targetType
    {
        isObject
    }
    public targetType targetMode;
    public cameraMode cameraStartMode;
    public ermakPlayerController playerController;
    public Vector3 cursorPos;
    public GameObject trackingObj;
    public Vector3 target;
    public delegate Vector3 cameraBehaviour();
    private cameraBehaviour camBehaviour;
    private cameraBehaviour getTarget;
    private Gubernia502.simpleFun onChangeCamBehavior = delegate () { };
    [SerializeField]
    private Camera cameraComp;
    private Vector3 camOnTrackObj()
    {
        return new Vector3(target.x,
                           Gubernia502.constData.mainCameraHeight,
                           target.z);
    }//следование камеры за объектом
    private Vector3 camOnSmoothTrackObj()
    {
        float dist = Vector3.Distance(new Vector3(transform.position.x, target.y, transform.position.z), target);
        if (dist > Gubernia502.constData.mainCameraMaxMoveRadius)
        {
            return transform.position + Vector3.Normalize(target - new Vector3(transform.position.x, target.y, transform.position.z)) *
                                                        Gubernia502.constData.NPCMoveSpeed*Time.deltaTime ;
        }
        else
        {
            return transform.position + Vector3.Normalize(target - new Vector3(transform.position.x, target.y, transform.position.z)) *
                                                        Gubernia502.constData.NPCMoveSpeed*Time.deltaTime
                                                        * dist / Gubernia502.constData.mainCameraMaxMoveRadius;
        }
    }//плавное следование за объектом
    private Vector3 camMoveToTrackObj()
    {
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                            new Vector2(target.x, target.z)) > Gubernia502.constData.mainCameraMoveSpeed*Time.deltaTime)
        {
            return transform.position + Vector3.Normalize(target - new Vector3(transform.position.x, target.y, transform.position.z)) *
                Gubernia502.constData.mainCameraMoveSpeed*Time.deltaTime;
        }
        else
        {
            return new Vector3(target.x, transform.position.y, target.z);
        }
    }

    private Vector3 targetIsStatic()
    {
        return target;
    }
    private Vector3 targetIsObject()
    {
        return trackingObj.transform.position;
    }
    private Vector3 thirdToCursor()//камера находится на расстоянии одной трети от гг до курсора
    {
        return new Vector3(playerController.transform.position.x + (cursorPos.x -
            playerController.NPCLockControl.transform.position.x) / 3,
                                                Gubernia502.constData.mainCameraHeight,
                                                playerController.transform.position.z + (cursorPos.z -
            playerController.transform.position.z) / 3);
    }

    public void changeRedactorMode()
    {
        Gubernia502.playerController.enabled = false;
        onChangeCamBehavior = delegate ()
          {
              enabled = true;
              Gubernia502.playerController.enabled = true;
              onChangeCamBehavior = delegate () { };
              Destroy(GetComponent<redactorCamera>());
          };
        enabled = false;
        camBehaviour = delegate () { return new Vector3(0, Gubernia502.constData.mainCameraHeight, 0); };
        gameObject.AddComponent<redactorCamera>();
    }

    public void setTargetIsObject()
    {
        getTarget = targetIsObject;
    }

    public void changeSmoothTrackObj(targetType targetMode)
    {
        switch (targetMode)
        {
            case targetType.isObject:
                changeSmoothTrackObj(targetIsObject);
                break;
            default:
                Debug.LogError("targetMode is not instance");
                break;
        }
    }
    private void changeSmoothTrackObj(cameraBehaviour targetMode)
    {
        getTarget = targetMode;
        onChangeCamBehavior();
        camBehaviour = camOnSmoothTrackObj;
    }
    public void changeMoveToTarget(targetType targetMode)
    {
        switch (targetMode)
        {
            case targetType.isObject:
                changeMoveToTarget(targetIsObject);
                break;
            default:
                Debug.LogError("targetMode is not instance");
                break;
        }
    }
    private void changeMoveToTarget(cameraBehaviour targetMode)
    {
        getTarget = targetMode;
        onChangeCamBehavior();
        camBehaviour = camMoveToTrackObj;
    }
    public void changeToStayOnTarget(Vector3 target)
    {
        this.target = target;
        onChangeCamBehavior();
        getTarget = targetIsStatic;
        camBehaviour = delegate ()
        {
            return this.target;
        };
    }
    public void changeToTargetTracking(GameObject trackingObj)
    {
        getTarget = targetIsObject;
        onChangeCamBehavior();
        this.trackingObj = trackingObj;
        camBehaviour = camOnTrackObj;
    }
    public void changeToTargetTracking()
    {
        changeToTargetTracking(trackingObj);
    }

    public void changeToDefaultTrack()
    {
        switch (cameraStartMode)
        {
            case cameraMode.smoothTrack:
                switch (targetMode)
                {
                    case targetType.isObject:
                        changeSmoothTrackObj(targetIsObject);
                        break;
                }
                break;
            case cameraMode.stayOnTarget:
                changeToStayOnTarget(new Vector3(target.x,Gubernia502.constData.mainCameraHeight,target.z));
                break;
            case cameraMode.targetTrack:
                changeToTargetTracking();
                break;
            case cameraMode.moveTrack:
                switch (targetMode)
                {
                    case targetType.isObject:
                        changeMoveToTarget(targetIsObject);
                        break;
                }
                break;
        }
    }
    void Update()
    {
        target = getTarget();
        transform.position = camBehaviour();
    }
    private void LateUpdate()
    {
        cursorPos = cameraComp.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.y));
    }
    private void OnEnable()
    {
        if (Gubernia502.constData != null)
        {
            transform.position = new Vector3(transform.position.x, Gubernia502.constData.mainCameraHeight, transform.position.z);
        }
    }
    private void Awake()
    {
        if (singltone == null)
        {
            singltone = this;
            Gubernia502.mainCamera = this;
            changeToDefaultTrack();
        }
        else
        {
            Destroy(this);
        }
    }
}
