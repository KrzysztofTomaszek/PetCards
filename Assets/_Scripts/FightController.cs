using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FightController: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScorePlayer1TMP;
    [SerializeField] TextMeshProUGUI ScorePlayer2TMP;
    [SerializeField] TextMeshProUGUI RoundTMP;
    [SerializeField] TextMeshProUGUI ResultTMP;
    [SerializeField] TextMeshProUGUI ActualScoreTMP;
    [SerializeField] TextMeshProUGUI ActivePlayerTMP;
    [SerializeField] GameObject skill1B;
    [SerializeField] GameObject skill2B;
    [SerializeField] GameObject skill3B;
    [SerializeField] GameObject skill4B;
    [SerializeField] GameObject continueB;
    [SerializeField] GameObject nextRoundB;
    [SerializeField] Material DustWin;
    [SerializeField] Material DustLose;
    [SerializeField] Material DustDraw;
    [SerializeField] Material FlareWin;
    [SerializeField] Material FlareLose;
    [SerializeField] Material FlareDraw;
    [SerializeField] ButtonController buttonController;
    [SerializeField] private AudioClip cardChoseSfx;
    [SerializeField] private AudioClip winSfx;
    [SerializeField] private AudioClip loseSfx;
    [SerializeField] private AudioClip drawSfx;
    [SerializeField] private AudioClip pointsSfx;
    
    int actualScore = 0;
    int[] activePlayerSet = new int[3] { -1, -1, -1 };
    int[] opponentPlayerSet = new int[3] { -1, -1, -1 };
    bool[] playerSkills = new bool[4] { false, false, false, false };
    int activePlayerChosenCard;
    int enemyPlayerChosenCard;
    int actualPhase = 0; // 0-wybór własnej karty lub skilla, 1-używanie skilla podglądania karty, 2- wybór własnej karty, 3- wybór karty przeciwnika, 4 - walka(zderzanie, odsłanianie, poruwnywanie), 5 - podlicznie punktów, 6 - wygrana(możliwość wyboru), 7 - przegrana(przechodzenie do następnej rundy), 8- wybór własnej karty podczas skilla prównywania, 9 - wybór karty przeciwnika podczas skilla prównywania   
    int result = -1; // 1-wygrana gracza aktywnego, 2-przegrana gracza aktywnego, 3 - remis;
    int hitCounter = 0;
    bool ifWin = false;
    bool blocadeEnable = false;
    bool scoreX2Enable = false;
    GameObject enemyCard;
    GameObject playerCard;
    GameObject enemyCardToCompare;
    GameObject playerCardToCompare;
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if(hit.collider.name.Substring(0, 5) == "Enemy" && actualPhase == 3)
            {
                enemyCard = hit.collider.gameObject;
                ChooseEnemyCard(hit.collider.gameObject);
                actualPhase = 4;
            }
            else if(hit.collider.name.Substring(0, 6) == "Player" && actualPhase == 0)
            {
                actualPhase = 2;
                playerCard = hit.collider.gameObject;
                activePlayerChosenCard = CardToInt(hit.collider.gameObject);
                CardUp(hit.collider.gameObject);
                actualPhase = 3;
                ChangeActiveStateSkillButtons(false);
            }
            else if(hit.collider.name.Substring(0, 5) == "Enemy" && actualPhase == 1)
            {
                ChooseEnemyCardToSee(hit.collider.gameObject);
                actualPhase = 0;
            }
            else if(hit.collider.name.Substring(0, 6) == "Player" && actualPhase == 8)
            {
                playerCardToCompare = hit.collider.gameObject;
                actualPhase = 9;
            }
            else if(hit.collider.name.Substring(0, 5) == "Enemy" && actualPhase == 9)
            {
                enemyCardToCompare = hit.collider.gameObject;
                CompareCardsSkillSetText();
                actualPhase = 0;
            }
        }
    }

    private void Awake()
    {
        SetSets();
        DrawPlayerCards();
        CoppySkills();
        SetSkillsButtons();
        buttonController.DissactiveButton(continueB);
        buttonController.DissactiveButton(nextRoundB);
        DrawText();
    }

    void SetSets()
    {
        if(Globals.activePlayer)
        {
            for(int i = 0;i < 3;i++)
                activePlayerSet[i] = Globals.rightActivSet[i];
            for(int i = 0;i < 3;i++)
                opponentPlayerSet[i] = Globals.leftActivSet[i];
        }
        else
        {
            for(int i = 0;i < 3;i++)
                activePlayerSet[i] = Globals.leftActivSet[i];
            for(int i = 0;i < 3;i++)
                opponentPlayerSet[i] = Globals.rightActivSet[i];
        }
    }

    void CoppySkills()
    {
        if(Globals.activePlayer)
        {
            for(int i = 0;i < 4;i++)
                playerSkills[i] = Globals.rightSkillSet[i];
        }
        else
        {
            for(int i = 0;i < 4;i++)
                playerSkills[i] = Globals.leftSkillSet[i];
        }
    }

    void DrawPlayerCards()
    {
        for(int i = 1;i <= 3;i++)
        {
            GameObject card = GameObject.Find("Player_Card_" + i);
            SpriteRenderer cardImg = card.GetComponent<SpriteRenderer>();
            switch(activePlayerSet[i - 1])
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
    }   

    void SetSkillsButtons()
    {
        SetSkillButton(skill1B,0);
        SetSkillButton(skill2B,1);
        SetSkillButton(skill3B,2);
        SetSkillButton(skill4B,3);
    }

    void SetSkillButton(GameObject button, int indexOfSkill)
    {
        if(playerSkills[indexOfSkill])
        {
            buttonController.ActiveButton(button);
        }
        else
        {
            buttonController.DissactiveButton(button);
        }
    }

    void ChangeActiveStateSkillButtons(bool turnOn)
    {
        if(turnOn)
        {
            SetSkillsButtons();
        }
        else
        {
            buttonController.DissactiveButton(skill1B);
            buttonController.DissactiveButton(skill2B);
            buttonController.DissactiveButton(skill3B);
            buttonController.DissactiveButton(skill4B);
        }
    }

    void DrawText()
    {
        ScorePlayer1TMP.text = "Score " + Globals.playerName[0] + ": " + Globals.leftScore;
        ScorePlayer2TMP.text = "Score " + Globals.playerName[1] + ": " + Globals.rightScore;
        RoundTMP.text = "Round " + Globals.activeRound;
        if(Globals.activePlayer)
            ActivePlayerTMP.text = "Active player is " + Globals.playerName[1];
        else
            ActivePlayerTMP.text = "Active player is " + Globals.playerName[0];        
    }

    void ChooseEnemyCard(GameObject card)
    {
        SpriteRenderer cardImg = card.GetComponent<SpriteRenderer>();
        if(cardImg.sprite.ToString() == Globals.reverse.ToString())
        {
            card.GetComponent<Animator>().SetBool("Around", true);
            AudioController.instance.PlaySfx(cardChoseSfx);
        }
        else
        {
            CompareCards();
            AudioController.instance.PlaySfx(cardChoseSfx);
        }        
    }

    public void DrawEnemyCard(GameObject card)
    {
        SpriteRenderer cardImg = card.GetComponent<SpriteRenderer>();
        int indexCard = int.Parse(card.name.Substring(11)) - 1;
        switch(opponentPlayerSet[indexCard])
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

    int CardToInt(GameObject card)
    {
        string cardName = card.GetComponent<SpriteRenderer>().sprite.name;
        switch(cardName)
        {
            case "wolf_card":
                return 1;
            case "hedgehog_card":
                return 2;
            case "raccoon_card":
                return 3;
            case "bear_card":
                return 4;
            case "frog_card":
                return 5;
            default:
                return -1; //Oznacza ERROR
        }
    }

    void CardUp(GameObject card)
    {
        card.GetComponent<Animator>().SetBool("Select", true);
        AudioController.instance.PlaySfx(cardChoseSfx);
    }

    public void CompareCards()
    {
        enemyPlayerChosenCard = CardToInt(enemyCard);
        int result = CompareCardsResult(activePlayerChosenCard, enemyPlayerChosenCard); //Zwraca: 0 - Wygrana, 1 - Przegrana, 2 - Remis

        SetFlare(result);

        playerCard.GetComponent<Animator>().SetBool("Fight", true);
        enemyCard.GetComponent<Animator>().SetBool("Fight", true);        

        switch(result)
        {
            case 0:
                Win();
                break;
            case 1:
                Lose();
                break;
            case 2:
                Draw();
                break;            
        }
    }

    int CompareCardsResult(int playerCard, int enemyCard)
    {
        //Zwraca: 0 - Wygrana, 1 - Przegrana, 2 - Remis
        switch(playerCard)//1-wilk, 2-jeż, 3-szop, 4-niedźwiedź, 5-żaba
        {
            case 1:
                switch(enemyCard)
                {
                    case 1:
                        return 2;
                    case 2:
                        return 0;
                    case 3:
                        return 0;
                    case 4:
                        return 1;
                    case 5:
                        return 1;
                }
                break;
            case 2:
                switch(enemyCard)
                {
                    case 1:
                        return 1;
                    case 2:
                        return 2;
                    case 3:
                        return 1;
                    case 4:
                        return 0;
                    case 5:
                        return 0;
                }
                break;
            case 3:
                switch(enemyCard)
                {
                    case 1:
                        return 1;
                    case 2:
                        return 0;
                    case 3:
                        return 2;
                    case 4:
                        return 1;
                    case 5:
                        return 0;
                }
                break;
            case 4:
                switch(enemyCard)
                {
                    case 1:
                        return 0;
                    case 2:
                        return 1;
                    case 3:
                        return 0;
                    case 4:
                        return 2;
                    case 5:
                        return 1;
                }
                break;
            case 5:
                switch(enemyCard)
                {
                    case 1:
                        return 0;
                    case 2:
                        return 1;
                    case 3:
                        return 1;
                    case 4:
                        return 0;
                    case 5:
                        return 2;
                }
                break;
        }
        return -1; //ERROR
    }

    void SetFlare(int flareState)
    {
        ParticleSystemRenderer playerFlare = playerCard.transform.GetChild(0).GetChild(0).GetComponentInChildren<ParticleSystemRenderer>();
        ParticleSystemRenderer playerDust = playerCard.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();
        ParticleSystemRenderer enemyFlare = enemyCard.transform.GetChild(0).GetChild(0).GetComponentInChildren<ParticleSystemRenderer>();
        ParticleSystemRenderer enemyDust = enemyCard.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();

        switch(flareState)
        {
            case 0:
                playerFlare.material = FlareWin;
                playerDust.material = DustWin;
                enemyFlare.material = FlareLose;
                enemyDust.material = DustLose;
                break;
            case 1:
                playerFlare.material = FlareLose;
                playerDust.material = DustLose;
                enemyFlare.material = FlareWin;
                enemyDust.material = DustWin;
                break;
            case 2:
                playerFlare.material = FlareDraw;
                playerDust.material = DustDraw;
                enemyFlare.material = FlareDraw;
                enemyDust.material = DustDraw;
                break;
        }
    }

    void Win()
    {
        hitCounter++;
        if(scoreX2Enable)
        {
            actualScore += (hitCounter*2);
        }
        else
        {
            actualScore += hitCounter;
        }        
        result = 1;
        ActualScoreTMP.text = "Active \r\n Score: " + actualScore;
        if(hitCounter < 3)
            buttonController.ActiveButton(continueB);
        buttonController.ActiveButton(nextRoundB);
        actualPhase = 6;
        ResultTMP.text = "YOU WIN";
        ResultTMP.color = new Color32(0, 255, 0, 0);
        ResultTMP.gameObject.GetComponent<Animator>().SetTrigger("Fade");
        ifWin = true;
        enemyCard.GetComponent<Animator>().SetBool("Lose", true);
        playerCard.GetComponent<Animator>().SetBool("Win", true);
        AudioController.instance.PlaySfx(winSfx);
    }

    void Lose()
    {
        hitCounter++;
        result = 2;
        buttonController.ActiveButton(nextRoundB);
        actualPhase = 7;
        ResultTMP.text = "YOU LOSE";
        ResultTMP.color = new Color32(255, 0, 0, 0);
        ResultTMP.gameObject.GetComponent<Animator>().SetTrigger("Fade");
        ifWin = false;
        playerCard.GetComponent<Animator>().SetBool("Lose", true);
        enemyCard.GetComponent<Animator>().SetBool("Win", true);
        AudioController.instance.PlaySfx(loseSfx);
    }

    void Draw()
    {
        hitCounter++;
        result = 3;
        ActualScoreTMP.text = "Active \r\n Score: " + actualScore;
        if(hitCounter < 3)
            buttonController.ActiveButton(continueB);
        buttonController.ActiveButton(nextRoundB);
        actualPhase = 6;
        ResultTMP.text = "YOU DRAW";
        ResultTMP.color = new Color32(255, 255, 0, 0);
        ResultTMP.gameObject.GetComponent<Animator>().SetTrigger("Fade");
        ifWin = true;
        playerCard.GetComponent<Animator>().SetBool("Win", true);
        enemyCard.GetComponent<Animator>().SetBool("Win", true);
        AudioController.instance.PlaySfx(drawSfx);
    }

    public void ContinueFight()
    {
        buttonController.DissactiveButton(continueB);
        buttonController.DissactiveButton(nextRoundB);
        ChangeActiveStateSkillButtons(true);
        blocadeEnable = false;
        scoreX2Enable = false;
        ResultTMP.text = "";
        actualPhase = 0; 
    }

    public void Summary()
    {
        Globals.activeRound++; //Zwiększnie rundy
        if(Globals.activePlayer) // Przepisywanie punktów i zmiana gracza aktywnego
        {
            if(ifWin)
            {
                Globals.rightScore += actualScore;
            }
            else if(!blocadeEnable)
            {
                Globals.leftScore += actualScore;
            }

            for(int i = 0;i < 4;i++)
                Globals.rightSkillSet[i] = playerSkills[i];
        }
        else
        {
            if(ifWin)
            {
                Globals.leftScore += actualScore;
            }
            else if(!blocadeEnable)
            {
                Globals.rightScore += actualScore;
            }            

            for(int i = 0;i < 4;i++)
                Globals.leftSkillSet[i] = playerSkills[i];
        }
        Globals.activePlayer = !Globals.activePlayer;
        Globals.roundPhase = 0;
    }

    public void SeeCardSkill()
    {
        SkillUsed(0);
        actualPhase = 1;

    }

    void ChooseEnemyCardToSee(GameObject card)
    {
        card.GetComponent<Animator>().SetBool("LookUp", true);
        AudioController.instance.PlaySfx(cardChoseSfx);
    }  

    public void CompareCardsSkill()
    {
        SkillUsed(1);
        actualPhase = 8;
    }

    void CompareCardsSkillSetText()
    {
        int indexEnemyCard = int.Parse(enemyCardToCompare.name.Substring(11))-1;
        int intPlayerCard = CardToInt(playerCardToCompare);
        int intEnemyCard = opponentPlayerSet[indexEnemyCard];
        int result = CompareCardsResult(intPlayerCard, intEnemyCard);   
        switch(result)//Zwraca: 0 - Wygrana, 1 - Przegrana, 2 - Remis
        {
            case 0:
                ResultTMP.text = "YOU WILL WIN";
                ResultTMP.color = new Color32(0, 255, 0, 0);
                AudioController.instance.PlaySfx(winSfx);
                break;
            case 1:
                ResultTMP.text = "YOU WILL LOSE";
                ResultTMP.color = new Color32(255, 0, 0, 0);
                AudioController.instance.PlaySfx(loseSfx);
                break;
            case 2:
                ResultTMP.text = "YOU WILL DRAW";
                ResultTMP.color = new Color32(255, 255, 0, 0);
                AudioController.instance.PlaySfx(drawSfx);
                break;
        }        
        ResultTMP.gameObject.GetComponent<Animator>().SetTrigger("Fade");
    }

    public void BlockingTheftSkill()
    {
        SkillUsed(2);
        blocadeEnable = true;
    }

    public void ScoreX2Skill()
    {
        SkillUsed(3);
        scoreX2Enable = true;
    }

    void SkillUsed(int indexOfSkill)
    {
        playerSkills[indexOfSkill] = false;
        ChangeActiveStateSkillButtons(false);
    }
}
