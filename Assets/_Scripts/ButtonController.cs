using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] ChooseController chooseController;
    [SerializeField] FightController fightController;
    [SerializeField] ScoreController scoreController;
    [SerializeField] SetPlayerNickController SetPlayerNickController;
    [SerializeField] private AudioClip buttonClickSfx;

    public void ExitApp()
    {
        Application.Quit();
        AudioController.instance.PlaySfx(buttonClickSfx);
    }

    public void GoToScene(string sceneName)
    {        
        SceneManager.LoadScene(sceneName);
        AudioController.instance.PlaySfx(buttonClickSfx);
    }

    public void ActiveButton(GameObject button)
    {
        button.GetComponent<Image>().color = Color.white;
        button.GetComponent<Button>().interactable = true;
    }

    public void DissactiveButton(GameObject button)
    {
        button.GetComponent<Image>().color = Color.gray;
        button.GetComponent<Button>().interactable = false;
    }

    public void GoToChangePlayer()
    {
        chooseController.InitializeSaveSet();
        Globals.activePlayer = !Globals.activePlayer;
        GoToScene("ChangePlayer");                      
    }

    public void ChangePlayerButton()
    {
        switch(Globals.roundPhase)
        {
            case 0:
                Globals.roundPhase = 1;
                GoToScene("CardsChoose");
                break;
            case 1:
                Globals.roundPhase = 2;
                GoToScene("CardsChoose");
                break;
            case 2:
                Globals.roundPhase = 3;
                GoToScene("CardsFight");
                break;
        }
    }

    public void NextRoundButton()
    {
        fightController.Summary();
        if(Globals.activeRound > 4)
            GoToScene("Scores");
        else
            GoToScene("ChangePlayer");
    }

    public void ContinueFightButton()
    {
        fightController.ContinueFight();
        AudioController.instance.PlaySfx(buttonClickSfx);
    }

    public void Skill1Button()
    {
        fightController.SeeCardSkill();
        AudioController.instance.PlaySfx(buttonClickSfx);
    }

    public void Skill2Button()
    {
        fightController.CompareCardsSkill();
        AudioController.instance.PlaySfx(buttonClickSfx);
    }

    public void Skill3Button()
    {
        fightController.BlockingTheftSkill();
        AudioController.instance.PlaySfx(buttonClickSfx);
    }

    public void Skill4Button()
    {
        fightController.ScoreX2Skill();
        AudioController.instance.PlaySfx(buttonClickSfx);
    }

    public void GoToStartButton()
    {
        scoreController.ResetToDefault();
        GoToScene("Start");
    }

    public void SetNickButton()
    {
        SetPlayerNickController.SetNicks();
        GoToScene("ChangePlayer");
    }

    public void LoadTutorial(string objToHide)
    {
        GameObject parentObject = GameObject.Find("Scene");
        FindObject(parentObject, "Tut").SetActive(true);
        FindObject(parentObject, "TutExt").SetActive(true);
        GameObject.Find(objToHide).SetActive(false); 
        AudioController.instance.PlaySfx(buttonClickSfx);
        
    }

    public void DestroyTutorial(string objToUnhide)
    {
        GameObject parentObject = GameObject.Find("Scene");
        FindObject(parentObject, objToUnhide).SetActive(true);
        GameObject.Find("Tut").SetActive(false);
        GameObject.Find("TutExt").SetActive(false);
        AudioController.instance.PlaySfx(buttonClickSfx);
    }

    static GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach(Transform t in trs)
        {
            if(t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
