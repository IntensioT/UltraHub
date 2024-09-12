using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;

public class PlayerStatusUIHandler : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI healthPointsTMP;
    [SerializeField ]public TextMeshProUGUI coolDownDoubleJumpTMP;

    public string healthPoints = "5";
    public string coolDownDoubleJump;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("HP: " + healthPoints + " | CD: " + coolDownDoubleJump);

        healthPointsTMP.text = healthPoints + " / 5 hp";

        if (coolDownDoubleJump == "0.00")  coolDownDoubleJumpTMP.text = "ready";
        else coolDownDoubleJumpTMP.text = coolDownDoubleJump + " / 3 sec";
    }
}
