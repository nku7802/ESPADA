using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    public List<Card> cardList = new List<Card>();
    [SerializeField]
    public Card cardPrefab;

    [SerializeField]
    private Player player;

    void Start()
    {
        CreateCard();
        ShareCard();
    }

    private void CreateCard() 
    {
        for (int s = 1; s <= 4; s++) {
            for(int i = 1; i < 14; i ++) 
            {
                Card card = Instantiate<Card>(cardPrefab, new Vector3(10,2,0), Quaternion.identity); 
                card.Init((CardShape)s, i);
                cardList.Add(card); 
            }
        }
    }

    private void ShareCard()
    {
        for (int i = 0; i < 9; i++)
        {
            int randomIndex = Random.Range(0, cardList.Count);
            Card card = cardList[randomIndex];
                
            card.selection = true;
            player.AddCard(card);

            cardList.Remove(card);
            card.Flip(true);
        }
    }

    public void DrawCard()
    {
        for (int i = 0; i < 5; i++)
        {
            int randomIndex = Random.Range(0, cardList.Count);
            Card card = cardList[randomIndex];

            card.selection = true;
            player.AddCard(card);

            cardList.Remove(card);
            card.Flip(true);

        }
    }
}
