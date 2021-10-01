using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distantShoot : MonoBehaviour
{
    public NPCLockControl NPCLockControl;
    public GameObject animatiedMagazine=null;
    public GameObject nonAnimatedMagazine=null;
    private void spawnAnimatedShotgunMag()
    {
        animatiedMagazine = Instantiate(NPCLockControl.Inventory.EquippedWeapons.weaponsItem.magazinePrefab,
            NPCLockControl.selectedWeaponScript.gameObject.transform.position,
            NPCLockControl.selectedWeaponScript.gameObject.transform.rotation,
            NPCLockControl.transform);
        animatiedMagazine.GetComponent<magazineAnim>().NPCLockControl = NPCLockControl;
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
            emptyMag = Instantiate(NPCLockControl.Inventory.EquippedWeapons.weaponsItem.magazinePrefab,
              NPCLockControl.iteractionScript.selectedWeaponScript.transform.position,
              NPCLockControl.iteractionScript.selectedWeaponScript.transform.rotation);
            Destroy(emptyMag.GetComponent<magazineAnim>());
            Destroy(emptyMag.GetComponent<Animator>());
            emptyMag.GetComponent<Rigidbody>().AddForce(
                new Vector3(NPCLockControl.Inventory.EquippedWeapons.weaponsItem.magImpulseTraectory.x * 
                                Mathf.Cos(NPCLockControl.weaponDispersion.transform.eulerAngles.y * Mathf.PI / 180) +
                            NPCLockControl.Inventory.EquippedWeapons.weaponsItem.magImpulseTraectory.z * 
                                Mathf.Sin(NPCLockControl.weaponDispersion.transform.eulerAngles.y * Mathf.PI / 180),
                            NPCLockControl.Inventory.EquippedWeapons.weaponsItem.magImpulseTraectory.y,
                            NPCLockControl.Inventory.EquippedWeapons.weaponsItem.magImpulseTraectory.z * 
                                Mathf.Cos(NPCLockControl.weaponDispersion.transform.eulerAngles.y * Mathf.PI / 180) +
                            NPCLockControl.Inventory.EquippedWeapons.weaponsItem.magImpulseTraectory.x * 
                                -Mathf.Sin(NPCLockControl.weaponDispersion.transform.eulerAngles.y * Mathf.PI / 180)) * 200);
        }
        nonAnimatedMagazine.SetActive(false);
    }//spawn empty magazine
    public void takeSignalReload3()
    {
            NPCLockControl.Inventory.loadAmmo(NPCLockControl.Inventory.EquippedWeapons.maxAmmoInMag-
                NPCLockControl.Inventory.EquippedWeapons.ammoInMag,
                NPCLockControl.Inventory.EquippedWeapons.currentAmmoId);
        Destroy(animatiedMagazine);
        nonAnimatedMagazine.SetActive(true);
    }//delete animation magazine and load ammo(not needed other method)
    public void takeSignalReload4()
    {
            NPCLockControl.Inventory.loadAmmo(1, NPCLockControl.Inventory.EquippedWeapons.currentAmmoId);
        Destroy(animatiedMagazine);
        if (NPCLockControl.Inventory.EquippedWeapons.ammoInMag >=
            NPCLockControl.Inventory.EquippedWeapons.maxAmmoInMag ||
            !NPCLockControl.Inventory.isHaveAmmo(NPCLockControl.Inventory.EquippedWeapons.currentAmmoId))
        {
            NPCLockControl.animator.SetBool("reload",false);
        }

    }//delete animation magazine,load ammo and restart reload,if mag no full(for shotgun)
    public void takeSignalReload5()
    {
        spawnAnimatedShotgunMag();
    }//spawn shotgunmagazine for animation
    public void takeSignalReload6()
    {
        spawnAnimatedMagWithAmmo(NPCLockControl.Inventory.EquippedWeapons.currentAmmoId - 1);
    }//spawn magazine for animation with add bullet in mag
    public void stopPickUpAnim()
    {
        NPCLockControl.stopPickUpAnim(2);
    }
    public void takeSignalShoot1()//spawn control rocket
    {
        NPCLockControl.iteractionScript.selectedWeaponScript.altShoot();
    }
}
