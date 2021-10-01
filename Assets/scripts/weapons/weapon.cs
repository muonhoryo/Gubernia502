using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class weapon : MonoBehaviour
{
    public int coolDown=0;
    public NPCLockControl NPCLockControl;
    public GameObject bulletStart;
    public virtual void generateSound() { }
    public void generateSound(float soundVolume)
    {
        NPCLockControl.soundGenerator.soundLevel = soundVolume;
    }
    public virtual void altShoot()
    {
    }
    public bool takeDurabilityDmg(int durabilityDmg=1)
    {//true-если сломалось при выстреле,false-если нет
        return NPCLockControl.Inventory.dmgSelectedWeapon(durabilityDmg);
    }
    public virtual void shoot()
    {
    }
    public virtual void reload()
    {

    }
    public virtual void changeAmmoType()
    {

    }
    public virtual void changeShootMode()
    {

    }
    public virtual void FixedUpdate()
    {
        
    }
    protected virtual void Start()
    {
    }
}
