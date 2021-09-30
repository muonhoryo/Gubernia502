using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class multiThreadManager : MonoBehaviour//singltone
{
    static multiThreadManager singltone = null;
    bool threadIsMustStoped = false;
    List<int> needTostopThread = new List<int> { };
    Gubernia502.simpleFun threadMode;
    object syns = new object();
    object stopingSyns = new object();
    public delegate void pathFindThreadAction(Gubernia502.pathFindThreadState pathFindState);
    saveLoader loader;
    int stateLoading = 0;
    public saveSystem.sceneSave sceneSave { get; private set; }
    public List<AutoResetEvent> waitHandlers { get; private set; } = new List<AutoResetEvent> { };
    public List<List<pathFindThreadAction>> lateActionsList { get; private set; } = new List<List<pathFindThreadAction>> { };
    public List<Gubernia502.pathFindThreadState> lateActionsHandler { get; private set; } = new List<Gubernia502.pathFindThreadState> { };
    public void addLateAction(pathFindThreadAction lateAction, Gubernia502.pathFindThreadState pathFindState)
    {
        lock (syns)
        {
            addLateActionsList(new List<pathFindThreadAction> { lateAction }, pathFindState);
        }
    }
    public void addLateActionsList(List<pathFindThreadAction>lateActions,Gubernia502.pathFindThreadState pathFindThreadState)
    {
        lock (syns)
        {
            lateActionsList.Add(lateActions);
            pathFindThreadState.threadIndex = lateActionsList.Count - 1;
            lateActionsHandler.Add(pathFindThreadState);
        }
    }
    public void requestStopThread(int threadIndex)
    {
        lock (stopingSyns)
        {
            needTostopThread.Add(threadIndex);
            threadIsMustStoped = true;
        }
    }
    public void startLoadLevel(saveLoader loader,saveSystem.sceneSave sceneSave)
    {
        this.loader = loader;
        this.sceneSave = sceneSave;
        threadMode = loadThreadMode;
    }
    void stopThread()
    {
        lock (syns)
        {
            lock (stopingSyns)
            {
                needTostopThread.Sort();
                int iteractionCount = needTostopThread.Count;
                int currentIteraction = 0;
                while (needTostopThread.Count > 0)
                {
                    int index = needTostopThread[0] - currentIteraction;
                    lateActionsList.RemoveAt(index);
                    lateActionsHandler[index].waitHandler.Set();
                    lateActionsHandler.RemoveAt(index);
                }
                threadIsMustStoped = false;
            }
        }
    }
    void simpleThreadMode()
    {
        if (lateActionsList.Count > 0)
        {
            while (lateActionsList.Count > 0)
            {
                while (lateActionsList[0].Count > 0)
                {
                    if (threadIsMustStoped)
                    {
                        stopThread();
                        return;
                    }
                    lateActionsList[0][0].Invoke(lateActionsHandler[0]);
                    lateActionsList[0].RemoveAt(0);
                    if (Time.timeSinceLevelLoad > Gubernia502.constData.threadFrameTime)
                    {
                        goto nextFrame;
                    }
                }
                lateActionsHandler[0].waitHandler.Set();
                lateActionsHandler.RemoveAt(0);
                lateActionsList.RemoveAt(0);
            }
        }
    nextFrame:;
    }
    void loadThreadMode()
    {
        float startTime = Time.realtimeSinceStartup;
        switch (loader.currentState)
        {
            case saveLoader.loadState.mainHeroLoad:
                break;
            case saveLoader.loadState.loadInit:
                break;
            case saveLoader.loadState.wallsLoad:
                goto wallsLoad;
            case saveLoader.loadState.enemiesLoad:
                goto enemiesLoad;
            case saveLoader.loadState.weaponsLoad:
                goto weaponsLoad;
            case saveLoader.loadState.itemsLoad:
                goto itemsLoad;
            default:
                Debug.LogError("unknown loadState(" + loader.currentState.ToString() + ")");
                goto loadEnd;
        }
        loader.currentState = saveLoader.loadState.mainHeroLoad;
        Gubernia502.mainMenu.updateLoadingInfo(loader.currentState);
        Gubernia502.playerController.ermakLockControl.transform.position = sceneSave.mainHero.position.vector;
        Gubernia502.playerController.ermakLockControl.bodyRotateScript.rotatedBody.rotation =
            Quaternion.Euler(0, sceneSave.mainHero.yRotation, 0);
        Gubernia502.playerController.ermakLockControl.hpSystem.hitPoint = sceneSave.mainHero.HPpoint;
        Gubernia502.playerController.ermakLockControl.hpSystem.shieldDurability = sceneSave.mainHero.shieldDuration;
        if (Time.realtimeSinceStartup - startTime>Gubernia502.constData.threadFrameTime)
        {
            return;
        }
        stateLoading = 0;
        loader.currentState = saveLoader.loadState.wallsLoad;
        wallsLoad:
        if (sceneSave.walls.Count > 0&&stateLoading<sceneSave.walls.Count)
        {
            for (; stateLoading < sceneSave.walls.Count; )
            {
                hitPointSystem wall = GameObject.Instantiate(Gubernia502.constData.levelObj[sceneSave.walls[stateLoading].phase - 1],
                    sceneSave.walls[stateLoading].position.vector, Quaternion.Euler(Vector3.zero)).GetComponent<hitPointSystem>();
                wall.hitPoint = sceneSave.walls[stateLoading].HPpoint;
                stateLoading++;
                if (Time.realtimeSinceStartup - startTime > Gubernia502.constData.threadFrameTime)
                {
                    Gubernia502.mainMenu.updateLoadingInfo(loader.currentState,stateLoading,sceneSave.walls.Count);
                    goto nextFrame;
                }
            }
        }
        stateLoading = 0;
        loader.currentState = saveLoader.loadState.enemiesLoad;
        enemiesLoad:
        if (sceneSave.enemies.Count > 0&&stateLoading<sceneSave.enemies.Count)
        {
            for (; stateLoading < sceneSave.enemies.Count; )
            {
                batrakBehavior enemy = GameObject.Instantiate(Gubernia502.constData.enemies[0],
                    sceneSave.enemies[stateLoading].position.vector,
                    Quaternion.Euler(Vector3.zero)).GetComponent<batrakBehavior>();
                enemy.bodyRotateScript.rotatedBody.rotation = Quaternion.Euler(new Vector3(0, sceneSave.enemies[stateLoading].yRotation, 0));
                enemy.dmgSystem.hitPoint = sceneSave.enemies[stateLoading].HPpoint;
                enemy.dmgSystem.shieldDurability = sceneSave.enemies[stateLoading].shieldDuration;
                if (sceneSave.enemies[stateLoading].patrulPoints.Count > 0)
                {
                    for (int j = 0; j < sceneSave.enemies[stateLoading].patrulPoints.Count; j++)
                    {
                        enemy.patrulPoint.Add(sceneSave.enemies[stateLoading].patrulPoints[j].vector);
                    }
                }
                switch (sceneSave.enemies[stateLoading].behavior)
                {
                    case "desactiveIdle":
                        enemy.startBehavior = batrakBehavior.startMode.desactiveIdle;
                        break;
                    case "passivePatrul":
                        enemy.startBehavior = batrakBehavior.startMode.passivePatrul;
                        break;
                    case "stayOnPoint":
                        enemy.startBehavior = batrakBehavior.startMode.stayOnPoint;
                        break;
                }
                enemy.changeDefaultState();
                stateLoading++;
                if (Time.realtimeSinceStartup - startTime > Gubernia502.constData.threadFrameTime)
                {
                    Gubernia502.mainMenu.updateLoadingInfo(loader.currentState, stateLoading, sceneSave.enemies.Count);
                    goto nextFrame;
                }
            }
        }
        stateLoading = 0;
        loader.currentState = saveLoader.loadState.weaponsLoad;
    weaponsLoad:
        if (sceneSave.weapons.Count > 0&&stateLoading<sceneSave.weapons.Count)
        {
            for (; stateLoading < sceneSave.weapons.Count;)
            {
                collectibleWeaponsItem weapon = GameObject.Instantiate(
                    Gubernia502.constData.gamingItems[sceneSave.weapons[stateLoading].id - 1],
                    sceneSave.weapons[stateLoading].position.vector,
                    Quaternion.Euler(sceneSave.weapons[stateLoading].eulerRotation.vector)).GetComponent<collectibleWeaponsItem>();
                weapon.damageBuff = sceneSave.weapons[stateLoading].damageBuff;
                weapon.speedBuff = sceneSave.weapons[stateLoading].speedBuff;
                weapon.magSizeBuff = sceneSave.weapons[stateLoading].magSizeBuff;
                weapon.accuracyBuff = sceneSave.weapons[stateLoading].accuracyBuff;
                weapon.durabilityBuff = sceneSave.weapons[stateLoading].durabilityBuff;
                weapon.ammoInMag = sceneSave.weapons[stateLoading].ammoInMag;
                weapon.ammoId = sceneSave.weapons[stateLoading].ammoId;
                weapon.durability = sceneSave.weapons[stateLoading].durability;
                stateLoading++;
                if (Time.realtimeSinceStartup - startTime > Gubernia502.constData.threadFrameTime)
                {
                    Gubernia502.mainMenu.updateLoadingInfo(loader.currentState, stateLoading, sceneSave.weapons.Count);
                    goto nextFrame;
                }
            }
        }
        stateLoading = 0;
        loader.currentState = saveLoader.loadState.itemsLoad;
    itemsLoad:
        if (sceneSave.items.Count > 0&&stateLoading<sceneSave.items.Count)
        {
            for (; stateLoading < sceneSave.items.Count;)
            {
                collectibleSimpleItem item = GameObject.Instantiate(
                    Gubernia502.constData.gamingItems[sceneSave.items[stateLoading].id - 1],
                    sceneSave.items[stateLoading].position.vector,
                    Quaternion.Euler(sceneSave.items[stateLoading].eulerRotation.vector)).GetComponent<collectibleSimpleItem>();
                item.count = sceneSave.items[stateLoading].count;
                stateLoading++;
                if (Time.realtimeSinceStartup - startTime > Gubernia502.constData.threadFrameTime)
                {
                    Gubernia502.mainMenu.updateLoadingInfo(loader.currentState, stateLoading, sceneSave.items.Count);
                    goto nextFrame;
                }
            }
        }
    loadEnd:
        loader.currentState = saveLoader.loadState.loadEnd;
        Gubernia502.mainMenu.updateLoadingInfo(loader.currentState);
        threadMode = simpleThreadMode;
        loader.waitHandler.Set();
        loader = null;
        sceneSave = null;
    nextFrame:;
    }
    void LateUpdate()
    {
        lock (syns)
        {
            threadMode();
        }
    }
    private void Awake()
    {
        if (singltone == null)
        {
            singltone = this;
            Gubernia502.threadManager = this;
            DontDestroyOnLoad(gameObject);
            threadMode = simpleThreadMode;
        }
        else
        {
            Destroy(this);
        }
    }
}
