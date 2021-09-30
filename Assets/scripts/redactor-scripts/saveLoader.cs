using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading;

public class saveLoader : MonoBehaviour//singltone
{
    static saveLoader singltone;
    bool isStarting = false;
    public enum loadState
    {
        loadInit,
        mainHeroLoad,
        wallsLoad,
        enemiesLoad,
        weaponsLoad,
        itemsLoad,
        loadEnd
    }
    public AutoResetEvent waitHandler { get; private set; }
    public loadState currentState = loadState.loadEnd;
    IEnumerator loadScene()
    {
        Gubernia502.gameIsActive = false;
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("emptyLevel");
        while (!op.isDone)
        {
            yield return null;
        }
        saveSystem.loadStatus = true;
        new Thread(new ThreadStart(loadLevelAsync)).Start();
        while (saveSystem.loadStatus)
        {
            yield return null;
        }
        Gubernia502.mainMenu.animator.SetTrigger("goToLoad");
        Gubernia502.gameIsActive = true;
        Destroy(gameObject);
        yield break;
    }
    IEnumerator loadMainMenu()
    {
        Gubernia502.gameIsActive = false;
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("mainMenu");
        while (!op.isDone)
        {
            yield return null;
        }
        Gubernia502.gameIsActive = true;
        Destroy(gameObject);
        yield break;
    }
    void loadLevelAsync()
    {
        saveSystem.loadSaveAsync(this);
    }
    public void startLoad(bool isLoadSave=true)
    {
        if (!isStarting)
        {
            DontDestroyOnLoad(gameObject);
            waitHandler = new AutoResetEvent(true);
            if (isLoadSave)
            {
                StartCoroutine(loadScene());
            }
            else
            {
                StartCoroutine(loadMainMenu());
            }
        }
    }
    private void Awake()
    {
        if (singltone == null)
        {
            singltone = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
