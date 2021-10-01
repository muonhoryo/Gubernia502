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
        Gubernia502.spawnControlRocket(NPCLockControl.Inventory.EquippedWeapons.currentAmmoId,
                                       NPCLockControl.Inventory.EquippedWeapons.dmg,
                                       bulletStart.transform.position,
                                       NPCLockControl.weaponDispersion.rotateAngle,
                                       NPCLockControl.weaponDispersion.dispersion,
                                       NPCLockControl.gameObject);
        NPCLockControl.Inventory.deleteOneAmmo();
        nonAnimatedMagazine.SetActive(false);
        if (NPCLockControl.iteractionScript.selectedWeaponScript.takeDurabilityDmg() == true)
        {
            NPCLockControl.isBreakWeapon(NPCLockControl.weaponDispersion.rotateAngle);
        }
    }
    private void nonControlShoot()
    {
        nonAnimatedMagazine.SetActive(false);
        Gubernia502.spawnBullet(NPCLockControl.Inventory.EquippedWeapons.currentAmmoId,
            NPCLockControl.Inventory.EquippedWeapons.dmg, bulletStart.transform.position,
                                    NPCLockControl.weaponDispersion.rotateAngle,
                                    NPCLockControl.weaponDispersion.dispersion,
                                    NPCLockControl.gameObject);
        NPCLockControl.Inventory.deleteOneAmmo();
        if (takeDurabilityDmg() == true)
        {
            NPCLockControl.isBreakWeapon(NPCLockControl.weaponDispersion.rotateAngle);
        }
    }
    private void controlShoot()
    {
        NPCLockControl.setFullAnim();
        NPCLockControl.animator.SetTrigger("shoot");
        NPCLockControl.lockCtrl();
        NPCLockControl.weaponDispersion.gameObject.SetActive(false);
        NPCLockControl.viewBodyScript.transfmoredBody.rotation = Quaternion.Euler(0f, NPCLockControl.weaponDispersion.transform.rotation.eulerAngles.y, 0f);
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
        coolDown = NPCLockControl.Inventory.EquippedWeapons.coolDownTime;
        if (NPCLockControl.Inventory.EquippedWeapons.ammoInMag > 0)
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
