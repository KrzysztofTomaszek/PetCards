using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseController : MonoBehaviour
{
    bool player;
    string[] tempChoose = new string[3];
    int[] cardSet = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    [SerializeField] float cardUp;
    [SerializeField] float transitionSpeed;
    [SerializeField] GameObject button;
    [SerializeField] ButtonController buttonController;
    [SerializeField] private AudioClip moveCardSfx;
    [SerializeField] private AudioClip drawCardSfx;
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if( hit.collider.name.Substring(0,4) == "Card")
            {
                ChooseCard(hit.collider.gameObject);
            }
        }
        
        if(IfAllSetChecked())
            buttonController.ActiveButton(button);
        else
            buttonController.DissactiveButton(button);
    }

    private void Awake()
    {
        SetPlayerWhoChoose();
        CopySet();
        DrawCards();
        DisableEmptyObject();
        buttonController.DissactiveButton(button);        
    }

    void SetPlayerWhoChoose()
    {
        switch(Globals.roundPhase)
        {
            case 1:
                player = false;   
                break;
            case 2:
                player = true;  
                break;
        }
    }

    void CopySet()
    {
        for(int i = 0;i < 12;i++)
        {
            if(player)
                cardSet[i] = Globals.rightSet[i];
            else
                cardSet[i] = Globals.leftSet[i];
        }
    }

    void DrawCards()
    {
        for(int i = 1;i <= 12;i++)
        {
            GameObject card = GameObject.Find("Card " + i);
            SpriteRenderer cardImg = card.GetComponent<SpriteRenderer>();
            switch(cardSet[i - 1])
            {
                case 0:
                    cardImg.sprite = null;
                    break;
                case 1:
                    cardImg.sprite = Globals.wolf;
                    break;
                case 2:
                    cardImg.sprite = Globals.hedgehog;
                    break;
                case 3:
                    cardImg.sprite = Globals.raccoon;
                    break;
                case 4:
                    cardImg.sprite = Globals.bear;
                    break;
                case 5:
                    cardImg.sprite = Globals.frog;
                    break;
            }
        }
        AudioController.instance.PlaySfx(drawCardSfx);
    }

    void ChooseCard(GameObject card)
    {
        if(IfCardChecked(card.name))
        {
            CardDown(CardPositionInSet(card.name), card);
        }
        else if(EmptyIndex()!=-1)
        {
            CardUp(EmptyIndex(), card);
        }
        AudioController.instance.PlaySfx(moveCardSfx);
    }

    void CardUp(int indexTempChoose, GameObject card)
    {
        Vector3 newPos = new Vector3(card.transform.position.x, card.transform.position.y + cardUp, card.transform.position.z);
        card.GetComponent<ParticleSystem>().Play();
        card.transform.position = Vector3.Lerp(card.transform.position, newPos, transitionSpeed);
        tempChoose[indexTempChoose] = card.name;   
    }

    void CardDown(int indexTempChoose, GameObject card)
    {
        Vector3 newPos = new Vector3(card.transform.position.x, card.transform.position.y - cardUp, card.transform.position.z);
        card.GetComponent<ParticleSystem>().Stop();
        card.transform.position = Vector3.Lerp(card.transform.position, newPos, transitionSpeed);        
        tempChoose[indexTempChoose] = null;       
    }

    bool IfCardChecked(string cardName)
    {
        for(int i = 0;i < 3;i++)
        { 

            if(tempChoose[i] == cardName)
                return true;
        }
        return false;
    }

    int CardPositionInSet(string cardName)
    {
        for(int i=0;i<3;i++)
        {
            if(tempChoose[i] == cardName)
                return i;
        }
        return -1; //ERROR
    }

    int EmptyIndex()
    {
        for(int i = 0;i < 3;i++)
        {

            if(tempChoose[i] == null)
                return i;
        }
        return -1;
    }

    bool IfAllSetChecked()
    {
        if(EmptyIndex()==-1)
            return true;
        else
            return false;
    }

    public void InitializeSaveSet()
    {
        SaveSet(TempChooseToInt()); 
    }

    void SaveSet(int[] choosedCards)
    {
        for(int i = 0;i < 3;i++)
        {
            if(player)
            {
                Globals.rightActivSet[i] = choosedCards[i];
                TakeFromSet(choosedCards[i]);
            }                
            else
            {
                Globals.leftActivSet[i] = choosedCards[i];
                TakeFromSet(choosedCards[i]);
            }
                
        }
    }    

    int[] TempChooseToInt()
    {
        int index = 0;
        int[] tab = new int[3] {0,0,0};
        foreach(string card in tempChoose)
        {
            GameObject cardGO = GameObject.Find(card);
            string cardName = cardGO.GetComponent<SpriteRenderer>().sprite.name;
            switch(cardName)
            {
                case "wolf_card":
                    tab[index++] = 1;
                    break;
                case "hedgehog_card":
                    tab[index++] = 2;
                    break;
                case "raccoon_card":
                    tab[index++] = 3;
                    break;
                case "bear_card":
                    tab[index++] = 4;
                    break;
                case "frog_card":
                    tab[index++] = 5;
                    break;
            }
        }
        return tab;
    }

    void TakeFromSet(int card)
    {
        if(player)
        {
            for(int i =0;i<=11;i++)
            {
                if(Globals.rightSet[i] == card)
                {
                    Globals.rightSet[i] = 0;
                    break;
                }
            }
        }
        else
        {
            for(int i = 0;i <= 11;i++)
            {
                if(Globals.leftSet[i] == card)
                {
                    Globals.leftSet[i] = 0;
                    break;
                }
            }
        }
    }

    void DisableEmptyObject()
    {
        GameObject cards = GameObject.Find("Cards");
        foreach(SpriteRenderer card in cards.GetComponentsInChildren<SpriteRenderer>())
        {
            if (card.sprite is null)
                card.gameObject.SetActive(false);
        }
    }
}
