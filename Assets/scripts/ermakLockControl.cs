using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermakLockControl : MonoBehaviour
{
    public float bodyRotationSpeed;
    public bool isLockedCtrl { get; private set; } = false;
    public ermakColliderMove collMove;
    public GameObject[] hands = new GameObject[2]//0-right,1-leftS
    {
        null,
        null
    };
    public Animator ermakAnim;
    public meleeShoot meleeShoot;
    public distantShoot distantShoot;
    public ermakWeaponDispersion weaponDispersion;
    public ermakSelectedWeapon ermakSelectedWeapon;
    public soundGenerator soundGenerator;
    public meleeHitBox meleeFrontHitBox;
    public ermakDmgSystem hpSystem;
    public ermakBodyRotateForView bodyRotateScript;
    public ermakMove moveScript;
    public ermakIteraction iteractionScript;
    public ermakViewBody viewBodyScript;
    public Rigidbody ermakRGBody;
    public ermakInventory ermakInventory;
    /// <summary>
    /// full lock control
    /// </summary>
    public void lockCtrl()
    {
        weaponDispersion.enabled = false;
        moveScript.isActive = false;
        iteractionScript.isActiveIteraction = false;
        viewBodyScript.enabled = false;
        isLockedCtrl = true;
    }
    public void meleeLock()
    {
        moveScript.isActive = false;
        iteractionScript.isActiveIteraction = false;
        isLockedCtrl = true;
    }
    public void meleeLockRotation()
    {
        bodyRotateScript.enabled = false;
        weaponDispersion.enabled = false;
        viewBodyScript.enabled = false;
    }
    /// <summary>
    /// lock iteraction
    /// </summary>
    public void lockOtherCtrl()
    {
        iteractionScript.isActiveIteraction = false;
    }
    /// <summary>
    /// unlock move,iteraction and view
    /// </summary>
    public void unlockCtrl()
    {
        isLockedCtrl = false;
        moveScript.isActive = true;
        iteractionScript.isActiveIteraction = true;
    }
    /// <summary>
    /// unlock viewbody,dispersionIndicator and bodyrotateforview
    /// </summary>
    public void unlockRotate()
    {
        weaponDispersion.enabled = true;
        viewBodyScript.enabled = true;
    }
    public void setFullAnim()
    {
        ermakAnim.SetLayerWeight(ermakAnim.GetLayerIndex("arms"), 0);
        stopPickUpAnim();
    }
    public void stopPickUpAnim()
    {
        StopCoroutine(pickUpItemAnim());
        StopCoroutine(smoothPickUpLayerWeight());
        if (ermakInventory.selectedWeaponIndex != 0 && ermakInventory.EquippedWeapons.weaponsItem.parentObj == 1)
        {
            ermakSelectedWeapon.parentObj = hands[ermakInventory.EquippedWeapons.weaponsItem.parentObj].transform;
            iteractionScript.selectedWeaponScript.transform.localPosition = ermakInventory.EquippedWeapons.weaponsItem.positionOnSelected;
            iteractionScript.selectedWeaponScript.transform.localRotation = ermakInventory.EquippedWeapons.weaponsItem.rotationOnSelected;
        }
        ermakAnim.SetLayerWeight(ermakAnim.GetLayerIndex("interaction"), 0);
    }
    public void stopPickUpAnim(float speedModifier)
    {
        StopCoroutine(pickUpItemAnim());
        StopCoroutine(smoothPickUpLayerWeight());
        StartCoroutine(smoothPickUpLayerWeight(false, speedModifier));
    }
    public void setSeparratedAnim()
    {
        ermakAnim.SetLayerWeight(ermakAnim.GetLayerIndex("arms"), 1);
    }
    public void isBreakWeapon(float rotation)
    {
        getStunned(rotation, 1);
    }
    public void getSimpleStunned(float rotation)
    {
        getStunned(rotation, 2);
    }
    private void getStunned(float rotation, int stunType)
    {
        ermakAnim.SetInteger("stan", stunType);
        viewBodyScript.ermakBody.transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        weaponDispersion.gameObject.SetActive(false);
    }
    private IEnumerator smoothPickUpLayerWeight(bool isIncrease=true,float speedModifier = 1)
    {
        float animSpeed = Gubernia502.constData.ermakPickUpItemAnimSpeed;
        if (isIncrease)
        {
            if (ermakInventory.selectedWeaponIndex != 0 && ermakInventory.EquippedWeapons.weaponsItem.parentObj == 1  )
            {
                ermakSelectedWeapon.parentObj = hands[0].transform;
                iteractionScript.selectedWeaponScript.transform.localPosition = 
                    ermakInventory.EquippedWeapons.weaponsItem.secondEquipedPos.positionOnSelected;
                iteractionScript.selectedWeaponScript.transform.localRotation = 
                    ermakInventory.EquippedWeapons.weaponsItem.secondEquipedPos.rotationOnSelected;
            }
            for (float weightStart= ermakAnim.GetLayerWeight(ermakAnim.GetLayerIndex("interaction"));
                weightStart < 1; weightStart += animSpeed/100 * speedModifier)
            {
                if (weightStart + (animSpeed/100 * speedModifier) > 1)
                {
                    weightStart = 1;
                }
                ermakAnim.SetLayerWeight(ermakAnim.GetLayerIndex("interaction"), weightStart);
                yield return null;
            }
        }
        else
        {
            for (float weightStart = ermakAnim.GetLayerWeight(ermakAnim.GetLayerIndex("interaction"));
                weightStart > 0; weightStart -= animSpeed/100 * speedModifier)
            {
                if(weightStart+(animSpeed/100*speedModifier)<0)
                {
                    weightStart = 0;
                }
                ermakAnim.SetLayerWeight(ermakAnim.GetLayerIndex("interaction"), weightStart);
                yield return null;
            }
            if (ermakInventory.selectedWeaponIndex != 0 && ermakInventory.EquippedWeapons.weaponsItem.parentObj == 1)
            {
                ermakSelectedWeapon.parentObj = hands[ermakInventory.EquippedWeapons.weaponsItem.parentObj].transform;
                iteractionScript.selectedWeaponScript.transform.localPosition = 
                    ermakInventory.EquippedWeapons.weaponsItem.positionOnSelected;
                iteractionScript.selectedWeaponScript.transform.localRotation = 
                    ermakInventory.EquippedWeapons.weaponsItem.rotationOnSelected;
            }
        }
        yield break;
    }
    private IEnumerator pickUpItemAnim()
    {
        StartCoroutine(smoothPickUpLayerWeight());
        yield return new WaitForSeconds(Gubernia502.constData.ermakPickUpItemAnimDelay);
        StartCoroutine(smoothPickUpLayerWeight(false));
        yield break;
    }
    public void pickUpItem()
    {
        StopAllCoroutines();
        StartCoroutine(pickUpItemAnim());
    }
    private void Awake()
    {
        bodyRotationSpeed = Gubernia502.constData.ermakBodyRotationSpeed;
    }
}
