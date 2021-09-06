using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeWallPropertie : changeWeaponPropertie
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
                case 0://phase
                    if (i < 1 || i >2)
                    {
                        i = 1;
                    }
                    break;
                case 1://HP
                    if (i > takingDamageObjData.globalMaxHPpoint)
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
