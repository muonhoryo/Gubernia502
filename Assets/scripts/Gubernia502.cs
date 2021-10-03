using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public static class Gubernia502
{
    public abstract class bulletStats
    {
        public float dispersionAngle;
        public abstract int prefabNum { get; }
        public abstract float flyAngle { get; set; }
        protected float FlyAngle;
        public abstract int hitDmg { get; set; }
        protected int HitDmg;
        public abstract float flySpeed { get; }
        public abstract Vector3 moveTraectory { get; }
        public abstract Vector3 bulletStart { get; set; }
        protected Vector3 BulletStart;
        public abstract GameObject owner { get; set; }
        protected GameObject Owner;
    }
    private class simpleBullet : bulletStats
    {
        public simpleBullet(float flyAngle, int hitDmg, Vector3 bulletStart, GameObject owner, float dispersionAngle)
        {
            this.dispersionAngle = dispersionAngle;
            this.flyAngle = flyAngle;
            this.hitDmg = hitDmg;
            this.bulletStart = bulletStart;
            this.owner = owner;
        }
        public override int prefabNum => 0;
        public override float flyAngle { get => FlyAngle; set => FlyAngle = value + Random.Range(-dispersionAngle / 2, dispersionAngle / 2); }
        public override int hitDmg { get => HitDmg; set => HitDmg = value; }
        public override float flySpeed => constData.simpleBulletSpeed;
        public override Vector3 moveTraectory
        {
            get => new Vector3(Mathf.Sin(flyAngle * Mathf.PI / 180),
                               0f,
                               Mathf.Cos(flyAngle * Mathf.PI / 180));
        }
        public override Vector3 bulletStart { get => BulletStart; set => BulletStart = value; }
        public override GameObject owner { get => Owner; set => Owner = value; }
    }
    private class cumulativeRocketShoot : simpleBullet
    {
        public cumulativeRocketShoot(float flyAngle, int hitDmg, Vector3 bulletStart, GameObject owner) :
            base(flyAngle, hitDmg, bulletStart, owner, 0f)
        {
        }
        public override float flyAngle { get => base.flyAngle; set => FlyAngle = value; }
        public override int prefabNum => 3;
        public override float flySpeed => constData.rocketSpeed;
        public readonly static float explosionRadius = constData.rocketExplosionRadius;
    }
    private class ctrlCumulativeRocketShoot : cumulativeRocketShoot
    {
        public ctrlCumulativeRocketShoot(float flyAngle, int hitDmg, Vector3 bulletStart, GameObject owner) :
                    base(flyAngle, hitDmg, bulletStart, owner)
        {
        }
        public override int prefabNum => 4;
    }
    private class shotgunShoot : simpleBullet
    {
        public shotgunShoot(float flyAngle, int hitDmg, Vector3 bulletStart, GameObject owner, float dispersionAngle)
            : base(flyAngle, hitDmg, bulletStart, owner, dispersionAngle)
        {
        }
        public readonly static int shootCount = constData.shotgunFragmentCount;
    }
    private class subSonicBullet : simpleBullet
    {
        public subSonicBullet(float flyAngle, int hitDmg, Vector3 bulletStart, GameObject owner, float dispersionAngle)
            : base(flyAngle, hitDmg, bulletStart, owner, dispersionAngle)
        {
        }
        public override int hitDmg
        {
            get => base.hitDmg;
            set
            {
                HitDmg = value - constData.subSonicDmgPenalty;
                if (HitDmg < 0)
                {
                    HitDmg = 0;
                }
            }
        }
        public override float flySpeed => constData.subSonicBulletSpeed;
    }
    private class rifleBullet : simpleBullet
    {
        public rifleBullet(float flyAngle, int hitDmg, Vector3 bulletStart, GameObject owner, float dispersionAngle)
            : base(flyAngle, hitDmg, bulletStart, owner, dispersionAngle)
        {
        }
        public override int prefabNum => 1;
        public override float flySpeed => constData.rifleBulletSpeed;
    }
    private class invasiveBullet : rifleBullet
    {
        public invasiveBullet(float flyAngle, int hitDmg, Vector3 bulletStart, GameObject owner, float dispersionAngle)
            : base(flyAngle, hitDmg, bulletStart, owner, dispersionAngle)
        {
        }
        public override int prefabNum => 2;
    }
    private static void rocket(cumulativeRocketShoot rocketStats)
    {
        GameObject rocket = bullet(rocketStats);
        rocket.GetComponent<cumulativeRocket>().explosionRadius = cumulativeRocketShoot.explosionRadius;
        rocket.GetComponent<cumulativeRocket>().bulletOwner = rocketStats.owner;
    }
    private static void controlRocket(ctrlCumulativeRocketShoot rocketStats)
    {
        GameObject rocket = bullet(rocketStats);
        mainCamera.changeToTargetTracking(rocket);
        rocket.GetComponent<controledCumulariveRocket>().explosionRadius = cumulativeRocketShoot.explosionRadius;
        rocket.GetComponent<controledCumulariveRocket>().bulletOwner = rocketStats.owner;
    }
    private static GameObject bullet<T>(T bulletStats)where T:simpleBullet
    {
        GameObject bullet = Object.Instantiate(constData.bullets[bulletStats.prefabNum], bulletStats.bulletStart,
                                      Quaternion.Euler(0f, bulletStats.flyAngle, 0f));
        bullet.GetComponent<bullet>().hitDmg = bulletStats.hitDmg;
        bullet.GetComponent<bullet>().speed = bulletStats.flySpeed;
        bullet.GetComponent<bullet>().moveTraectory = bulletStats.moveTraectory;
        bullet.GetComponent<bullet>().bulletOwner = bulletStats.owner;
        return bullet;
    }
    public static void spawnBullet(int type, int dmg, Vector3 bulletStart, float dispersionCenter, float dispersionAngle, GameObject owner)
    {
        switch (type)
        {
            case 2:
                bullet(new subSonicBullet(dispersionCenter, dmg, bulletStart, owner, dispersionAngle));
                break;
            case 4:
                for (int i = 0; i < shotgunShoot.shootCount - 1; i++)
                {
                    bullet(new shotgunShoot(dispersionCenter, dmg, bulletStart, owner, dispersionAngle));
                }
                break;
            case 5:
                bullet(new rifleBullet(dispersionCenter, dmg, bulletStart, owner, dispersionAngle));
                break;
            case 6:
                bullet(new invasiveBullet(dispersionCenter, dmg, bulletStart, owner, dispersionAngle));
                break;
            case 8:
                rocket(new cumulativeRocketShoot(dispersionCenter, dmg, bulletStart, owner));
                break;
            default:
                bullet(new simpleBullet(dispersionCenter, dmg, bulletStart, owner, dispersionAngle));
                break;
        }
    }
    public static void spawnControlRocket(int type, int dmg, Vector3 bulletStart, float dispersionCenter,
                                                float dispersionAngle, GameObject owner)
    {
        controlRocket(new ctrlCumulativeRocketShoot(dispersionCenter, dmg, bulletStart, owner));
    }
    public static readonly Color activeColor = new Color(0, 0, 1);
    public static readonly Color noneActiveColor = new Color(0.2196078f, 0.2196078f, 0.2196078f);
    private static bool GameIsActive = true;
    public static bool gameIsActive
    {
        get => GameIsActive;
        set
        {
            if (value)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].changeDefaultState();
                    enemies[i].setDefaultBehavior();
                }
                recalculatePathMap();
            }
            else
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].disableBatrak();
                }
            }
            if (playerController != null)
            {
                playerController.enabled = value;
            }
            GameIsActive = value;
        }
    }
    public delegate float getFloatFun();
    public delegate void simpleFun();
    public static Gubernia502ConstData constData;
    private static List<GameObject> drawingSpheres = new List<GameObject> { };
    public static List<temporalPathPoint> pathFindingMap = new List<temporalPathPoint> { };
    public static List<wallHitPointSystem> walls = new List<wallHitPointSystem> { };
    public static List<mobBehavior> enemies = new List<mobBehavior> { };
    public static List<collectibleWeaponsItem> weapons = new List<collectibleWeaponsItem> { };
    public static List<collectibleSimpleItem> items = new List<collectibleSimpleItem> { };
    public static string saveFileName;
    public static multiThreadManager threadManager;
    public static debugConsole debugConsole;
    public static MainCamera mainCamera;
    public static playerController playerController;
    public static mainMenu mainMenu;
    public enum fraction
    {
        serednyak = 0,
        batrak = 1,
        other=2
    }
    public class oneSidePathWay
    {
        public oneSidePathWay(temporalPathPoint startPoint, temporalPathPoint endPoint, twoSidePathWay pathWay)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.pathWay = pathWay;
            startPoint.ways.Add(this);
        }
        public readonly temporalPathPoint startPoint;
        public readonly temporalPathPoint endPoint;
        public readonly twoSidePathWay pathWay;
        public void destroyWay()
        {
            pathWay.destroyWay();
        }
    }
    public class twoSidePathWay
    {
        public twoSidePathWay(temporalPathPoint firstPoint, temporalPathPoint secondPoint)
        {
            firstWay = new oneSidePathWay(firstPoint, secondPoint, this);
            secondWay = new oneSidePathWay(secondPoint, firstPoint, this);
            distance = Vector3.Distance(firstPoint.transform.position, secondPoint.transform.position);
        }
        public readonly oneSidePathWay firstWay;
        public readonly oneSidePathWay secondWay;
        public readonly float distance;
        public void destroyWay()
        {
            firstWay.startPoint.ways.Remove(firstWay);
            secondWay.startPoint.ways.Remove(secondWay);
        }
    }
    public class pathFindThreadState
    {
        public pathFindThreadState(Vector3 start,Vector3 end)
        {
            this.start = start;
            this.end = end;
            waitHandler = new AutoResetEvent(true);
            temporalStart = null;
            temporalEnd = null;
        }
        public temporalPathPoint temporalStart;
        public temporalPathPoint temporalEnd;
        public Vector3 start { get; private set; }
        public Vector3 end { get; private set; }
        public AutoResetEvent waitHandler { get; private set; }
        public Thread currentThread;
        public List<Vector3> finalPath;
        public bool pathFindDone = false;
        public int threadIndex = -1;
        public void threadDextraPathFind()
        {
            dextraPathFindThread(this);
        }
    }
    private class dextraPathFinding
    {
        struct potentialPathWay
        {
            public potentialPathWay(potentialPathWay copyPathWay)
            {
                pathPoints = new List<temporalPathPoint> { };
                pathPoints.AddRange(copyPathWay.pathPoints);
                globalLength = copyPathWay.globalLength;
            }
            public potentialPathWay(temporalPathPoint startPoint)
            {
                pathPoints = new List<temporalPathPoint> { startPoint };
                globalLength = 0;
            }
            public List<temporalPathPoint> pathPoints { get; private set; }
            public float globalLength { get; private set; }
            public void addPathPoint(oneSidePathWay pathWay)
            {
                pathPoints.Add(pathWay.endPoint);
                globalLength += pathWay.pathWay.distance;
            }
        }
        public dextraPathFinding(pathFindThreadState pathFindState)
        {
            pathFindState.waitHandler.Reset();
            threadManager.addLateAction(delegate (pathFindThreadState pathFindState)
            {
                pathFindState.temporalStart = Object.Instantiate(Gubernia502.constData.temporalPathPoint,
                                                                new Vector3(pathFindState.start.x, 0, pathFindState.start.z),
                                                                Quaternion.Euler(Vector3.zero)).GetComponent<temporalPathPoint>();
                pathFindState.temporalEnd = Object.Instantiate(Gubernia502.constData.temporalPathPoint,
                                                                new Vector3(pathFindState.end.x, 0, pathFindState.end.z),
                                                                Quaternion.Euler(Vector3.zero)).GetComponent<temporalPathPoint>();
                Gubernia502.recalculateTemporalPathPointsWays(pathFindState.temporalStart);
                Gubernia502.recalculateTemporalPathPointsWays(pathFindState.temporalEnd);
            },pathFindState);
            pathFindState.waitHandler.WaitOne();
            createDextra(pathFindState);
        }
        void createDextra(pathFindThreadState pathFindState)
        {
            this.pathFindState = pathFindState;
            potentialPathPoints = new List<temporalPathPoint> { pathFindState.temporalStart };
            potentialPathWays = new List<potentialPathWay> { new potentialPathWay(pathFindState.temporalStart) };
            deadEnds = new List<temporalPathPoint> { };
            noneActualPoints = new List<temporalPathPoint> { };
            newPath = new potentialPathWay(pathFindState.temporalStart);
            toEndPoints = new List<temporalPathPoint> { };
            for (int i = 0; i < pathFindState.temporalEnd.ways.Count; i++)
            {
                toEndPoints.Add(pathFindState.temporalEnd.ways[i].endPoint);
            }
        }
        public pathFindThreadState pathFindState { get; private set; }
        List<temporalPathPoint> potentialPathPoints;
        List<temporalPathPoint> secondPotPathPoints;
        List<potentialPathWay> potentialPathWays;
        List<temporalPathPoint> deadEnds;
        List<temporalPathPoint> noneActualPoints;
        potentialPathWay newPath;
        List<temporalPathPoint> toEndPoints;
        public bool isPathWasFound { get; private set; } = false;
        private int findIndexOfPointInWays(temporalPathPoint point)
        {
            for(int i = 0; i < potentialPathWays.Count; i++)
            {
                if(potentialPathWays[i].pathPoints[potentialPathWays[i].pathPoints.Count-1]==
                    point)
                {
                    return i;
                }
            }
            return -1;
        }
        public List<Vector3> lastPath()
        {
            List<Vector3> path = new List<Vector3> { };
            for (int i = 0; i < newPath.pathPoints.Count; i++)
            {
                path.Add(newPath.pathPoints[i].position);
            }
            return path;
        }
        public void dextraPathFind()
        {
            dextraMainCycle();
        }
        private void dextraMainCycle()
        {
            if (pathFindState.temporalEnd.ways.Count <= 0 && pathFindState.temporalStart.ways.Count <= 0)
            {
                return;
            }
            secondPotPathPoints = new List<temporalPathPoint> { };
            while (true)
            {
                while (potentialPathPoints.Count>0)
                {
                    if (dextraPathFindingCycle(potentialPathPoints[0]))
                    {
                        return;
                    }
                }
                if (secondPotPathPoints.Count > 0)
                {
                    potentialPathPoints.AddRange(secondPotPathPoints);
                    secondPotPathPoints = new List<temporalPathPoint> { };
                }
                else
                {
                    return;
                }
            }
        }
        private bool dextraPathFindingCycle(temporalPathPoint currentPoint)
        {
            if (currentPoint == pathFindState.temporalEnd)
            {
                if (potentialPathPoints.Count+secondPotPathPoints.Count <= 1)
                {
                    isPathWasFound = true;
                    newPath = potentialPathWays[0];
                    return true;
                }
                else
                {
                    goto removePathPoint;
                }
            }
            if (currentPoint.ways.Count == 1 && noneActualPoints.Contains(currentPoint.ways[0].endPoint))
            {
                deadEnds.Add(currentPoint);
                goto removePathPoint;
            }
            int currentPointIndexInWays=findIndexOfPointInWays(currentPoint);
            for (int i = 0; i < currentPoint.ways.Count; i++)
            {
                if (!noneActualPoints.Contains(currentPoint.ways[i].endPoint) && !deadEnds.Contains(currentPoint.ways[i].endPoint))
                {
                    if (potentialPathPoints.Contains(currentPoint.ways[i].endPoint)||
                        secondPotPathPoints.Contains(currentPoint.ways[i].endPoint))
                    {
                        int nextPointIndex = findIndexOfPointInWays(currentPoint.ways[i].endPoint);
                        if (potentialPathWays[nextPointIndex].globalLength >
                            potentialPathWays[currentPointIndexInWays].globalLength + currentPoint.ways[i].pathWay.distance)
                        {
                            potentialPathWays[nextPointIndex] = new potentialPathWay(potentialPathWays[currentPointIndexInWays]);
                            potentialPathWays[nextPointIndex].addPathPoint(currentPoint.ways[i]);
                        }
                    }
                    else
                    {
                        secondPotPathPoints.Add(currentPoint.ways[i].endPoint);
                        potentialPathWays.Add(new potentialPathWay(potentialPathWays[currentPointIndexInWays]));
                        potentialPathWays[potentialPathWays.Count-1].addPathPoint(currentPoint.ways[i]);
                    }
                }
            }
            noneActualPoints.Add(currentPoint);
            potentialPathWays.RemoveAt(findIndexOfPointInWays(currentPoint));
        removePathPoint:
            potentialPathPoints.Remove(currentPoint);
            if (toEndPoints.Contains(currentPoint))
            {
                toEndPoints.Remove(currentPoint);
                if (toEndPoints.Count <= 0)
                {
                    newPath = potentialPathWays[findIndexOfPointInWays(pathFindState.temporalEnd)];
                    isPathWasFound = true;
                    return true;
                }
            }
            return false;
        }
    }//multithread
    public static void recalculatePathMap()
    {
        for(int i = 0; i < pathFindingMap.Count; i++)
        {
            recalculateTemporalPathPointsWays(pathFindingMap[i]);
        }
    }
    public static void recalculateTemporalPathPointsWays(temporalPathPoint point)
    {
        for (int i = 0; i < pathFindingMap.Count; i++)
        {
            if (point != pathFindingMap[i])
            {
                for (int j = 0; j < point.ways.Count; j++)
                {
                    if (point.ways[j].endPoint == pathFindingMap[i])
                    {
                        goto skipIteraction;
                    }
                }
                float rayCastDistance = Vector3.Distance(point.transform.position, pathFindingMap[i].transform.position);
                Vector3 direction = (pathFindingMap[i].transform.position - point.transform.position).normalized;
                if (!Physics.SphereCast(new Ray(point.transform.position, direction), constData.alifeObjColliderRadius,
                                      rayCastDistance, 512, QueryTriggerInteraction.Collide))
                {
                    new twoSidePathWay(point, pathFindingMap[i]);
                }
            skipIteraction:
                continue;
            }
        }
    }
    /*private static bool pathFindingCycle(ref List<Vector3>newPath,ref List<temporalPathPoint> noneActualPoints,
                                         temporalPathPoint currentPoint,ref List<List<Vector3>> potentialPaths,
                                         ref List<float> potentialPathsdistance,ref List<temporalPathPoint> deadEnds,
                                         ref temporalPathPoint endPoint,float newPathDistance,ref int pathSortIteractions,
                                         float highPriorityDirection=0)
    {
        newPath.Add(currentPoint.transform.position);
        if (currentPoint == endPoint)
        {
            potentialPaths.Add(new List<Vector3> { });
            potentialPaths[potentialPaths.Count-1].AddRange(newPath);
            potentialPathsdistance.Add(newPathDistance);
            if (potentialPaths.Count >= pathSortIteractions)
            {
                return true;
            }
            newPath.RemoveAt(newPath.Count - 1);
            //newPathDistance -= prevWayDistance;
            return false;
        }
        noneActualPoints.Add(currentPoint);
        int rightIndex;
        int leftIndex;
        int startWayNum=currentPoint.findNearestWayIndex(highPriorityDirection,out rightIndex,out leftIndex);
        bool isOnClockWise = rightIndex == startWayNum;
        for (int i = startWayNum,lIndex=leftIndex,rIndex=rightIndex;rIndex<=lIndex;)
        //for(int i=0;i<currentPoint.getWaysCount();i++)
        {
            temporalPathPoint nextPoint;
            if (currentPoint.getWay(i).points[0] == currentPoint)
            {
                nextPoint = currentPoint.getWay(i).points[1];
            }
            else
            {
                nextPoint = currentPoint.getWay(i).points[0];
            }
            if (nextPoint == endPoint)
            {
                potentialPaths.Add(new List<Vector3> { });
                potentialPaths[potentialPaths.Count-1].AddRange(newPath);
                potentialPaths[potentialPaths.Count - 1].Add(nextPoint.transform.position);
                potentialPathsdistance.Add(newPathDistance+currentPoint.getWay(i).distance);
                if (potentialPaths.Count >= pathSortIteractions)
                {
                    return true;
                }
                newPath.RemoveAt(newPath.Count - 1);
                return false;
            }
            //Debug.Log(currentPoint.gameObject.name +" ---->> "+ nextPoint.gameObject.name+"_______"+newPath.Count);
            if (deadEnds.Contains(nextPoint)||noneActualPoints.Contains(nextPoint))
            {
                continue;
            }
            if (nextPoint.getWaysCount() == 1)
            {
                deadEnds.Add(nextPoint);
                continue;
            }
            if (pathFindingCycle(ref newPath,ref noneActualPoints,nextPoint,ref potentialPaths,
                                 ref potentialPathsdistance,ref deadEnds,ref endPoint,newPathDistance+currentPoint.getWay(i).distance,
                                  ref pathSortIteractions))
            {
                return true;
            }
            if (isOnClockWise)
            {
                if (rIndex++ >= lIndex)
                {
                    break;
                }
                isOnClockWise=!isOnClockWise;
                i = lIndex;
            }
            else
            {
                if (lIndex-- <= rIndex)
                {
                    break;
                }
                isOnClockWise = !isOnClockWise;
                i = rIndex;
            }
        }
        newPath.RemoveAt(newPath.Count - 1);
        noneActualPoints.RemoveAt(noneActualPoints.Count - 1);
        return false;
    }
    public static List<Vector3>simplePathFinding(GameObject owner,Vector3 pathEnd,ref List<temporalPathPoint> toEndWays,
                                                 int pathSortIteractions,float highPriorityDirection=0)
    {
        temporalPathPoint endPoint = Object.Instantiate(temporalPathPoint, pathEnd, Quaternion.Euler(Vector3.zero)).GetComponent<temporalPathPoint>();
        toEndWays= recalculateTemporalPathPointsWays(endPoint);
        return pathFinding(owner,ref endPoint,pathSortIteractions,highPriorityDirection);
    }
    /*public static List<Vector3> economPathFinding(GameObject owner,Vector3 pathEnd,ref List<temporalPathPoint> oldToEndPoints,in List<Vector3>oldPath,out bool wasChanged,
                                                  int pathSortIteractions, GameObject highPriorityPoint = null)
    {
        temporalPathPoint endPoint = Object.Instantiate(temporalPathPoint, pathEnd, Quaternion.Euler(Vector3.zero)).GetComponent<temporalPathPoint>();
        List<temporalPathPoint> newToEndPoints= recalculateTemporalPathPointsWays(endPoint);
        if (oldToEndPoints.Count != newToEndPoints.Count)
        {
            Debug.Log("econom path finding,counts not equal");
            oldToEndPoints = newToEndPoints;
            wasChanged = true;
            return(pathFinding(owner,ref endPoint, pathSortIteractions,highPriorityPoint));
        }
        else
        {
            for (int i = 0; i < oldToEndPoints.Count; i++)
            {
                if (!newToEndPoints.Contains(oldToEndPoints[i]))
                {
                    Debug.Log("econom path finding,element(s) not found");
                    oldToEndPoints = newToEndPoints;
                    wasChanged = true;
                    return (pathFinding(owner,ref endPoint, pathSortIteractions, highPriorityPoint));
                }
            }
        }
        Object.Destroy(endPoint.gameObject);
        wasChanged = false;
        return (oldPath);
    }*/
    /*private static List< Vector3> pathFinding(GameObject owner,ref temporalPathPoint endPoint, int pathSortIteractions,float highPriorityDirection=0)
    {
        temporalPathPoint startPoint= Object.Instantiate(temporalPathPoint, owner.transform.position , Quaternion.Euler(Vector3.zero)).GetComponent<temporalPathPoint>();
        recalculateTemporalPathPointsWays(startPoint);
        List<Vector3> pathPoints;
        float currentPathDistance;
        List<List<Vector3>> potentialPaths=new List<List<Vector3>> { };
        List<float> potentialPathsdistance=new List<float> { };
        List<temporalPathPoint> deadEnds=new List<temporalPathPoint> { };
        List<Vector3> newPath = new List<Vector3> {};
        List<temporalPathPoint> noneActualPoints = new List<temporalPathPoint> { };
        pathFindingCycle(ref newPath, ref noneActualPoints, startPoint,
                         ref potentialPaths,ref potentialPathsdistance,ref deadEnds,ref endPoint,
                         0,ref pathSortIteractions);//simplePathFinding
        if (potentialPaths.Count == 0)
        {
            return null;
        }
        pathPoints = potentialPaths[0];
        currentPathDistance = potentialPathsdistance[0];
        for (int i = 1; i < potentialPaths.Count; i++)
        {
            if (potentialPathsdistance[i] < currentPathDistance)
            {
                pathPoints = potentialPaths[i];
                currentPathDistance = potentialPathsdistance[i];
            }
        }
        Debug.Log(currentPathDistance+"_____"+pathPoints.Count);
        Object.Destroy(startPoint.gameObject);
        Object.Destroy(endPoint.gameObject);
        return pathPoints;
    }*/
    public static List<Vector3>economPathFinding(List<Vector3>oldPath,Vector3 endPoint,Vector3 startPoint)
    {
        if (Physics.SphereCast(new Ray(new Vector3(oldPath[oldPath.Count - 2].x, 
                                                   endPoint.y, 
                                                   oldPath[oldPath.Count-2].z), 
                                      (endPoint - oldPath[oldPath.Count - 2]).normalized),
           constData.alifeObjColliderRadius, Vector3.Distance(endPoint, oldPath[oldPath.Count - 2]),
           512,QueryTriggerInteraction.Ignore))
        {
            return null;
        }
        else
        {
            oldPath[oldPath.Count - 1] = endPoint;
            return oldPath;
        }
    }
    public static void dextraPathFindThread(pathFindThreadState pathFindState)
    {
        dextraPathFinding path = new dextraPathFinding(pathFindState);
        path.dextraPathFind();
        pathFindState.waitHandler.Reset();
        threadManager.addLateAction(delegate (pathFindThreadState pathFindState)
        {
            if (pathFindState.temporalStart != null)
            {
                Object.Destroy(pathFindState.temporalStart.gameObject);
            }
            if (pathFindState.temporalEnd != null)
            {
                Object.Destroy(pathFindState.temporalEnd.gameObject);
            }
        }, pathFindState);
        pathFindState.waitHandler.WaitOne();
        if (path.isPathWasFound)
        {
            pathFindState.finalPath = path.lastPath();
        }
        else
        {
            pathFindState.finalPath = null;
        }
        pathFindState.pathFindDone = true;
    } 
    /*private static void APathFindingCycle(in temporalPathPoint endPoint,temporalPathPoint currentPoint,
        ref List<Vector3>newPath,ref List<Vector3> currentPath,ref float pathDistance,ref List<temporalPathPoint>noneActualPoints,
        ref List<temporalPathPoint>deadEnds,in int depthPathLevel,float currentPathDistance,int currentDepthLevel)
    {
        currentPath.Add(currentPoint.transform.position);
        currentDepthLevel++;
        if (currentPoint == endPoint)
        {
            if (newPath == null || currentPathDistance < pathDistance)
            {
                newPath = currentPath;
                pathDistance = currentPathDistance;
            }
            currentPath.RemoveAt(currentPath.Count - 1);
            return;
        }
        noneActualPoints.Add(currentPoint);
        if (currentDepthLevel >= depthPathLevel)
        {
            return;
        }
        for(int i = currentPoint.ways.Count - 1; i >= 0; i--)
        {
            if (!noneActualPoints.Contains(currentPoint.ways[i].endPoint) && !deadEnds.Contains(currentPoint.ways[i].endPoint))
            {
                APathFindingCycle(in endPoint, currentPoint.ways[i].endPoint, ref newPath, ref currentPath, ref pathDistance,
                    ref noneActualPoints, ref deadEnds, in depthPathLevel, currentPathDistance + currentPoint.ways[i].pathWay.distance,
                    currentDepthLevel);
            }
        }
        noneActualPoints.Remove(currentPoint);
        currentPath.RemoveAt(currentPath.Count - 1);
    }
    public static List<Vector3> APathFinding(Vector3 end,Vector3 start,int depthFindLevel)
    {
        Debug.Log("A path finding");
        temporalPathPoint startPoint = Object.Instantiate(constData.temporalPathPoint,
                                                        new Vector3(start.x, 0, start.z),
                                                        Quaternion.Euler(Vector3.zero)).GetComponent<temporalPathPoint>();
        recalculateTemporalPathPointsWays(startPoint);
        temporalPathPoint endPoint = Object.Instantiate(constData.temporalPathPoint,
                                                        new Vector3(end.x, 0, end.z),
                                                        Quaternion.Euler(Vector3.zero)).GetComponent<temporalPathPoint>();
        recalculateTemporalPathPointsWays(endPoint);
        List<Vector3> newPath =null;
        List<Vector3> currentPath = new List<Vector3> { };
        float pathDistance = 0;
        List<temporalPathPoint> noneActualPoints = new List<temporalPathPoint> { };
        List<temporalPathPoint> deadEnds = new List<temporalPathPoint> { };
        APathFindingCycle(endPoint, startPoint, ref newPath,ref currentPath, ref pathDistance, ref noneActualPoints, ref deadEnds,
            in depthFindLevel, 0, 0);
        if (newPath == null)
        {
            dextraPathFinding pathFind = new dextraPathFinding(ref startPoint,ref endPoint);
            pathFind.destroyPathPoints();
            return pathFind.dextraPathFind();
        }
        else
        {
            Object.Destroy(startPoint.gameObject);
            Object.Destroy(endPoint.gameObject);
            return newPath;
        }
    }*/
    public static void foundSideRotation(in float neededAngle,out int sideRotation, float rotatedAngle)
    {
        if (neededAngle > rotatedAngle)
        {
            if (neededAngle - rotatedAngle > 180)
            {
                sideRotation = -1;
                return;
            }
            sideRotation = 1;
        }
        else
        {
            if (rotatedAngle - neededAngle > 180)
            {
                sideRotation = 1;
                return;
            }
            sideRotation = -1;
        }
    }
    public static void drawSphereLine(float radius, Vector3 start, Vector3 end)
    {
        for (int i = 0; i < drawingSpheres.Count; i++)
        {
            Object.Destroy(drawingSpheres[i]);
        }
        if (constData.drawingSphere != null)
        {
            float distance = Vector3.Distance(start, end);
            Vector3 direction = (end - start).normalized;
            Vector3 currentCastPos = start;
            float stepsCount = (int)(distance / 0.05f) + 1;
            for (int i = 0; i < stepsCount; i++)
            {
                GameObject sphere = Object.Instantiate(constData.drawingSphere, currentCastPos, Quaternion.Euler(Vector3.zero));
                sphere.transform.localScale = Vector3.one * radius;
                currentCastPos += direction * 0.05f;
                drawingSpheres.Add(sphere);
            }
        }
    }
    /// <summary>
    /// Преобразование угла из float в Vector3
    /// </summary>
    /// <param name="angleInDegrees"></param>
    /// <param name="directionHeight"></param>
    /// <returns></returns>
    public static Vector3 directionFromAngle(float angleInDegrees,float directionHeight=0f)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), directionHeight, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    public static float angleFromDirection(Vector3 direction)
    {
        float angle=0;
        if (direction.z == 0)
        {
            if (direction.x > 0)
            {
                return 90;
            }
            return 270;
        }
        angle = Mathf.Atan(direction.x / direction.z) * Mathf.Rad2Deg;
        if (direction.z < 0)
        {
            angle += 180;
        }
        return (360+angle)%360;
    }
    public static float differenceOf2Angle(float firstValue,float secondValue)
    {
        float difference = 0;
        if (firstValue >= secondValue)
        {
            difference= firstValue - secondValue;
        }
        else
        {
            difference= secondValue - firstValue;
        }
        if (difference > 180)
        {
            return 360 - difference;
        }
        else
        {
            return difference;
        }
    }
    public static float xOffset(Vector3 offSet,float rotationAngle)
    {
        return offSet.x * Mathf.Cos(rotationAngle * Mathf.PI / 180) +
               offSet.z * Mathf.Sin(rotationAngle * Mathf.PI / 180);
    }
    public static float zOffset(Vector3 offSet, float rotationAngle)
    {
        return offSet.z * Mathf.Cos(rotationAngle * Mathf.PI / 180) +
               offSet.x * -Mathf.Sin(rotationAngle * Mathf.PI / 180);
    }
    public static int roundToUp(float a)
    {
        if (a % 1 != 0)
        {
            return ((int)a) + 1;
        }
        else
        {
            return (int)a;
        }
    }
}

