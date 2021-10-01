using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mapRedactor : MonoBehaviour//singltone
{
    static mapRedactor singltone = null;
    [SerializeField]
    private GameObject patrulPointSphere;
    Camera cameraComp;
    public enum redactorMode
    {
        adding,
        moving,
        deleting,
        addingP,
        movingP,
        deletingP
    }
    public enum addingObj
    {
        weapon,
        item,
        wall,
        enemy
    }
    private mobBehavior.startMode enemyStartBehavior = mobBehavior.startMode.desactiveIdle;
    private addingObj addingObjType = addingObj.weapon;
    private redactorMode mode = redactorMode.adding;
    private Gubernia502.simpleFun updateAction;
    [SerializeField]
    private Text addingEnemyBehavior;
    [SerializeField]
    private GameObject enemyBehaviorButton;
    [SerializeField]
    private GameObject changeAddingObjButton;
    [SerializeReference]
    private GameObject movingObj;
    [SerializeField]
    private GameObject mapRedactorPanel;
    [SerializeField]
    private GameObject redactorModePanel;
    [SerializeField]
    private Text redactorModeText;
    [SerializeField]
    private GameObject addingObjPanel;
    [SerializeField]
    private Text addingObjText;
    [SerializeField]
    GameObject playerInterface;
    [SerializeField]
    private Text[] propertiesNames = new Text[] { };
    [SerializeField]
    public Text[] propertiesFields = new Text[] { };

    public List<GameObject> enemyPatrulPoints = new List<GameObject> { };
    public List<mobBehavior> noneActiveEnemy = new List<mobBehavior> { };
    private void setMovingMode()
    {
        mode = redactorMode.moving;
        updateAction = movingObjectUpdate;
        redactorModeText.text = "moving";
        changeAddingObjButton.SetActive(false);
    }
    private void setAddingMode()
    {
        switch (addingObjType)
        {
            case addingObj.weapon:
                setAddWeapon();
                break;
            case addingObj.item:
                setAddItem();
                break;
            case addingObj.enemy:
                setAddEnemy();
                break;
            case addingObj.wall:
                setAddWall();
                break;
        }
        mode = redactorMode.adding;
        redactorModeText.text = "adding";
        changeAddingObjButton.SetActive(true);
    }
    private void setDeletingMode()
    {
        updateAction = deletingObjectUpdate;
        mode = redactorMode.deleting;
        redactorModeText.text = "deleting";
        changeAddingObjButton.SetActive(false);
    }
    private void setAddingPointMode()
    {
        mode = redactorMode.addingP;
        redactorModeText.text = "addingP";
        updateAction = addingPatrulPointUpdate;
    }
    private void setMovingPointMode()
    {
        mode = redactorMode.movingP;
        redactorModeText.text = "movingP";
        updateAction = movingPatrulPointUpdate;
    }
    private void setDeletingPointMode()
    {
        mode = redactorMode.deletingP;
        redactorModeText.text = "deletingP";
        updateAction = deletingPatrulPointUpdate;
    }

    private void setAddWeapon()
    {
        enemyBehaviorButton.SetActive(false);
        updateAction = addingWeaponUpdate;
        addingObjType = addingObj.weapon;
        addingObjText.text = "weapon";
        propertiesNames[0].text = "id";
        propertiesNames[1].text = "dmgBuff";
        propertiesNames[2].text = "spdBuff";
        propertiesNames[3].text = "mgSBuff";
        propertiesNames[4].text = "accBuff";
        propertiesNames[5].text = "durBuff";
        propertiesNames[6].text = "ammoIn";
        propertiesNames[7].text = "ammoId";
        propertiesNames[8].text = "durabil";

        propertiesFields[0].text = "15";
        propertiesFields[1].text = "0";
        propertiesFields[2].text = "0";
        propertiesFields[3].text = "0";
        propertiesFields[4].text = "0";
        propertiesFields[5].text = "0";
        propertiesFields[6].text = "0";
        propertiesFields[7].text = "1";
        propertiesFields[8].text = "100";
    }
    private void setAddItem()
    {
        enemyBehaviorButton.SetActive(false);
        updateAction = addingItemUpdate;
        addingObjType = addingObj.item;
        addingObjText.text = "item";
        propertiesNames[0].text = "id";
        propertiesNames[1].text = "count";
        propertiesNames[2].text = "";
        propertiesNames[3].text = "";
        propertiesNames[4].text = "";
        propertiesNames[5].text = "";
        propertiesNames[6].text = "";
        propertiesNames[7].text = "";
        propertiesNames[8].text = "";

        propertiesFields[0].text = "1";
        propertiesFields[1].text = "1";
        propertiesFields[2].text = "";
        propertiesFields[3].text = "";
        propertiesFields[4].text = "";
        propertiesFields[5].text = "";
        propertiesFields[6].text = "";
        propertiesFields[7].text = "";
        propertiesFields[8].text = "";
    }
    private void setAddEnemy()
    {
        enemyBehaviorButton.SetActive(true);
        updateAction = addingEnemyUpdate;
        addingObjType = addingObj.enemy;
        addingObjText.text = "enemy";
        propertiesNames[0].text = "shDur";
        propertiesNames[1].text = "HP";
        propertiesNames[2].text = "";
        propertiesNames[3].text = "";
        propertiesNames[4].text = "";
        propertiesNames[5].text = "";
        propertiesNames[6].text = "";
        propertiesNames[7].text = "";
        propertiesNames[8].text = "";

        propertiesFields[0].text = "0";
        propertiesFields[1].text = "1";
        propertiesFields[2].text = "";
        propertiesFields[3].text = "";
        propertiesFields[4].text = "";
        propertiesFields[5].text = "";
        propertiesFields[6].text = "";
        propertiesFields[7].text = "";
        propertiesFields[8].text = "";
    }
    private void setAddWall()
    {
        enemyBehaviorButton.SetActive(false);
        updateAction = addingWallUpdate;
        addingObjType = addingObj.wall;
        addingObjText.text = "wall";
        propertiesNames[0].text = "phase";
        propertiesNames[1].text = "HP";
        propertiesNames[2].text = "";
        propertiesNames[3].text = "";
        propertiesNames[4].text = "";
        propertiesNames[5].text = "";
        propertiesNames[6].text = "";
        propertiesNames[7].text = "";
        propertiesNames[8].text = "";

        propertiesFields[0].text = "1";
        propertiesFields[1].text = "1";
        propertiesFields[2].text = "";
        propertiesFields[3].text = "";
        propertiesFields[4].text = "";
        propertiesFields[5].text = "";
        propertiesFields[6].text = "";
        propertiesFields[7].text = "";
        propertiesFields[8].text = "";
    }

    public void nextEnemyBehavior()//patrul->potatoMode->stayOnPoint
    {
        switch (enemyStartBehavior)
        {
            case mobBehavior.startMode.passivePatrul:
                enemyStartBehavior = mobBehavior.startMode.desactiveIdle;
                addingEnemyBehavior.text = "desactiveIdle";
                break;
            case mobBehavior.startMode.desactiveIdle:
                enemyStartBehavior = mobBehavior.startMode.stayOnPoint;
                addingEnemyBehavior.text = "stayOnPoint";
                break;
            case mobBehavior.startMode.stayOnPoint:
                enemyStartBehavior = mobBehavior.startMode.passivePatrul;
                addingEnemyBehavior.text = "passivePatrul";
                break;
        }
    }
    public void nextRedactorMode()//adding(addingP->movingP->deletingP)->moving->deleting
    {
        switch (mode)
        {
            case redactorMode.adding:
                setMovingMode();
                break;
            case redactorMode.moving:
                setDeletingMode();
                break;
            case redactorMode.deleting:
                setAddingMode();
                break;
            //edit patrul points
            case redactorMode.addingP:
                setMovingPointMode();
                break;
            case redactorMode.movingP:
                setDeletingPointMode();
                break;
            case redactorMode.deletingP:
                setAddingPointMode();
                break;
        }
    }
    public void editPatrulPointsMode()
    {
        if ((int)mode <= 2)
        {
            setAddingPointMode();
        }
        else
        {
            setAddingMode();
        }
    }
    public void nextAddingObj()
    {
        if ((int)mode <= 2)
        {
            switch (addingObjType)
            {
                case addingObj.item:
                    setAddWeapon();
                    break;
                case addingObj.weapon:
                    setAddEnemy();
                    break;
                case addingObj.enemy:
                    setAddWall();
                    break;
                case addingObj.wall:
                    setAddItem();
                    break;
            }
        }
    }//item->weapon->enemy->wall
    public void enableRedactor()
    {
        if (Gubernia502.gameIsActive)
        {
            Gubernia502.gameIsActive = false;
            switch (mode)
            {
                case redactorMode.adding:
                    setAddingMode();
                    break;
                case redactorMode.moving:
                    setMovingMode();
                    break;
                case redactorMode.deleting:
                    setDeletingMode();
                    break;
                default:
                    setAddingMode();
                    break;
            }
            mapRedactorPanel.SetActive(true);
        }
        enabled = true;
        playerInterface.SetActive(false);
    }
    public void disableRedactor()
    {
        Gubernia502.gameIsActive = true;
        enabled = false;
        mapRedactorPanel.SetActive(false);
        playerInterface.SetActive(true);
    }
    private void addingItemUpdate()
    {
        if (Input.GetMouseButtonDown(0) &&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 pos = cameraComp.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraComp.transform.position.y));
            pos = new Vector3(pos.x, 1, pos.z);
            RaycastHit[] hits = Physics.SphereCastAll(new Ray(pos, Vector3.down), 0.5f, 1, 9984,QueryTriggerInteraction.Ignore);
            int id = int.Parse(propertiesFields[0].text);
            if ((id <15&&id>0) && hits.Length == 0)
            {
                collectibleSimpleItem item = Instantiate(Gubernia502.constData.gamingItems[id - 1],
                    pos, Quaternion.Euler(new Vector3(0, 0, 0))).GetComponent<collectibleSimpleItem>();
                item.count = int.Parse(propertiesFields[1].text);
            }
        }
    }
    private void addingWeaponUpdate()
    {
        if (Input.GetMouseButtonDown(0)&&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 pos = cameraComp.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraComp.transform.position.y));
            pos = new Vector3(pos.x, 1, pos.z);
            RaycastHit[] hits = Physics.SphereCastAll(new Ray(pos, Vector3.down), 0.5f, 1, 9984,QueryTriggerInteraction.Ignore);
            int id = int.Parse(propertiesFields[0].text);
            if ((id >= 15 || id <= 20)&&hits.Length==0)
            {
                collectibleWeaponsItem weapon = Instantiate(Gubernia502.constData.gamingItems[id - 1],
                    pos, Quaternion.Euler(new Vector3(0,0,-85))).GetComponent<collectibleWeaponsItem>();
                weapon.damageBuff = int.Parse( propertiesFields[1].text);
                weapon.speedBuff = int.Parse(propertiesFields[2].text);
                weapon.magSizeBuff = int.Parse(propertiesFields[3].text);
                weapon.accuracyBuff = int.Parse(propertiesFields[4].text);
                weapon.durabilityBuff = int.Parse(propertiesFields[5].text);
                weapon.ammoInMag = int.Parse(propertiesFields[6].text);
                weapon.ammoId = int.Parse(propertiesFields[7].text);
                weapon.durability = int.Parse(propertiesFields[8].text);
            }
        }
    }
    private void addingEnemyUpdate()
    {
        if (Input.GetMouseButtonDown(0) &&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 pos = cameraComp.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraComp.transform.position.y));
            pos = new Vector3(pos.x, 1, pos.z);
            RaycastHit[] hits = Physics.SphereCastAll(new Ray(pos, Vector3.down), 0.5f, 1, 9984,QueryTriggerInteraction.Ignore);
            if (hits.Length == 0)
            {
                mobBehavior enemy = Instantiate(Gubernia502.constData.enemies[0], new Vector3(pos.x, 0, pos.z),
                    Quaternion.Euler(Vector3.zero)).GetComponent<mobBehavior>();
                if (enemyPatrulPoints.Count > 0)
                {
                    for(int i=0,j=enemyPatrulPoints.Count;i<j;i++)
                    {
                        enemy.patrulPoint.Add(enemyPatrulPoints[0].transform.position);
                        Destroy(enemyPatrulPoints[0]);
                        enemyPatrulPoints.RemoveAt(0);
                    }
                }
                enemy.dmgSystem.shieldDurability = int.Parse(propertiesFields[0].text);
                enemy.dmgSystem.hitPoint = int.Parse(propertiesFields[1].text);
                enemy.startBehavior = enemyStartBehavior;
                enemy.changeDefaultState();
            }
        }
    }
    private void addingWallUpdate()
    {
        if(Input.GetMouseButtonDown(0)&&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 pos = cameraComp.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraComp.transform.position.y));
            float xMod;
            float zMod;
            if (pos.x < 0)
            {
                xMod = -0.5f;
            }
            else
            {
                xMod = 0.5f;
            }
            if (pos.z < 0)
            {
                zMod = -0.5f;
            }
            else
            {
                zMod = 0.5f;
            }
            pos = new Vector3((int)(pos.x+xMod)/1, 1, (int)(pos.z+zMod)/1);
            if (!Physics.CheckBox(new Vector3(pos.x, 0.5f, pos.z), Vector3.one*0.25f,
                Quaternion.Euler(Vector3.zero), 9984,QueryTriggerInteraction.Ignore))
            {
                hitPointSystem wall = Instantiate(Gubernia502.constData.levelObj[int.Parse(propertiesFields[0].text)-1], new Vector3(pos.x, 0, pos.z),
                    Quaternion.Euler(Vector3.zero)).GetComponent<hitPointSystem>();
                wall.hitPoint = int.Parse( propertiesFields[1].text);
            }
        }
    }
    private void addingPatrulPointUpdate()
    {
        if (Input.GetMouseButtonDown(0) &&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 pos = cameraComp.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraComp.transform.position.y));
            GameObject point = Instantiate(patrulPointSphere, new Vector3(pos.x, 0, pos.z),
                Quaternion.Euler(Vector3.zero));
            enemyPatrulPoints.Add(point);
        }
    }
    private void movingPatrulPointUpdate()
    {
        Vector3 pos = cameraComp.ScreenToWorldPoint(
               new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraComp.transform.position.y));
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, new Vector3(pos.x, 0, pos.z), out hit, 16384, QueryTriggerInteraction.Collide))
            {
                movingObj = hit.collider.gameObject;
            }
        }
        if (Input.GetMouseButton(0) && movingObj != null)
        {
            movingObj.transform.position = new Vector3(pos.x, movingObj.transform.position.y, pos.z);
        }
        if (Input.GetMouseButtonUp(0) && movingObj != null)
        {
            movingObj = null;
        }
    }
    private void movingObjectUpdate()
    {
        Vector3 pos = cameraComp.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraComp.transform.position.y));
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(transform.position, (new Vector3(pos.x, 0, pos.z) - transform.position).normalized),out hit,
                1000, 9984, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.GetComponent<hitPointSystem>() != null || hit.collider.GetComponent<collectiblleItem>() != null)
                {
                    movingObj = hit.collider.gameObject;
                }
                else
                {
                    if ((hit.collider.gameObject.layer & 1024) != 0)
                    {
                        movingObj = hit.collider.GetComponentInParent<collectiblleItem>().gameObject;
                    }
                    else
                    {
                        movingObj = hit.collider.GetComponentInParent<hitPointSystem>().gameObject;
                    }
                }
            }
        }
        if (Input.GetMouseButton(0)&&movingObj!=null)
        {
            if (movingObj.layer != 9)
            {
                RaycastHit[] hits = Physics.SphereCastAll(new Ray(new Vector3(pos.x, 1, pos.z), Vector3.down), 0.5f, 1, 9984,
                    QueryTriggerInteraction.Ignore);
                if (hits.Length == 0 || (hits.Length == 1 && hits[0].collider.gameObject == movingObj))
                {
                    movingObj.transform.position = new Vector3(pos.x, movingObj.transform.position.y, pos.z);
                }
            }
            else
            {
                float xMod;
                float zMod;
                if (pos.x < 0)
                {
                    xMod = -0.5f;
                }
                else
                {
                    xMod = 0.5f;
                }
                if (pos.z < 0)
                {
                    zMod = -0.5f;
                }
                else
                {
                    zMod = 0.5f;
                }
                pos = new Vector3((int)(pos.x + xMod) / 1, 1, (int)(pos.z + zMod) / 1);
                if (!Physics.CheckBox(new Vector3(pos.x, 0.5f, pos.z),
                    Vector3.one * 0.25f, Quaternion.Euler(Vector3.zero), 9984,QueryTriggerInteraction.Ignore))
                {
                    movingObj.transform.position = new Vector3(pos.x, movingObj.transform.position.y, pos.z);
                    movingObj.GetComponent<wallHitPointSystem>().refreshNearWalls();
                }
            }
        }
        if (Input.GetMouseButtonUp(0)&&movingObj!=null)
        {
            movingObj = null;
        }
    }
    private void deletingPatrulPointUpdate()
    {
        if(Input.GetMouseButtonDown(0) &&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 pos = cameraComp.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraComp.transform.position.y));
            RaycastHit hit;
            if(Physics.Linecast(transform.position,new Vector3(pos.x,0,pos.z),out hit, 16384, QueryTriggerInteraction.Collide))
            {
                enemyPatrulPoints.Remove(hit.collider.gameObject);
                Destroy(hit.collider.gameObject);
            }
        }
    }
    private void deletingObjectUpdate()
    {
        if(Input.GetMouseButtonDown(0)&&
            !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 pos = cameraComp.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraComp.transform.position.y));
            RaycastHit hit;
            if (Physics.Linecast(transform.position, new Vector3(pos.x, 0, pos.z), out hit, 1792, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.GetComponent<hitPointSystem>() != null || hit.collider.GetComponent<collectiblleItem>() != null)
                {
                    Destroy( hit.collider.gameObject);
                }
                else
                {
                    if ((hit.collider.gameObject.layer & 1024) != 0)
                    {
                        Destroy( hit.collider.GetComponentInParent<collectiblleItem>().gameObject);
                    }
                    else
                    {
                        hitPointSystem deletingObj = hit.collider.GetComponentInParent<hitPointSystem>();
                        Destroy( deletingObj.gameObject);
                    }
                }
            }
        }
    }
    public void changePropertie(int propertieNum)
    {
        if (enabled)
        {
            switch (addingObjType)
            {
                case addingObj.weapon:
                    addChangePropertieScript<changeWeaponPropertie>(propertieNum);
                    break;
                case addingObj.item:
                    if (propertieNum < 3)
                    {
                        addChangePropertieScript<changeItemPropertie>(propertieNum);
                    }
                    break;
                case addingObj.enemy:
                    if (propertieNum < 3)
                    {
                        addChangePropertieScript<changeEnemyProperties>(propertieNum);
                    }
                    break;
                case addingObj.wall:
                    if (propertieNum < 3)
                    {
                        addChangePropertieScript<changeWallPropertie>(propertieNum);
                    }
                    break;
            }
        }
    }
    private void addChangePropertieScript<T>(int propertieNum) where T:changeWeaponPropertie
    {
        T changeObjectPropertie = gameObject.AddComponent<T>();
        changeObjectPropertie.redactor = this;
        changeObjectPropertie.changingPropertieNum = propertieNum - 1;
        propertiesFields[changeObjectPropertie.changingPropertieNum].text =
            propertiesFields[changeObjectPropertie.changingPropertieNum].text + "_";
        enabled = false;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            Gubernia502.debugConsole.enabled = true;
        }
        updateAction();
    }
    private void Awake()
    {
        if (singltone == null)
        {
            singltone = this;
            cameraComp = GetComponent<Camera>();
        }
        else
        {
            Destroy(this);
        }
    }
}
