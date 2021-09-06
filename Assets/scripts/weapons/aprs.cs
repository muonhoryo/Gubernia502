using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aprs : automat
{
    delegate void aprsShoot();
    private aprsShoot fire;
    private bool fireMode;
    public override void altShoot()
    {
        Gubernia502.spawnControlRocket(ermakLockControl.ermakInventory.EquippedWeapons.currentAmmoId,
                                       ermakLockControl.ermakInventory.EquippedWeapons.dmg,
                                       bulletStart.transform.position,
                                       ermakLockControl.weaponDispersion.rotateAngle,
                                       ermakLockControl.weaponDispersion.dispersion,
                                       ermakLockControl.gameObject);
        ermakLockControl.ermakInventory.deleteOneAmmo();
        nonAnimatedMagazine.SetActive(false);
        if (ermakLockControl.iteractionScript.selectedWeaponScript.takeDurabilityDmg() == true)
        {
            ermakLockControl.isBreakWeapon(ermakLockControl.weaponDispersion.rotateAngle);
        }
    }
    private void nonControlShoot()
    {
        nonAnimatedMagazine.SetActive(false);
        Gubernia502.spawnBullet(ermakLockControl.ermakInventory.EquippedWeapons.currentAmmoId,
            ermakLockControl.ermakInventory.EquippedWeapons.dmg, bulletStart.transform.position,
                                    ermakLockControl.weaponDispersion.rotateAngle,
                                    ermakLockControl.weaponDispersion.dispersion,
                                    ermakLockControl.gameObject);
        ermakLockControl.ermakInventory.deleteOneAmmo();
        if (takeDurabilityDmg() == true)
        {
            ermakLockControl.isBreakWeapon(ermakLockControl.weaponDispersion.rotateAngle);
        }
    }
    private void controlShoot()
    {
        ermakLockControl.setFullAnim();
        ermakLockControl.ermakAnim.SetTrigger("shoot");
        ermakLockControl.lockCtrl();
        ermakLockControl.weaponDispersion.gameObject.SetActive(false);
        ermakLockControl.viewBodyScript.ermakBody.rotation = Quaternion.Euler(0f, ermakLockControl.weaponDispersion.transform.rotation.eulerAngles.y, 0f);
    }
    public override void changeShootMode()
    {
        if (fireMode)
        {
            fire = nonControlShoot;
            fireMode = false;
        }
        else
        {
            fire = controlShoot;
            fireMode = true;
        }
    }
    public override void shoot()
    {
        coolDown = ermakLockControl.ermakInventory.EquippedWeapons.coolDownTime;
        if (ermakLockControl.ermakInventory.EquippedWeapons.ammoInMag > 0)
        {
            fire();
        }
        else
        {
            reload();
        }
    }
    protected override void Start()
    {
        fire = nonControlShoot;
        fireMode = false;
    }
}
