using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class saveLoader : MonoBehaviour
{
    IEnumerator loadScene()
    {
        Gubernia502.gameIsActive = false;
        saveSystem.loadStatus = true;
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("emptyLevel");
        while (!op.isDone)
        {
            yield return null;
        }
        saveSystem.loadStatus = false;
        saveSystem.loadSave();
        Destroy(gameObject);
        yield break;
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(loadScene());
    }
}
