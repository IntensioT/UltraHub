using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class NetworkButtonUIHandler : MonoBehaviour
{
    [SerializeField] Button button;


    public void OnEndGameMessageReceived()
    {
        Debug.Log("ButtonUIHandler trying to draw");
        button.gameObject.SetActive(true); // Включает кнопку
    }
}
