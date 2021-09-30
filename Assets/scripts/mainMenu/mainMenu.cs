using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour//singltone
{
    public bool isActive = true;
    static mainMenu singltone = null;
    int selectedButtonIndex;
    [SerializeField]
    Text[] mainMenuButtons;//кнопки назначаются сверху вниз
    [SerializeField]
    GameObject buttonSelector;
    Text selectedButton;
    Vector3 selectorNeededPos;
    [SerializeField]
    Text loadingStateInfo;
    public Animator animator;
    public void selectButton(Text button)
    {
        if (button != selectedButton&&isActive)
        {
            selectedButton.color = new Color(0.5f,0.5f,0.5f,1);
            selectButtonAction(button);
        }
    }
    private void selectButtonAction(Text button)
    {
        if (isActive)
        {
            button.color = Color.white;
            selectorNeededPos = button.transform.position;
            enabled = true;
            selectedButtonIndex = System.Array.IndexOf(mainMenuButtons, button);
            selectedButton = button;
        }
    }
    public void pressPlay()
    {
        if (isActive)
        {
            DontDestroyOnLoad(gameObject);
            loadingStateInfo.gameObject.SetActive(true);
            animator.SetTrigger("goToLoad");
            saveSystem.loadMap();
        }
    }
    public void pressQuit()
    {
        if (isActive)
        {
            Application.Quit();
        }
    }
    public void pressFAQ()
    {

    }
    public void selectButtonUp()
    {
        if (selectedButtonIndex != 0)
        {
            selectButton(mainMenuButtons[selectedButtonIndex - 1]);
        }
    }
    public void selectButtonDown()
    {
        if (selectedButtonIndex != mainMenuButtons.Length - 1)
        {
            selectButton(mainMenuButtons[selectedButtonIndex + 1]);
        }
    }
    public void updateLoadingInfo(saveLoader.loadState state,int isLoaded=0,int needToLoad=0)
    {
        loadingStateInfo.text = (state.ToString() + "( " + isLoaded + "  /  " + needToLoad + " )");
    }
    public void destroyMainMenu()
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        float dist = Vector3.Distance(buttonSelector.transform.position, selectorNeededPos);
        if (dist >Gubernia502.constData.mainMenuSelectorDeadZone)
        {
            buttonSelector.transform.position += Vector3.Normalize(selectorNeededPos - buttonSelector.transform.position)
                *Gubernia502.constData.mainMenuSelectorSpeed * Time.fixedDeltaTime;
        }
        else
        {
            buttonSelector.transform.position=selectorNeededPos;
            enabled = false;
        }
    }
    private void Awake()
    {
        if (singltone == null)
        {
            singltone = this;
            selectButtonAction(mainMenuButtons[0]);
            Gubernia502.mainMenu = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
