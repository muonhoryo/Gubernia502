using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermakPlayerController : MonoBehaviour//singltone
{
    static ermakPlayerController singltone = null;
    private float selectWeaponCoolDown = 0.2f;

    private bool isCanSelectWeapon=true;
    public mainFieldOfView fieldOfView;
    public NPCLockControl NPCLockControl;
    public playerMeleeShoot meleeShoot;
    private float diagonalModifier;
    delegate void fire1();
    private fire1 fire;
    public GameObject exitMessage;
    IEnumerator waitToAddChoise()
    {
        yield return null;
        exitMessage.SetActive(true);
        gameObject.AddComponent<choiseMessage>();
        enabled = false;
        yield break;
    }
    private IEnumerator selectCoolDown(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        isCanSelectWeapon = true;
        yield break;
    }
    public void setNeededHeadDirection()
    {
        NPCLockControl.viewBodyScript.neededHeadDirection = Gubernia502.angleFromDirection(
            Gubernia502.mainCamera.cursorPos - transform.position);
    }
    public void changeAutoMode(bool isAuto)
    {
        if (isAuto)
        {
            fire = fireAuto;
        }
        else
        {
            fire = fireSingle;
        }
    }
    private void fireAuto()
    {
        if (Input.GetButton("_Fire1"))
        {
            NPCLockControl.iteractionScript.fire1();
        }
    }
    private void fireSingle()
    {
        if (Input.GetButtonDown("_Fire1"))
        {
            NPCLockControl.iteractionScript.fire1();
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            NPCLockControl.lockCtrl();
            Gubernia502.debugConsole.enabled = true;
        }
        if (Input.GetAxis("_Horizontal") != 0 || Input.GetAxis("_Vertical") != 0)
        {
            if (Input.GetAxis("_Horizontal") != 0 && Input.GetAxis("_Vertical") != 0)
            {
                diagonalModifier = 1 / Mathf.Sqrt(2);
            }
            else
            {
                diagonalModifier = 1;
            }
            NPCLockControl.moveScript.moveTraectory = new Vector3(Input.GetAxis("_Horizontal") * diagonalModifier,
                                                                                 0f,
                                                                                 Input.GetAxis("_Vertical") * diagonalModifier);
        }
    }
    private void Awake()
    {
        if (singltone == null)
        {
            singltone = this;
            saveSystem.mainHero = gameObject;
            Gubernia502.playerController = this;
            fire = fireSingle;
            enabled = Gubernia502.gameIsActive;
        }
        else
        {
            Destroy(this);
        }
    }
    private void LateUpdate()
    {
        if (!NPCLockControl.isLockedCtrl)
        {
            setNeededHeadDirection();
        }
        fieldOfView.generateAngle = NPCLockControl.viewBodyScript.transfmoredBody.transform.eulerAngles.y +
            (NPCLockControl.animator.GetFloat("HeadView") - 22.5f);
        if (Input.GetButtonDown("Cancel"))
        {
            StartCoroutine(waitToAddChoise());
        }
        if (NPCLockControl.iteractionScript.isActiveIteraction)
        {
            fire();
            if (Input.GetButtonDown("_Interaction"))
            {
                NPCLockControl.iteractionScript.firstIteraction();
            }
            if (Input.GetButtonDown("_Reload"))
            {
                NPCLockControl.iteractionScript.reload();
            }
            if (Input.GetButtonDown("_ChangeAmmo"))
            {
                NPCLockControl.iteractionScript.changeAmmo();
            }
            if (Input.GetButtonDown("_ChangeFireMode"))
            {
                NPCLockControl.iteractionScript.changeFireMode();
            }
            if (isCanSelectWeapon&&!NPCLockControl.animator.GetBool("reload"))
            {
                if (Input.GetButton("_NextWeapon"))
                {
                    NPCLockControl.Inventory.selectNextWeapon();
                    isCanSelectWeapon = false;
                    StartCoroutine(selectCoolDown(selectWeaponCoolDown));
                }
                if (Input.GetButton("_PrevWeapon"))
                {
                    NPCLockControl.Inventory.selectPreviousWeapon();
                    isCanSelectWeapon = false;
                    StartCoroutine(selectCoolDown(selectWeaponCoolDown));
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("_Fire1")&&NPCLockControl.animator.GetInteger("punchNum") != 0)
            {
                meleeShoot.isNextAttack = true;
            }
        }
    }
}



