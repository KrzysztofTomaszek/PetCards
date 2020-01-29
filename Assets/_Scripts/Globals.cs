using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static Sprite wolf = Resources.Load<Sprite>("Cards/wolf_card");
    public static Sprite hedgehog = Resources.Load<Sprite>("Cards/hedgehog_card");
    public static Sprite bear = Resources.Load<Sprite>("Cards/bear_card");
    public static Sprite frog = Resources.Load<Sprite>("Cards/frog_card");
    public static Sprite raccoon = Resources.Load<Sprite>("Cards/raccoon_card");
    public static Sprite reverse = Resources.Load<Sprite>("Cards/reverse_card");
    //public static int firstPlayer = 2; //2- jeszcze nie wybrane, 0 - gracz pierwszy, 1 - gracz drugi
    public static bool activePlayer = false; //Oznaczenie aktywnego gracza w fazie walki; false - pierwszy gracz, true - drugi gracz
    public static int leftScore = 0; //Punkty gracza pierwszego;
    public static int rightScore = 0; //Punkty gracza drugiego;
    //0-Brak karty, 1-Karta wilka, 2-Karta jeża, 3-Karta szopa pracza, 4-Karta niedźwiedzia,5-Karta żaby 
    public static int[] cardSet = new int[30] { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 }; //Tablica z wszystkimi kartami na strcie
    public static int[] leftSet = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //Tablica z wszystkimi kartami gracza pierwszego;
    public static int[] rightSet = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //Tablica z wszystkimi kartami gracza drugiego;
    public static int[] leftActivSet = new int[3] { 0, 0, 0 }; //Tablica z aktywnymi kartami w rundzie gracza pierwszego;
    public static int[] rightActivSet = new int[3] { 0, 0, 0 }; //Tablica z aktywnymi kartami w rundzie gracza drugiego;
    //Kolejność umiejętnośći “Pokaż kartę”, “Porównaj karty”, “Blokada kradzieży”, “Podwojenie zdobywanych punktów”
    public static bool[] rightSkillSet = new bool[4] { true, true, true, true }; //Tablica z umiejętnościami gracza drugiego;
    public static bool[] leftSkillSet = new bool[4] { true, true, true, true }; //Tablica z umiejętnościami gracza pierwszego;
    public static int activeRound = 0; //Znacznik numeru rundy
    public static string[] playerName = new string[2] {"Player1", "Player2" };
    public static int roundPhase = 0; //Oznaczenie fazy gry; 0 - wybór kart gracza pierwszego, 1 - wybór kart gracza drugiego, 2 - faza walki, 3 - podsumowanie 
}
