using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistol : weapon
{
    public GameObject nonAnimatedMagazine;
    public GameObject altBulletStart;
    public override void shoot()
    {
        coolDown = ermakLockControl.ermakInventory.EquippedWeapons.coolDownTime;
        if(ermakLockControl.ermakInventory.EquippedWeapons.ammoInMag > 0)
        {
            Vector3 bullStart = bulletStart.transform.position;
            generateSound();
            RaycastHit[] hits= Physics.RaycastAll(altBulletStart.transform.position,
                Gubernia502.directionFromAngle(ermakLockControl.weaponDispersion.rotateAngle),
                ermakLockControl.ermakInventory.EquippedWeapons.weaponsItem.altBulletStartZOffset,
                ~(1 << 10),QueryTriggerInteraction.Ignore);
            if (hits.Length>1||hits.Length>0&&hits[0].collider.gameObject!=ermakLockControl.gameObject)
            {
                bullStart = altBulletStart.transform.position;
            }
            Instantiate(Gubernia502.constData.effects[0], bulletStart.transform.position,
                        Quaternion.Euler(0, ermakLockControl.weaponDispersion.rotateAngle, 0), bulletStart.transform);
            Gubernia502.spawnBullet(ermakLockControl.ermakInventory.EquippedWeapons.currentAmmoId, 
                ermakLockControl.ermakInventory.EquippedWeapons.dmg, bullStart,
                                ermakLockControl.weaponDispersion.gameObject.transform.rotation.eulerAngles.y,
                                ermakLockControl.weaponDispersion.dispersion,
                                ermakLockControl.gameObject);
            ermakLockControl.ermakInventory.deleteOneAmmo();
            if (takeDurabilityDmg()==false)
            { 
                ermakLockControl.ermakAnim.SetTrigger("shoot");
            }
            else
            {
                ermakLockControl.isBreakWeapon(ermakLockControl.weaponDispersion.transform.localRotation.eulerAngles.y);
            }
        }
        else
        {
            reload();
        }
    }
    public override void reload()
    {
        if(ermakLockControl.ermakInventory.EquippedWeapons.ammoInMag <
               ermakLockControl.ermakInventory.EquippedWeapons.maxAmmoInMag &&
               ermakLockControl.ermakInventory.isHaveAmmo(ermakLockControl.ermakInventory.EquippedWeapons.currentAmmoId))
        {
            ermakLockControl.distantShoot.nonAnimatedMagazine = nonAnimatedMagazine;
            ermakLockControl.ermakAnim.SetBool("reload",true);
            ermakLockControl.lockOtherCtrl();
        }
    }
    public override void FixedUpdate()
    {
        if (coolDown > 0)
        {
            coolDown -= 1;
        }
    }
    public override void changeAmmoType()
    {
        ermakLockControl.ermakInventory.unloadWeapon();
        ermakLockControl.ermakInventory.nextAmmoType();
        ermakLockControl.iteractionScript.reload();
    }
    public override void generateSound()
    {
        if (ermakLockControl.ermakInventory.EquippedWeapons.currentAmmoId != 2)
        {
            generateSound(ermakLockControl.ermakInventory.EquippedWeapons.weaponsItem.soundVolume);
        }
        else
        {
            generateSound(Gubernia502.constData.subSonicSoundLevel);
        }
    }
}
