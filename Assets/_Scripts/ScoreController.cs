using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI winnerTMP;
    [SerializeField] TextMeshProUGUI score1TMP;
    [SerializeField] TextMeshProUGUI score2TMP;
    int winner = 0;//0 - nie ustawiono, 1- wygrał gracz pierwszy/lewy, 2-wygrał gracz drugi/prawy, 3-remis
    [SerializeField] private AudioClip victorySfx;
    
    void Awake()
    {
        WhoWin();
        SetText();
        AudioController.instance.PlaySfx(victorySfx);
    }

    void WhoWin()
    {
        if(Globals.leftScore > Globals.rightScore)
        {
            winner = 1;
        }
        else if(Globals.leftScore < Globals.rightScore)
        {
            winner = 2;
        }
        else if(Globals.leftScore == Globals.rightScore)
        {
            winner = 3;
        }
    }

    void SetText()
    {
        SetPlayersScore();       
        SetWinnerText();
    }

    void SetPlayersScore()
    {
        score1TMP.text = "Score " + Globals.playerName[0] + ": " + Globals.leftScore.ToString();
        ;
        score2TMP.text = "Score " + Globals.playerName[1] + ": " + Globals.rightScore.ToString();
    }

    void SetWinnerText()
    {
        switch(winner)
        {
            case 0:
                winnerTMP.text = "Error";
                break;
            case 1:
                winnerTMP.text = "Winner is " + Globals.playerName[0];
                break;
            case 2:
                winnerTMP.text = "Winner is " + Globals.playerName[1];
                break;
            case 3:
                winnerTMP.text = "Draw";
                break;
        }
    }

    public void ResetToDefault()
    {
        Globals.activePlayer = false;
        Globals.leftScore = 0;
        Globals.rightScore = 0;
        Globals.cardSet = new int[30] { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 }; //Tablica z wszystkimi kartami na strcie
        Globals.leftSet = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        Globals.rightSet = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        Globals.leftActivSet = new int[3] { 0, 0, 0 };
        Globals.rightActivSet = new int[3] { 0, 0, 0 };
        Globals.rightSkillSet = new bool[4] { true, true, true, true };
        Globals.leftSkillSet = new bool[4] { true, true, true, true };
        Globals.activeRound = 0;
        Globals.roundPhase = 0;
    }
}
