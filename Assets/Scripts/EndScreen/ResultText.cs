using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultText : MonoBehaviour
{
    [SerializeField] string winText = "YOU WIN";
    [SerializeField] string loseText = "YOU LOSE";
    string textToUse;
    
    void Start()
    {
        if (GameData.Get().GetWinState()) textToUse = winText;
        else textToUse = loseText;

        GetComponent<TextMeshProUGUI>().text = textToUse;
    }

}
