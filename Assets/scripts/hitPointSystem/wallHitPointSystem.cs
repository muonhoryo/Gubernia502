using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallHitPointSystem : notAlifeDmgSystem
{
    ~wallHitPointSystem()
    {
        Gubernia502.walls.Remove(this);
    }
    [SerializeField]
    public int phase { get; private set; } = 1;
    [SerializeField]
    GameObject deletingObj;
    [SerializeField]
    takingDamageObjData secondPhaseDataStats;
    [SerializeField]
    Vector3 newColliderSize;
    [SerializeField]
    Vector3 newColliderOffset;
    [SerializeField]
    BoxCollider hitBox;
    [SerializeReference]
    wallHitPointSystem[] nearWalls = new wallHitPointSystem[8]
    {
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null
    };
    protected override void death(float bulletAngle)
    {
        if (phase == 1)
        {
            Instantiate(takingDamageObjData.onDeathEffect, deletingObj.transform.position, Quaternion.Euler(transform.eulerAngles.x, bulletAngle, transform.eulerAngles.z));
            Destroy(deletingObj);
            takingDamageObjData = secondPhaseDataStats;
            hitBox.size = newColliderSize;
            hitBox.center = newColliderOffset;
            hitPoint = takingDamageObjData.maxHitPoint;
            shieldDurability = takingDamageObjData.maxShieldDuability;
            phase = 2;
        }
        else
        {
            base.death(bulletAngle);
            OnDestroy();
        }
    }
    private void OnDestroy()
    {
        Gubernia502.walls.Remove(this);
        if (!saveSystem.loadStatus)
        {
            for (int i = 0; i < 8; i++)
            {
                if (nearWalls[i] != null)
                {
                    nearWalls[i].nearWallDestroyed((i * 45 + 180) % 360);
                }
            }
        }
    }
    public void nearPlacedNewWall(float direction, wallHitPointSystem newNearWall)
    {
        int nearWallNum = (int)direction / 45;
        nearWalls[nearWallNum] = newNearWall;
        int disabledPoint;
        if ((nearWallNum & 1) > 0)
        {
            disabledPoint = nearWallNum / 2;
            if (pathPoints[disabledPoint].gameObject.activeSelf)
            {
                pathPoints[disabledPoint].gameObject.SetActive(false);
            }
        }
        else
        {
            disabledPoint = (nearWallNum + 7) % 8 / 2;
            if (pathPoints[disabledPoint].gameObject.activeSelf)
            {
                pathPoints[disabledPoint].gameObject.SetActive(false);
            }
            disabledPoint = (disabledPoint + 1) % 4;
            if (pathPoints[disabledPoint].gameObject.activeSelf)
            {
                pathPoints[disabledPoint].gameObject.SetActive(false);
            }
        }
    }
    public void nearWallDestroyed(float direction)
    {
        int nearWallNum = (int)direction / 45;
        if ((nearWallNum & 1) > 0)
        {
            if (nearWallNum == 7)
            {
                if (nearWalls[0] == null && nearWalls[6] == null)
                {
                    pathPoints[3].gameObject.SetActive(true);
                }
            }
            else
            {
                if (nearWalls[nearWallNum + 1] == null && nearWalls[nearWallNum - 1] == null)
                {
                    pathPoints[nearWallNum / 2].gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (nearWalls[(nearWallNum + 7) % 8] == null && nearWalls[(nearWallNum + 6) % 8] == null)
            {
                pathPoints[(nearWallNum + 7) % 8 / 2].gameObject.SetActive(true);
            }
            if (nearWalls[(nearWallNum + 9) % 8] == null && nearWalls[(nearWallNum + 10) % 8] == null)
            {
                pathPoints[(nearWallNum + 9) % 8 / 2].gameObject.SetActive(true);
            }
        }
        nearWalls[nearWallNum] = null;
    }
    public void refreshNearWalls()
    {
        Vector3 checkBoxSize = Vector3.one / 4;
        int activePathPoints = 15;
        Collider[] nearWall = Physics.OverlapBox(transform.position + Vector3.forward,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);
        if (nearWall.Length > 0)
        {
            if (nearWalls[0] != null)
            {
                nearWalls[0].nearWallDestroyed(180);
            }
            activePathPoints = activePathPoints & 6;
            nearWalls[0] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[0].nearPlacedNewWall(180, this);
        }
        else
        {
            if (nearWalls[0] != null)
            {
                nearWalls[0].nearWallDestroyed(180);
                nearWalls[0] = null;
            }
        }
        nearWall = Physics.OverlapBox(transform.position + Vector3.back,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);
        if (nearWall.Length > 0)
        {
            if (nearWalls[4] != null)
            {
                nearWalls[4].nearWallDestroyed(0);
            }
            activePathPoints = activePathPoints & 9;
            nearWalls[4] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[4].nearPlacedNewWall(0, this);
        }
        else
        {
            if (nearWalls[4] != null)
            {
                nearWalls[4].nearWallDestroyed(180);
                nearWalls[4] = null;
            }
        }
        nearWall = Physics.OverlapBox(transform.position + Vector3.right,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);
        if (nearWall.Length > 0)
        {
            if (nearWalls[2] != null)
            {
                nearWalls[2].nearWallDestroyed(270);
            }
            activePathPoints = activePathPoints & 12;
            nearWalls[2] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[2].nearPlacedNewWall(270, this);
        }
        else
        {
            if (nearWalls[2] != null)
            {
                nearWalls[2].nearWallDestroyed(270);
                nearWalls[2] = null;
            }
        }
        nearWall = Physics.OverlapBox(transform.position + Vector3.left,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);
        if (nearWall.Length > 0)
        {
            if (nearWalls[6] != null)
            {
                nearWalls[6].nearWallDestroyed(90);
            }
            activePathPoints = activePathPoints & 3;
            nearWalls[6] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[6].nearPlacedNewWall(90, this);
        }
        else
        {
            if (nearWalls[6] != null)
            {
                nearWalls[6].nearWallDestroyed(90);
                nearWalls[6] = null;
            }
        }
        nearWall = Physics.OverlapBox(transform.position + Vector3.forward + Vector3.right,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);
        if (nearWall.Length > 0)
        {
            if (nearWalls[1] != null)
            {
                nearWalls[1].nearWallDestroyed(225);
            }
            activePathPoints = activePathPoints & 14;
            nearWalls[1] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[1].nearPlacedNewWall(225, this);
        }
        else
        {
            if (nearWalls[1] != null)
            {
                nearWalls[1].nearWallDestroyed(225);
                nearWalls[1] = null;
            }
        }
        nearWall = Physics.OverlapBox(transform.position + Vector3.back + Vector3.right,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);
        if (nearWall.Length > 0)
        {
            if (nearWalls[3] != null)
            {
                nearWalls[3].nearWallDestroyed(315);
            }
            activePathPoints = activePathPoints & 13;
            nearWalls[3] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[3].nearPlacedNewWall(315, this);
        }
        else
        {
            if (nearWalls[3] != null)
            {
                nearWalls[3].nearWallDestroyed(315);
                nearWalls[3] = null;
            }
        }
        nearWall = Physics.OverlapBox(transform.position + Vector3.back + Vector3.left,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);
        if (nearWall.Length > 0)
        {
            if (nearWalls[5] != null)
            {
                nearWalls[5].nearWallDestroyed(45);
            }
            activePathPoints = activePathPoints & 11;
            nearWalls[5] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[5].nearPlacedNewWall(45, this);
        }
        else
        {
            if (nearWalls[5] != null)
            {
                nearWalls[5].nearWallDestroyed(45);
                nearWalls[5] = null;
            }
        }
        nearWall = Physics.OverlapBox(transform.position + Vector3.forward + Vector3.left,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);
        if (nearWall.Length > 0)
        {
            if (nearWalls[7] != null)
            {
                nearWalls[7].nearWallDestroyed(135);
            }
            activePathPoints = activePathPoints & 7;
            nearWalls[7] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[7].nearPlacedNewWall(135, this);
        }
        else
        {
            if (nearWalls[7] != null)
            {
                nearWalls[7].nearWallDestroyed(135);
                nearWalls[7] = null;
            }
        }

        bool isPointMustBeActive = (activePathPoints & 1) != 0;
        if (isPointMustBeActive!= pathPoints[0].gameObject.activeSelf)
        {
            pathPoints[0].gameObject.SetActive(isPointMustBeActive);
        }
        isPointMustBeActive = (activePathPoints & 2) != 0;
        if (isPointMustBeActive != pathPoints[1].gameObject.activeSelf)
        {
            pathPoints[1].gameObject.SetActive(isPointMustBeActive);
        }
        isPointMustBeActive = (activePathPoints & 4) != 0;
        if (isPointMustBeActive != pathPoints[2].gameObject.activeSelf)
        {
            pathPoints[2].gameObject.SetActive(isPointMustBeActive);
        }
        isPointMustBeActive = (activePathPoints & 8) != 0;
        if (isPointMustBeActive != pathPoints[3].gameObject.activeSelf)
        {
            pathPoints[3].gameObject.SetActive(isPointMustBeActive);
        }
    }
    private void Awake()
    {
        Vector3 checkBoxSize = Vector3.one / 4;
        int activePathPoints = 15;
        Collider[] nearWall=Physics.OverlapBox(transform.position+Vector3.forward,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);//0
        if (nearWall.Length>0)
        {
            activePathPoints = activePathPoints & 6;
            nearWalls[0] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[0].nearPlacedNewWall(180, this);
        }
        nearWall = Physics.OverlapBox(transform.position+Vector3.back,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);//4
        if (nearWall.Length > 0)
        {
            activePathPoints = activePathPoints & 9;
            nearWalls[4] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[4].nearPlacedNewWall(0, this);
        }
        nearWall = Physics.OverlapBox(transform.position+Vector3.right,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);//2
        if (nearWall.Length > 0)
        {
            activePathPoints = activePathPoints & 12;
            nearWalls[2] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[2].nearPlacedNewWall(270, this);
        }
        nearWall = Physics.OverlapBox(transform.position+Vector3.left,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);//6
        if (nearWall.Length > 0)
        {
            activePathPoints = activePathPoints & 3;
            nearWalls[6] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[6].nearPlacedNewWall(90, this);
        }
        nearWall = Physics.OverlapBox(transform.position+Vector3.forward+Vector3.right,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);//1
        if (nearWall.Length > 0)
        {
            activePathPoints = activePathPoints & 14;
            nearWalls[1] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[1].nearPlacedNewWall(225, this);
        }
        nearWall = Physics.OverlapBox(transform.position+Vector3.back+Vector3.right,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);//3
        if (nearWall.Length > 0)
        {
            activePathPoints = activePathPoints & 13;
            nearWalls[3] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[3].nearPlacedNewWall(315, this);
        }
        nearWall = Physics.OverlapBox(transform.position+Vector3.back+Vector3.left,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);//5
        if (nearWall.Length > 0)
        {
            activePathPoints = activePathPoints & 11;
            nearWalls[5] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[5].nearPlacedNewWall(45, this);
        }
        nearWall = Physics.OverlapBox(transform.position+Vector3.forward+Vector3.left,
            checkBoxSize, Quaternion.Euler(Vector3.zero), 512,QueryTriggerInteraction.Ignore);//7
        if (nearWall.Length > 0)
        {
            activePathPoints = activePathPoints & 7;
            nearWalls[7] = nearWall[0].GetComponent<wallHitPointSystem>();
            nearWalls[7].nearPlacedNewWall(135, this);
        }

        if ((activePathPoints & 1) != 0)
        {
            pathPoints[0].gameObject.SetActive(true);
        }
        if ((activePathPoints & 2) != 0)
        {
            pathPoints[1].gameObject.SetActive(true);
        }
        if ((activePathPoints & 4) != 0)
        {
            pathPoints[2].gameObject.SetActive(true);
        }
        if ((activePathPoints & 8) != 0)
        {
            pathPoints[3].gameObject.SetActive(true);
        }
        Gubernia502.walls.Add(this);
    }
}
