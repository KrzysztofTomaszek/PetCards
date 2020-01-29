using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetPlayerNickController : MonoBehaviour
{
    [SerializeField] TMP_InputField Player1NameTMP;
    [SerializeField] TMP_InputField Player2NameTMP;

    private void Awake()
    {
        Player1NameTMP.text = Globals.playerName[0];
        Player2NameTMP.text = Globals.playerName[1];
    }

    public void SetNicks()
    {
        Globals.playerName[0] = Player1NameTMP.text;
        Globals.playerName[1] = Player2NameTMP.text;
    }
}
