using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distantShoot : MonoBehaviour
{
    public ermakLockControl ermakLockControl;
    public GameObject animatiedMagazine=null;
    public GameObject nonAnimatedMagazine=null;
    private void spawnAnimatedShotgunMag()
    {
        animatiedMagazine = Instantiate(ermakLockControl.ermakInventory.EquippedWeapons.weaponsItem.magazinePrefab,
            ermakLockControl.ermakSelectedWeapon.gameObject.transform.position,
            ermakLockControl.ermakSelectedWeapon.gameObject.transform.rotation,
            ermakLockControl.transform);
        animatiedMagazine.GetComponent<magazineAnim>().ermakLockControl = ermakLockControl;
        animatiedMagazine.transform.localScale *= 4;
    }
    private void spawnAnimatedMag()
    {
        spawnAnimatedShotgunMag();
        Destroy(animatiedMagazine.GetComponent<Rigidbody>());
        Destroy(animatiedMagazine.GetComponent<Collider>());
    }
    private void spawnAnimatedMagWithAmmo(int animAmmoInMagNum)
    {
        spawnAnimatedMag();
        Instantiate(Gubernia502.constData.animatedAmmoInMag[animAmmoInMagNum],
                    animatiedMagazine.transform.position,
                    animatiedMagazine.transform.rotation,
                    animatiedMagazine.transform.GetChild(0));
    }
    public void takeSignalReload1()
    {
        spawnAnimatedMag();
    }//spawn magazine for animation
    public void takeSignalReload2()
    {
        {
            GameObject emptyMag;
            emptyMag = Instantiate(ermakLockControl.ermakInventory.EquippedWeapons.weaponsItem.magazinePrefab,
              ermakLockControl.iteractionScript.selectedWeaponScript.transform.position,
              ermakLockControl.iteractionScript.selectedWeaponScript.transform.rotation);
            Destroy(emptyMag.GetComponent<magazineAnim>());
            Destroy(emptyMag.GetComponent<Animator>());
            emptyMag.GetComponent<Rigidbody>().AddForce(
                new Vector3(ermakLockControl.ermakInventory.EquippedWeapons.weaponsItem.magImpulseTraectory.x * 
                                Mathf.Cos(ermakLockControl.weaponDispersion.transform.eulerAngles.y * Mathf.PI / 180) +
                            ermakLockControl.ermakInventory.EquippedWeapons.weaponsItem.magImpulseTraectory.z * 
                                Mathf.Sin(ermakLockControl.weaponDispersion.transform.eulerAngles.y * Mathf.PI / 180),
                            ermakLockControl.ermakInventory.EquippedWeapons.weaponsItem.magImpulseTraectory.y,
                            ermakLockControl.ermakInventory.EquippedWeapons.weaponsItem.magImpulseTraectory.z * 
                                Mathf.Cos(ermakLockControl.weaponDispersion.transform.eulerAngles.y * Mathf.PI / 180) +
                            ermakLockControl.ermakInventory.EquippedWeapons.weaponsItem.magImpulseTraectory.x * 
                                -Mathf.Sin(ermakLockControl.weaponDispersion.transform.eulerAngles.y * Mathf.PI / 180)) * 200);
        }
        nonAnimatedMagazine.SetActive(false);
    }//spawn empty magazine
    public void takeSignalReload3()
    {
            ermakLockControl.ermakInventory.loadAmmo(ermakLockControl.ermakInventory.EquippedWeapons.maxAmmoInMag-
                ermakLockControl.ermakInventory.EquippedWeapons.ammoInMag,
                ermakLockControl.ermakInventory.EquippedWeapons.currentAmmoId);
        Destroy(animatiedMagazine);
        nonAnimatedMagazine.SetActive(true);
    }//delete animation magazine and load ammo(not needed other method)
    public void takeSignalReload4()
    {
            ermakLockControl.ermakInventory.loadAmmo(1, ermakLockControl.ermakInventory.EquippedWeapons.currentAmmoId);
        Destroy(animatiedMagazine);
        if (ermakLockControl.ermakInventory.EquippedWeapons.ammoInMag >=
            ermakLockControl.ermakInventory.EquippedWeapons.maxAmmoInMag ||
            !ermakLockControl.ermakInventory.isHaveAmmo(ermakLockControl.ermakInventory.EquippedWeapons.currentAmmoId))
        {
            ermakLockControl.ermakAnim.SetBool("reload",false);
        }

    }//delete animation magazine,load ammo and restart reload,if mag no full(for shotgun)
    public void takeSignalReload5()
    {
        spawnAnimatedShotgunMag();
    }//spawn shotgunmagazine for animation
    public void takeSignalReload6()
    {
        spawnAnimatedMagWithAmmo(ermakLockControl.ermakInventory.EquippedWeapons.currentAmmoId - 1);
    }//spawn magazine for animation with add bullet in mag
    public void stopPickUpAnim()
    {
        ermakLockControl.stopPickUpAnim(2);
    }
    public void takeSignalShoot1()//spawn control rocket
    {
        ermakLockControl.iteractionScript.selectedWeaponScript.altShoot();
    }
}
