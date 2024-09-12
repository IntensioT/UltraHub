using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMenuUIHandler : MonoBehaviour
{

    [SerializeField] Image aimImage;
    [SerializeField] TextMeshProUGUI winnerCongrats;
    [SerializeField] TextMeshProUGUI playerNick;
    [SerializeField] TextMeshProUGUI timeSpent;

    private bool isWritten = false;

    void Start()
    {

    }

    public void OnEndGameMessageReceived(string playerName)
    {
        aimImage.enabled = false;

        winnerCongrats.text = "Winner is determined ! Congratulate  ";
        

        playerNick.text = playerName;
        if (timeSpent.text == null || timeSpent.text == "" )
            timeSpent.text = "Time spent to finish: " + Utils.GetGameTime().ToString("F2") + " seconds";
    }
}
