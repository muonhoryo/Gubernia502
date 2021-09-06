using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeWeaponPropertie : MonoBehaviour
{
    public Button selectedButton;
    public mapRedactor redactor;
    public int changingPropertieNum = 0;
    protected static readonly string str = "0123456789";
    protected virtual void closeChanger()
    {
        redactor.propertiesFields[changingPropertieNum].text=
            redactor.propertiesFields[changingPropertieNum].text.Remove(redactor.propertiesFields[changingPropertieNum].text.IndexOf("_"));
        if (redactor.propertiesFields[changingPropertieNum].text.Length==0)
        {
            redactor.propertiesFields[changingPropertieNum].text = "0";
        }
        else
        {
            int i= int.Parse(redactor.propertiesFields[changingPropertieNum].text);
            switch (changingPropertieNum)
            {
                case 0://id
                    if (i > 20||i<15)
                    {
                        i = 15;
                    }
                    break;
                case 1://dmgBuff
                    if (i > weaponsItem.maxDamageBuff)
                    {
                        i = weaponsItem.maxDamageBuff;
                    }
                    break;
                case 2://speedBuff
                    if (i > weaponsItem.maxSpeedBuff)
                    {
                        i = weaponsItem.maxSpeedBuff;
                    }
                    break;
                case 3://magSizeBuff
                    if (i > weaponsItem.maxMagSizeBuff)
                    {
                        i = weaponsItem.maxMagSizeBuff;
                    }
                    break;
                case 4://accuracyBuff
                    if (i > weaponsItem.maxAccuracyBuff)
                    {
                        i = weaponsItem.maxAccuracyBuff;
                    }
                    break;
                case 5://durabilityBuff
                    if (i > weaponsItem.maxDurabilityBuff)
                    {
                        i = weaponsItem.maxDurabilityBuff;
                    }
                    break;
                case 7://ammoId
                    if (i > 8)
                    {
                        i =8;
                    }
                    break;
            }
            redactor.propertiesFields[changingPropertieNum].text = i.ToString();
        }
        redactor.enableRedactor();
        Destroy(this);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            closeChanger();
        }
        if (Input.GetKeyDown(KeyCode.Backspace)&& redactor.propertiesFields[changingPropertieNum].text.Length>1)
        {
            redactor.propertiesFields[changingPropertieNum].text = redactor.propertiesFields[changingPropertieNum].text.Remove(
                redactor.propertiesFields[changingPropertieNum].text.Length - 2,1);
        }
        if (Input.anyKeyDown)
        {
            if (str.Contains(Input.inputString)&&Input.inputString!="")
            {
                redactor.propertiesFields[changingPropertieNum].text=redactor.propertiesFields[changingPropertieNum].text.Insert
                    (redactor.propertiesFields[changingPropertieNum].text.Length-1,
                    Input.inputString);
            }
            if (redactor.propertiesFields[changingPropertieNum].text.Length >= 10)
            {
                closeChanger();
            }
        }
    }
}
