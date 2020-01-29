using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsAnimationsController : MonoBehaviour
{
    [SerializeField] FightController FG;

    void CardCollisionAnimation()
    {     
        FG.CompareCards();
    }

    void ChooseEnemyCardAnimation()
    {
        FG.DrawEnemyCard(gameObject);
    }
    
    void DestroyCard()
    {
        Destroy(gameObject);
    }
}
