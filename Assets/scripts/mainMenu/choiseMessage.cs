using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choiseMessage : MonoBehaviour
{
    IEnumerator waitToClose()
    {
        yield return null;
        Gubernia502.playerController.enabled = true;
        Gubernia502.playerController.exitMessage.SetActive(false);
        Destroy(this);
        yield break;
    }
    private void LateUpdate()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            StartCoroutine(waitToClose());
        }
        if (Input.GetButtonDown("Submit"))
        {
            saveSystem.loadMainMenu();
        }
    }
}
