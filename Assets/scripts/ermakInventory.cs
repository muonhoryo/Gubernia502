using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermakInventory : MonoBehaviour
{
    public class weaponSlot
    {
        public weaponSlot(collectibleWeaponsItem weapon)
        {
            simpleItem = weapon.item;
            breakableItem = weapon.breakItem;
            weaponsItem = weapon.weaponItem;
            buffs = new int[]
            {
                weapon.quality,
                weapon.damageBuff,
                weapon.durabilityBuff,
                weapon.speedBuff,
                weapon.magSizeBuff,
                weapon.accuracyBuff
            };
            durability = weapon.durability;
            ammoInMag = weapon.ammoInMag;
            maxAmmoInMag = weaponsItem.magSize + weapon.magSizeBuff;
            currentAmmoId = weapon.ammoId;
            maxDurability = breakableItem.maxDurability + weapon.durabilityBuff;
            dmg = weaponsItem.damage + weapon.damageBuff;
            coolDownTime = weaponsItem.fireCoolDown - weapon.speedBuff;
            dispersion = weaponsItem.dispersion - weapon.accuracyBuff;
        }
        public simpleItem simpleItem { get; private set; }
        public breakableItem breakableItem { get; private set; }
        public weaponsItem weaponsItem { get; private set; }
        public int[] buffs { get; private set; }
        public int durability { get; private set; }
        public int ammoInMag { get; private set; }
        public int maxAmmoInMag { get; private set; }
        public int currentAmmoId { get; private set; }
        public int maxDurability { get; private set; }
        public int dmg { get; private set; }
        public int coolDownTime { get; private set; }
        public float dispersion { get; private set; }
        public void setDurability(int dur)
        {
            durability = dur;
        }
        public void setAmmoInMag(int ammo)
        {
            ammoInMag = ammo;
        }
        public void setAmmoId(int id)
        {
            currentAmmoId = id;
        }
    }
    public class ammoSlot
    {
        public ammoSlot(simpleItem item,int count)
        {
            this.count = count;
            simpleItem = item;
        }
        public int count;
        public simpleItem simpleItem;
    }
    public virtual int selectedWeaponIndex { get;protected set; } = 0;
    [SerializeField]
    private weapon Hand;
    public weapon hand { get=>Hand; private set=>Debug.LogError("trying to set hand"); }
    [SerializeField]
    protected ermakLockControl ermakLockControl;
    public weaponSlot EquippedWeapons
    {
        get
        {
            if (selectedWeaponIndex > 0)
            {
                return equippedWeapons[selectedWeaponIndex - 1];
            }
            else
            {
                Debug.LogError("trying to read parametres of hands");
                return null;
            }
        }
        private set { Debug.LogError("trying to set new equippedWeapon"); }
    }
    protected List<weaponSlot> equippedWeapons = new List<weaponSlot> { };
    protected ammoSlot[] ammoInInventory;
    public virtual void addItem(collectibleSimpleItem item)
    {
        if (item.item.id <= ammoInInventory.Length)
        {
            ammoInInventory[item.item.id - 1].count += item.count;
            Destroy(item.gameObject);
        }
        else
        {
            Debug.LogError("item with id=" + item.item.id + " does not exist");
        }
    }
    public void addBreakItem(collectibleBreakableItem item)
    {
        Debug.LogError("cannot add this item in inventory");
    }
    public virtual void addWeapon(collectibleWeaponsItem weapon)
    {
        equippedWeapons.Add(new weaponSlot(weapon));
        Destroy(weapon.gameObject);
    }
    public virtual void loadAmmo(int loadingCount,int id)
    {
            if (loadingCount > ammoInInventory[id - 1].count)
            {
                loadingCount = ammoInInventory[id - 1].count;
                ammoInInventory[id - 1].count = 0;
            }
            else
            {
                ammoInInventory[id - 1].count -= loadingCount;
            }
        EquippedWeapons.setAmmoInMag(EquippedWeapons.ammoInMag + loadingCount);
    }
    public virtual void deleteOneAmmo()
    {
        EquippedWeapons.setAmmoInMag(EquippedWeapons.ammoInMag - 1);
    }
    public virtual void deleteWeapon(int slot)
    {
        if (slot != 0 && slot <= equippedWeapons.Count)
        {
            equippedWeapons.RemoveAt(slot - 1);
        }
        else
        {
            Debug.LogError("weapon at " + slot + " slot does not exist or cannot be deleted");
        }
    }
    public void breakWeapon()
    {
        deleteWeapon(selectedWeaponIndex);
        selectWeapon(0);
    }
    public bool isHaveAmmo(int id)
    {
        if (id > 0 && id <= ammoInInventory.Length )
        {
            return ammoInInventory[id - 1].count > 0;
        }
        else
        {
            Debug.LogError("ammo with id=" + id + " does not exist");
            return false;
        }
    }
    public virtual void selectWeapon(int slot)
    {
        if (slot <= equippedWeapons.Count)
        {
            if (selectedWeaponIndex != 0)
            {
                Destroy(ermakLockControl.iteractionScript.selectedWeaponScript.gameObject);
            }
            else { hand.enabled = false; }
            selectedWeaponIndex = slot;
            if (slot!=0)
            {
                int slotIndex = slot - 1;
                ref weapon selectedWeaponScript = ref ermakLockControl.iteractionScript.selectedWeaponScript;
                selectedWeaponScript = Instantiate(equippedWeapons[slotIndex].weaponsItem.equippedWeaponPrefab,
                                           ermakLockControl.ermakSelectedWeapon.gameObject.transform).GetComponent<weapon>();//инициализация оружия и присвоения его в выбранное
                ermakLockControl.ermakSelectedWeapon.parentObj = 
                    ermakLockControl.hands[equippedWeapons[slotIndex].weaponsItem.parentObj].transform;//присвоение руки,которой держится оружие
                selectedWeaponScript.transform.localPosition += equippedWeapons[slotIndex].weaponsItem.positionOnSelected;//выставление позиции удержания оружия
                selectedWeaponScript.transform.localRotation = equippedWeapons[slotIndex].weaponsItem.rotationOnSelected;//выставление ротации удержания оружия
                selectedWeaponScript.ermakLockControl = ermakLockControl;
                ermakLockControl.ermakAnim.SetInteger("Weapon", equippedWeapons[slotIndex].weaponsItem.animWeaponModifier);
                if (equippedWeapons[slotIndex].weaponsItem.isDistant == false)
                {
                    ermakLockControl.weaponDispersion.gameObject.SetActive(false);
                    ermakLockControl.ermakAnim.SetBool("isDistant", false);
                    ermakLockControl.viewBodyScript.isMeleeWeapon = 1;
                    ermakLockControl.meleeFrontHitBox.hitDmg = equippedWeapons[slotIndex].dmg;
                }
                else
                {
                    ermakLockControl.weaponDispersion.gameObject.SetActive(true);
                    ermakLockControl.weaponDispersion.setDispersion(equippedWeapons[slotIndex].dispersion);
                    ermakLockControl.ermakAnim.SetBool("isDistant", true);
                    ermakLockControl.viewBodyScript.isMeleeWeapon = 0;
                }
                if (gameObject.tag == "MainHero")
                {
                    Gubernia502.playerController.changeAutoMode(equippedWeapons[slotIndex].weaponsItem.isAuto);
                }
            }
            else
            {
                if (gameObject.tag == "MainHero")
                {
                    Gubernia502.playerController.changeAutoMode(false);
                }
                ermakLockControl.weaponDispersion.gameObject.SetActive(false);
                ermakLockControl.ermakAnim.SetInteger("Weapon", 0);
                ermakLockControl.ermakAnim.SetBool("isDistant", false);
                hand.enabled = true;
                ermakLockControl.iteractionScript.selectedWeaponScript = hand;
                ermakLockControl.meleeFrontHitBox.hitDmg = Gubernia502.constData.ermakHandsDmg;
            }
        }
        else
        {
            Debug.LogError("selected slot in does not exist");
        }
    }
    public void selectNextWeapon()
    {
        if (selectedWeaponIndex >= equippedWeapons.Count)
        {
            selectWeapon(0);
        }
        else
        {
            selectWeapon(selectedWeaponIndex + 1);
        }
    }
    public void selectPreviousWeapon()
    {
        if (selectedWeaponIndex <= 0)
        {
            selectWeapon(equippedWeapons.Count);
        }
        else
        {
            selectWeapon(selectedWeaponIndex - 1);
        }
    }
    public virtual void unloadWeapon()
    {
        ammoInInventory[EquippedWeapons.currentAmmoId - 1].count += EquippedWeapons.ammoInMag;
        EquippedWeapons.setAmmoInMag(0);
    }
    public virtual void nextAmmoType()
    {
        if (EquippedWeapons.currentAmmoId == 
            EquippedWeapons.weaponsItem.ammotTypes[EquippedWeapons.weaponsItem.ammotTypes.Count - 1])
        {
            EquippedWeapons.setAmmoId(EquippedWeapons.weaponsItem.ammotTypes[0]);
        }
        else
        {
            EquippedWeapons.setAmmoId(EquippedWeapons.currentAmmoId + 1);
        }
    }
    public virtual bool dmgSelectedWeapon(int dmg=1)
    {
        EquippedWeapons.setDurability(EquippedWeapons.durability-dmg);
        if (EquippedWeapons.durability <= 0)
        {
            Instantiate(EquippedWeapons.weaponsItem.onDestroyEffect,
                transform.position, transform.rotation);
            breakWeapon();
            return true;
        }
        return false;
    }
    private void Start()
    {
        ammoInInventory = new ammoSlot[6]
        {
            new ammoSlot(Gubernia502.constData.ammoInfo[0],0),
            new ammoSlot(Gubernia502.constData.ammoInfo[1],0),
            new ammoSlot(Gubernia502.constData.ammoInfo[2],0),
            new ammoSlot(Gubernia502.constData.ammoInfo[3],0),
            new ammoSlot(Gubernia502.constData.ammoInfo[4],0),
            new ammoSlot(Gubernia502.constData.ammoInfo[5],0)
        };
    }
}
