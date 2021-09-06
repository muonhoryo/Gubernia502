using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class serednyakInventory : ermakInventory
{
    float stateToSelectDurIndic = 0.3f;
    const float ammoGroupPlaceStep =85f;

    class altAmmoCounter
    {
        public altAmmoCounter(ammoGroup group,int id)
        {
            ammoGroup = group;
            this.id = id;
        }
        public ammoGroup ammoGroup { get; private set; }
        public int id { get; private set; }
    }
    [SerializeField]
    GameObject playerInterfaceCanvas;
    [SerializeField]
    Image weaponDurMonitor;
    [SerializeField]
    Image weaponDurIndicator;
    [SerializeField]
    Image weaponCounter;
    [SerializeField]
    Text selectedWeaponText;
    [SerializeField]
    Text equippedWeaponsText;
    [SerializeField]
    Image ammoCounter;
    [SerializeField]
    Text ammoInMagCounter;
    [SerializeField]
    Text magSizeMonitor;
    [SerializeField]
    Text inventoryAmmoCounter;
    [SerializeField]
    GameObject ammmoGroupIndicPref;
    List<altAmmoCounter> altAmmoCounters = new List<altAmmoCounter> { };
    IEnumerator hideWeaponCoroutine;
    IEnumerator hideAmmoCoroutine;
    IEnumerator hideWeaponGui()
    {
        yield return new WaitForSeconds(Gubernia502.constData.hideGuiDelayTime);
        weaponCounter.gameObject.SetActive(false);
        weaponDurMonitor.gameObject.SetActive(false);
        weaponDurIndicator.gameObject.SetActive(false);
        yield break;
    }
    IEnumerator hideAmmoGui()
    {
        yield return new WaitForSeconds(Gubernia502.constData.hideGuiDelayTime);
        ammoCounter.gameObject.SetActive(false);
        if (altAmmoCounters.Count > 0)
        {
            for(int i = 0; i < altAmmoCounters.Count; i++)
            {
                altAmmoCounters[i].ammoGroup.gameObject.SetActive(false);
            }
        }
        yield break;
    }
    private int indexOfId(int id)
    {
        if (altAmmoCounters.Count > 0)
        {
            for(int i = 0; i < altAmmoCounters.Count; i++)
            {
                if (altAmmoCounters[i].id == id)
                {
                    return i;
                }
            }
        }
        return -1;
    }
    private int indexOfId(int id,out int index)
    {
        index = indexOfId(id);
        return index;
    }
    private void showWeaponPanel()
    {
        showWeaponPanel(selectedWeaponIndex);
    }
    private void showWeaponPanel(int slot)
    {
        weaponCounter.gameObject.SetActive(true);
        showWeaponDurIndicator(slot);
    }
    private void showWeaponDurIndicator()
    {
        showWeaponDurIndicator(selectedWeaponIndex);
    }
    private void showWeaponDurIndicator(int slot)
    {
        weaponDurMonitor.gameObject.SetActive(true);
        if (slot > 0)
        {
            weaponDurIndicator.gameObject.SetActive(true);
        }
        else
        {
            weaponDurIndicator.gameObject.SetActive(false);
        }
        showAmmoIndicator(slot);
        StopCoroutine(hideWeaponCoroutine);
        hideWeaponCoroutine = hideWeaponGui();
        StartCoroutine(hideWeaponCoroutine);
    }
    private void showAmmoIndicator(int slot)
    {
        if (slot > 0 && equippedWeapons[slot - 1].weaponsItem.isDistant)
        {
            ammoCounter.gameObject.SetActive(true);
        }
        else
        {
            ammoCounter.gameObject.SetActive(false);
        }
        if (altAmmoCounters.Count > 0)
        {
            for(int i = 0; i < altAmmoCounters.Count; i++)
            {
                altAmmoCounters[i].ammoGroup.gameObject.SetActive(true);
            }
        }
        StopCoroutine(hideAmmoCoroutine);
        hideAmmoCoroutine = hideAmmoGui();
        StartCoroutine(hideAmmoCoroutine);
    }
    private void showAmmoIndicator()
    {
        showAmmoIndicator(selectedWeaponIndex);
    }
    public override void selectWeapon(int slot)
    {
        base.selectWeapon(slot);
        showWeaponPanel();
        if (altAmmoCounters.Count > 0)
        {
            for (int i = 0; i < altAmmoCounters.Count; i++)
            {
                Destroy(altAmmoCounters[i].ammoGroup.gameObject);
            }
            altAmmoCounters = new List<altAmmoCounter> { };
        }
        if (slot > 0)
        {
            weaponDurIndicator.material.SetFloat("fullness",
                ((float)EquippedWeapons.durability / EquippedWeapons.maxDurability) + 0.01f);
            weaponDurIndicator.material.SetTexture("sprite", EquippedWeapons.simpleItem.icon);
            weaponDurIndicator.material.SetTexture("addedSprite", EquippedWeapons.simpleItem.icon);
            showAmmoIndicator();
            if (EquippedWeapons.weaponsItem.isDistant)
            {
                ammoInMagCounter.text = EquippedWeapons.ammoInMag.ToString();
                magSizeMonitor.text = EquippedWeapons.maxAmmoInMag.ToString();
                inventoryAmmoCounter.text = ammoInInventory[EquippedWeapons.currentAmmoId - 1].count.ToString();
                if (EquippedWeapons.weaponsItem.ammotTypes.Count > 0)
                {
                    for(int i = 0; i < EquippedWeapons.weaponsItem.ammotTypes.Count; i++)
                    {
                        ammoGroup group = Instantiate(ammmoGroupIndicPref, playerInterfaceCanvas.transform).GetComponent<ammoGroup>();
                        altAmmoCounters.Add(new altAmmoCounter(group, EquippedWeapons.weaponsItem.ammotTypes[i]));
                        group.GetComponent<RectTransform>().Translate(new Vector3(0, ammoGroupPlaceStep * i, 0));
                        Texture text = Gubernia502.constData.ammoInfo[EquippedWeapons.weaponsItem.ammotTypes[i]-1].icon;
                        group.groupIcon.sprite =Sprite.Create((Texture2D)text,new Rect(Vector2.zero,
                            new Vector2(text.width,text.height)),Vector2.one * 0.5f);
                        if (EquippedWeapons.weaponsItem.ammotTypes[i] == EquippedWeapons.currentAmmoId)
                        {
                            group.ammoCounter.gameObject.SetActive(false);
                            group.groupIcon.color = Gubernia502.activeColor;
                        }
                        else
                        {
                            group.ammoCount.text = ammoInInventory[EquippedWeapons.weaponsItem.ammotTypes[i]-1].count.ToString();
                        }
                    }
                }
            }
        }
        selectedWeaponText.text = slot.ToString();
    }
    public override void addWeapon(collectibleWeaponsItem weapon)
    {
        base.addWeapon(weapon);
        showWeaponPanel();
        equippedWeaponsText.text = equippedWeapons.Count.ToString();
    }
    public override void deleteWeapon(int slot)
    {
        base.deleteWeapon(slot);
        showWeaponPanel();
        equippedWeaponsText.text = equippedWeapons.Count.ToString();
    }
    public override bool dmgSelectedWeapon(int dmg = 1)
    {
        {
            float a = ((float)(EquippedWeapons.durability - dmg)) / EquippedWeapons.maxDurability;
        weaponDurIndicator.material.SetFloat("fullness",a + 0.01f);
            if (a <= stateToSelectDurIndic)
            {
                showWeaponDurIndicator();
            }
        }
        return base.dmgSelectedWeapon(dmg);
    }
    public override void addItem(collectibleSimpleItem item)
    {
        int itemId = item.item.id;
        base.addItem(item);
        if (selectedWeaponIndex!=0 )
        {
            if(itemId == EquippedWeapons.currentAmmoId)
            {
                inventoryAmmoCounter.text = ammoInInventory[itemId - 1].count.ToString();
                showAmmoIndicator();
            }
            else if(indexOfId(itemId,out int index) != -1)
            {
                altAmmoCounters[index].ammoGroup.ammoCount.text = ammoInInventory[itemId - 1].count.ToString();
            }
        }
    }
    public override void loadAmmo(int loadingCount, int id)
    {
        base.loadAmmo(loadingCount, id);
        ammoInMagCounter.text = EquippedWeapons.ammoInMag.ToString();
        inventoryAmmoCounter.text = ammoInInventory[id - 1].count.ToString();
        showAmmoIndicator();
    }
    public override void deleteOneAmmo()
    {
        base.deleteOneAmmo();
        ammoInMagCounter.text = EquippedWeapons.ammoInMag.ToString();
        showAmmoIndicator();
    }
    public override void unloadWeapon()
    {
        base.unloadWeapon();
        ammoInMagCounter.text = EquippedWeapons.ammoInMag.ToString();
        inventoryAmmoCounter.text = ammoInInventory[EquippedWeapons.currentAmmoId - 1].count.ToString();
        showAmmoIndicator();
    }
    public override void nextAmmoType()
    {
        {
            altAmmoCounters[indexOfId(EquippedWeapons.currentAmmoId, out int indexOf)].ammoGroup.ammoCount.text =
                ammoInInventory[EquippedWeapons.currentAmmoId - 1].count.ToString();
            altAmmoCounters[indexOf].ammoGroup.ammoCounter.gameObject.SetActive(true);
            altAmmoCounters[indexOf].ammoGroup.groupIcon.color = Gubernia502.noneActiveColor;
        }
        base.nextAmmoType();
        {
            altAmmoCounters[indexOfId(EquippedWeapons.currentAmmoId, out int indexOf)].ammoGroup.ammoCounter.gameObject.SetActive(false);
            altAmmoCounters[indexOf].ammoGroup.groupIcon.color = Gubernia502.activeColor;
            inventoryAmmoCounter.text = ammoInInventory[EquippedWeapons.currentAmmoId - 1].count.ToString();
        }
    }
    private void Awake()
    {
        hideWeaponCoroutine = hideWeaponGui();
        hideAmmoCoroutine = hideAmmoGui();
    }
}
