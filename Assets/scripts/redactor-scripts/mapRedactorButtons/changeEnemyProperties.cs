using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeEnemyProperties : changeWeaponPropertie
{
    protected override void closeChanger()
    {
        redactor.propertiesFields[changingPropertieNum].text =
            redactor.propertiesFields[changingPropertieNum].text.Remove(redactor.propertiesFields[changingPropertieNum].text.IndexOf("_"));
        if (redactor.propertiesFields[changingPropertieNum].text.Length == 0)
        {
            redactor.propertiesFields[changingPropertieNum].text = "0";
        }
        else
        {
            int i = int.Parse(redactor.propertiesFields[changingPropertieNum].text);
            switch (changingPropertieNum)
            {
                case 0://shieldDurability
                    if (i < 0 ||i>takingDamageObjData.globalMaxShieldDurability)
                    {
                        i = 0;
                    }
                    break;
                case 1://HPpoint
                    if (i <1|| i > takingDamageObjData.globalMaxHPpoint)
                    {
                        i = 1;
                    }
                    break;
                default:
                    i = 0;
                    break;
            }
            redactor.propertiesFields[changingPropertieNum].text = i.ToString();
        }
        redactor.enableRedactor();
        Destroy(this);
    }
}
