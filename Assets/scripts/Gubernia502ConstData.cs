using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Gubernia502ConstData: ScriptableObject
{
    public List<Gubernia502.fraction> batrakEnemyFractions=new List<Gubernia502.fraction> { };
    public List<Gubernia502.fraction> batrakFriendFractions = new List<Gubernia502.fraction> { };
    public TextAsset saveFile;
    //global const
    [Tooltip("time to working thread in one frame(in seconds)")]
    [Range(0,10)]
    public float threadFrameTime;
    [Range(0.0001f,100)]
    public float mainMenuSelectorDeadZone;
    [Range(0.0001f,1000)]
    public float mainMenuSelectorSpeed;
    [Range(0.1f, 10)]
    public float soundDegressSpeed;
    [Range(0, 100)]
    public float alifeObjColliderRadius;
    [Range(0.1f, 1000)]
    public float rifleBulletSpeed;
    [Range(0.1f, 1000)]
    public float subSonicBulletSpeed;
    [Range(0, 1000)]
    public int subSonicDmgPenalty;
    [Range(1, 100)]
    public int shotgunFragmentCount;
    [Range(0.1f, 100)]
    public float rocketExplosionRadius;
    [Range(0.1f, 1000)]
    public float rocketSpeed;
    [Range(0.1f, 1000)]
    public float simpleBulletSpeed;
    [Range(0, 100)]
    public int beyondExplRezist;
    [Range(0.1f, 100)]
    public float ctrlRocketRotationSpeed;
    [Range(1, 100)]
    public float invasiveOnHitDmgPenalty;
    [Range(1, 360)]
    public float invasiveOnHitDispersion;
    [Range(1, 10000)]
    public float bulletMaxMoveDistant;
    [Range(0,100)]
    public float subSonicSoundLevel;
    //player const
    [Range(0.1f, 100)]
    public float hideGuiDelayTime;
    [Tooltip("maximum sound level to full sound indicator")]
    [Range(0.1f, 100)]
    public float soundIndicMax;
    [Tooltip("minimum sound level to sound indicator")]
    [Range(0.1f, 100)]
    public float soundIndicMin;
    [Range(0, 100)]
    public float mainCameraMoveSpeed;
    [Range(0.1f, 250)]
    public float mainCameraHeight;
    [Tooltip("camera move speed modifier in redactor mode")]
    [Range(0, 1)]
    public float redactorCameraMoveSpeedMod;
    [Range(0.01f, 100)]
    public float mainCameraMaxMoveRadius;
    //ermak const
    [Range(0.1f, 100)]
    public float ermakMeleeBodyRotSpeed;
    [Range(0.1f, 100)]
    public float ermakMoveSoundVolume;
    [Range(0.1f, 100)]
    public float ermakMinSoundVolume;
    [Range(0.1f, 100)]
    public float ermakHandsSoundVolume;
    [Range(0.1f,100)]
    public float absolutVisionDistance;
    [Range(3,1000)]
    public int absolutVisionQuality;
    [Tooltip("second field of view range")]
    [Range(0.1f, 100)]
    public float ermakSecondFieldOfViewRange;
    [Tooltip("field of view raycast height")]
    [Range(0, 3)]
    public float fieldOfViewRayCastHeight;
    [Tooltip("field of view edge distant threshold")]
    [Range(0f, 100)]
    public float fieldOfViewEdgeDstThreshold;
    [Tooltip("field of view edge resolve iteractions")]
    [Range(1, 100)]
    public int fieldOfViewEdgeResolveIterations;
    [Tooltip("count of raycast to generate field")]
    [Range(3,1000)]
    public int ermakFieldOfViewQuality;
    [Tooltip("field of view angle")]
    [Range(0.1f, 360)]
    public float ermakFieldOfViewAngle;
    [Tooltip("field of view range")]
    [Range(0.1f, 100)]
    public float ermakFieldOfViewRange;
    [Tooltip("time delay on pick up item")]
    [Range(0.1f, 100)]
    public float ermakPickUpItemAnimDelay;
    [Tooltip("animation speed on pick up item")]
    [Range(0.1f, 100)]
    public float ermakPickUpItemAnimSpeed;
    [Range(0.1f, 100)]
    public float ermakBodyRotationSpeed;
    [Range(1, 1000)]
    public int ermakHandsDmg;
    [Range(0.1f, 100f)]
    public float ermakHeadRotationSpeed;
    [Range(0, 10)]
    public float ermakMoveSpeed;
    //batrak const
    [Range(0,1000)]
    public int batrakJerkDmg;
    [Tooltip("minimum difference between batrak move angle and target move angle to jerk")]
    [Range(0,360)]
    public float batrakJerkMinAngle;
    [Range(0,1000)]
    public int batrakSimpleAttackDmg;
    [Range(0, 10)]
    [Tooltip("min hit distance to move")]
    public float moveMinHitDistance;
    [Range(0.1f, 100)]
    public float batrakTimeToDeath;
    [Tooltip("batrak path finding iteractions count")]
    [Range(1, 1000)]
    public int batrakPathFindingIteractionsCount;
    [Range(0, 10)]
    public float batrakPassiveMoveSpeed;
    [Range(0, 10)]
    public float batrakActiveMoveSpeed;
    [Tooltip("direction between needed and current angle,when move speed is slowed")]
    [Range(0.1f, 360)]
    public float batrakSlowedMoveRotationAngle;
    [Tooltip("batrak simple attack rotation speed")]
    [Range(0.1f, 360)]
    public float batrakSimpleAtkRotSpeed;
    [Tooltip("batrak active rotation speed")]
    [Range(0.1f, 360)]
    public float batrakActiveRotationSpeed;
    [Tooltip("batrak passive rotation speed")]
    [Range(0.1f, 360)]
    public float batrakPassiveRotationSpeed;
    [Tooltip("max distance for simple combat")]
    [Range(0.1f, 10)]
    public float batrakMaxSimpleCombatDistance;
    [Tooltip("idle time before or between patrul state(batrak)")]
    [Range(0, 100)]
    public float batrakPatrulIdleStateTime;
    [Range(0,50)]
    public float batrakOnAttackMoveDoneDistance;
    [Range(0.001f,10)]
    public float batrakCheckSoundDelay;

    public GameObject saveLoader;
    public GameObject drawingSphere;
    public GameObject[] gamingItems;
    public GameObject[] bullets;
    public GameObject[] animatedAmmoInMag;
    public GameObject[] explosions;
    public GameObject[] effects;
    public GameObject[] levelObj;
    public GameObject[] enemies;
    public GameObject temporalPathPoint;
    public GameObject pathPoint;
    public List<simpleItem> ammoInfo = new List<simpleItem> { };
}
