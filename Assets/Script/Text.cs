using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Text : MonoBehaviour
{
    private Player player;
    private TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        string pokerHand = player.CheckPokerHand();
        textMeshProUGUI.text = pokerHand;
    }
}
