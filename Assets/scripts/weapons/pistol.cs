using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistol : weapon
{
    public GameObject nonAnimatedMagazine;
    public GameObject altBulletStart;
    public override void shoot()
    {
        coolDown = NPCLockControl.Inventory.EquippedWeapons.coolDownTime;
        if(NPCLockControl.Inventory.EquippedWeapons.ammoInMag > 0)
        {
            Vector3 bullStart = bulletStart.transform.position;
            generateSound();
            RaycastHit[] hits= Physics.RaycastAll(altBulletStart.transform.position,
                Gubernia502.directionFromAngle(NPCLockControl.weaponDispersion.rotateAngle),
                NPCLockControl.Inventory.EquippedWeapons.weaponsItem.altBulletStartZOffset,
                ~(1 << 10),QueryTriggerInteraction.Ignore);
            if (hits.Length>1||hits.Length>0&&hits[0].collider.gameObject!=NPCLockControl.gameObject)
            {
                bullStart = altBulletStart.transform.position;
            }
            Instantiate(Gubernia502.constData.effects[0], bulletStart.transform.position,
                        Quaternion.Euler(0, NPCLockControl.weaponDispersion.rotateAngle, 0), bulletStart.transform);
            Gubernia502.spawnBullet(NPCLockControl.Inventory.EquippedWeapons.currentAmmoId, 
                NPCLockControl.Inventory.EquippedWeapons.dmg, bullStart,
                                NPCLockControl.weaponDispersion.gameObject.transform.rotation.eulerAngles.y,
                                NPCLockControl.weaponDispersion.dispersion,
                                NPCLockControl.gameObject);
            NPCLockControl.Inventory.deleteOneAmmo();
            if (takeDurabilityDmg()==false)
            { 
                NPCLockControl.animator.SetTrigger("shoot");
            }
            else
            {
                NPCLockControl.isBreakWeapon(NPCLockControl.weaponDispersion.transform.localRotation.eulerAngles.y);
            }
        }
        else
        {
            reload();
        }
    }
    public override void reload()
    {
        if(NPCLockControl.Inventory.EquippedWeapons.ammoInMag <
               NPCLockControl.Inventory.EquippedWeapons.maxAmmoInMag &&
               NPCLockControl.Inventory.isHaveAmmo(NPCLockControl.Inventory.EquippedWeapons.currentAmmoId))
        {
            NPCLockControl.distantShoot.nonAnimatedMagazine = nonAnimatedMagazine;
            NPCLockControl.animator.SetBool("reload",true);
            NPCLockControl.lockOtherCtrl();
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
        NPCLockControl.Inventory.unloadWeapon();
        NPCLockControl.Inventory.nextAmmoType();
        NPCLockControl.iteractionScript.reload();
    }
    public override void generateSound()
    {
        if (NPCLockControl.Inventory.EquippedWeapons.currentAmmoId != 2)
        {
            generateSound(NPCLockControl.Inventory.EquippedWeapons.weaponsItem.soundVolume);
        }
        else
        {
            generateSound(Gubernia502.constData.subSonicSoundLevel);
        }
    }
}
