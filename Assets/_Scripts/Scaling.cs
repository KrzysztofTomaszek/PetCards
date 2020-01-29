using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaling : MonoBehaviour
{
    [SerializeField] GameObject cards;
    void Start()
    {
        float scalex, scaley;
        scalex = (float)Screen.width / 1920f;
        scaley = (float)Screen.height / 1080f;
        if (!Screen.fullScreen)
        {
            cards.transform.localScale = new Vector3(scalex, scaley, 1f);
        }
        else if (Screen.fullScreen && ((float)Screen.width / (float)Screen.height) < 1.3334f && ((float)Screen.width / (float)Screen.height) > 1.3332f)
        {
            cards.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
        }
        
    }
}
