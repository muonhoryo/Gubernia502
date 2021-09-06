using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class debugConsole : MonoBehaviour
{
    public GameObject debugConsolePanel;
    public Text debugConsoleText;
    public mapRedactor redactor;
    public GameObject xRayField;
    private void OnEnable()
    {
        debugConsolePanel.SetActive(true);
        debugConsoleText.text = null;
    }
    private void closedDebugConsole()
    {
                debugConsoleText.text = null;
                enabled = false;
        debugConsolePanel.SetActive(false);
        Gubernia502.playerController.ermakLockControl.unlockCtrl();
        Gubernia502.playerController.ermakLockControl.unlockRotate();
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                string command;
                if (debugConsoleText.text.Contains("_"))
                {
                    command = debugConsoleText.text.Substring(0, debugConsoleText.text.IndexOf("_"));
                }
                else
                {
                    command = debugConsoleText.text;
                }
                switch (command)
                {
                    case "killGG":
                        Gubernia502.playerController.ermakLockControl.hpSystem.takeNormalDamage(10000, 0, 0f,Vector3.zero);
                        Gubernia502.playerController.ermakLockControl.hpSystem.takeNormalDamage(10000, 0, 0f,Vector3.zero);
                        break;
                    case "takeDmg":
                        command = debugConsoleText.text.Substring(debugConsoleText.text.IndexOf("_") + 1);
                        command = command.Replace(" ", "");
                        if(int.TryParse(command,out int dmg) && dmg > 0)
                        {
                            Gubernia502.playerController.ermakLockControl.hpSystem.takeNormalDamage(dmg,0, 0, Vector3.zero);
                        }
                        break;
                    case "deleteWpn":
                        Gubernia502.playerController.ermakLockControl.iteractionScript.selectedWeaponScript.takeDurabilityDmg(
                            Gubernia502.playerController.ermakLockControl.ermakInventory.EquippedWeapons.durability + 1);
                        Gubernia502.playerController.ermakLockControl.isBreakWeapon(
                            Gubernia502.playerController.ermakLockControl.transform.rotation.eulerAngles.y);
                        break;
                    case "loadMap":
                        saveSystem.loadMap();
                        break;
                    case "saveMap":
                        saveSystem.saveMap();
                        break;
                    case "close":
                        Application.Quit();
                        break;
                    case "redactorMode":
                        command = debugConsoleText.text.Substring(debugConsoleText.text.IndexOf("_") + 1);
                        command = command.Replace(" ", "");
                        switch (command)
                        {
                            case "on":
                                redactor.enableRedactor();
                                Gubernia502.mainCamera.changeRedactorMode();
                                xRayField.SetActive(true);
                                break;
                            case "off":
                                redactor.disableRedactor();
                                Gubernia502.mainCamera.changeToDefaultTrack();
                                xRayField.SetActive(false);
                                break;
                        }
                        break;
                }
                closedDebugConsole();
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (debugConsoleText.text.Length > 0)
                {
                    debugConsoleText.text = debugConsoleText.text.Remove(debugConsoleText.text.Length - 1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                closedDebugConsole();
            }
            else
            {
                debugConsoleText.text += Input.inputString;
            }
        }
    }
}
