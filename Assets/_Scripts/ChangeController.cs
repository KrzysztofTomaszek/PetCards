using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeController : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;

    private void Awake()
    {
        SetPlayerName();
        if(!Globals.activePlayer && Globals.activeRound == 0)
        {
            InicializeCardsSet();
        }
            
    }

    void SetPlayerName()
    {
        int playerNum=0;
        switch(Globals.roundPhase)
        {
            case 0:
                playerNum = 0;
                break;
            case 1:
                playerNum = 1;
                break;
            case 2:
                if(Globals.activePlayer)
                    playerNum = 1;
                else
                    playerNum = 0;
                break;
        }
        playerName.text = Globals.playerName[playerNum];
    }

    void InicializeCardsSet()
    {
       if((int)Random.Range(0f, 1.9f) > 0)
          Globals.activePlayer = true;
       else
          Globals.activePlayer = false;

        for(int card = 0;card <= 11;card++)
        {            
            Globals.leftSet[card] = TakeCard();
            Globals.rightSet[card] = TakeCard();
        }
        Globals.activeRound = 1;
    }


    int TakeCard()
    {
        int card = 0;
        int index = (int)Random.Range(0.1f, 29.8f);

        if(Globals.cardSet[index] == 0)
        {
            return TakeCard();
        }
        else
        {
            card = Globals.cardSet[index];
            Globals.cardSet[index] = 0;
            return card;            
        }
    }  
    
}
