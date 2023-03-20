using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    public readonly float[] xPositions = new float[]{ -8f, -6f, -4f, -2f, 0f, 2f, 4f, 6f, 8f, 10f }; // 패의 X 포지션
    public readonly float[] SelectxPositions = new float[]{ -4f, -2f, 0f, 2f, 4f };
    public readonly float[] yPositions = new float[]{ -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f }; // 패의 Y 포지션
    public List<Card> firsthands = new List<Card>(new Card[10]); // 10개의 카드를 저장하는 리스트
    private Dictionary<Card, int> firsthandsIndex = new Dictionary<Card, int>();
    public List<Card> selectedhands = new List<Card>(new Card[5]); // 크기를 5로 고정


    // public List<Card> 
    [SerializeField]
    private Dealer dealer;
    
    public bool AddCard(Card card)
    {
        for (int i = 0; i < firsthands.Count; i++)
        {
            if (firsthands[i] == null )//&& card.selection[] == true)
            {
                firsthands[i] = card;
                card.Move(new Vector3(xPositions[i], yPositions[i], 0), true);
                card.Flip(true);
                firsthandsIndex.Add(card, i);
                return true;
            }
        }
        return false;
    }

    private void Update() 
    {
        for (int i = 0; i < firsthands.Count; i++) 
        {
            if (firsthands[i] == null) continue;
                
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 cardPosition = new Vector2(xPositions[i], yPositions[i]);
            float distance = Vector2.Distance(mousePosition, cardPosition);

            if (Input.GetMouseButtonDown(0) && distance < 1) 
            {
                Card card = firsthands[i];
                
                // selectedhands로 넣어보기 시도
                bool success = AddSelectedHands(card);
                if(success)
                    firsthands[i] = null;   // 잘 들어갔으면, firsthands애서 제거

                break;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            Vector2 currentCardPosition = new Vector2(SelectxPositions[i], -3f);
            Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distanceToCurrent = Vector2.Distance(currentMousePosition, currentCardPosition);

            if (Input.GetMouseButtonDown(0) && distanceToCurrent < 1) 
            {
                // 카드 뺌
                Card selectedCard = selectedhands[i];                       // selectedhands에서 누른 카드
                int currentIndex =  selectedhands.IndexOf(selectedCard);
                selectedhands[currentIndex] = null;

                // 카드 넣음
                int originalIndex = firsthandsIndex[selectedCard];
                firsthands[originalIndex] = selectedCard;
                selectedCard.Move(new Vector3(xPositions[originalIndex], yPositions[originalIndex]));
            }
        }
    }

    private bool AddSelectedHands(Card card) {
        for (int i = 0; i < selectedhands.Count; i++)
        {
            if(selectedhands[i] != null) continue;

            selectedhands[i] = card;
            card.Move(new Vector3(SelectxPositions[i], -3f));
            return true;
        }

        return false;   // 넣을 자리 없음
    }


    public enum HandRank
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
        None
    }

    private string GetCardNumberName(int number)
    {
        return number switch
        {
            1 => "TWO",
            2 => "THREE",
            3 => "FOUR",
            4 => "FIVE",
            5 => "SIX",
            6 => "SEVEN",
            7 => "EIGHT",
            8 => "NINE",
            9 => "TEN",
            10 => "JACK",
            11 => "QUEEN",
            12 => "KING",
            13 => "ACE",
            _ => throw new ArgumentException("Invalid card number."),
        };
    }

    public string GetSuitName(CardShape shape)
    {
        switch (shape)
        {
            case CardShape.Club:
                return "Clubs";
            case CardShape.Diamond:
                return "Diamonds";
            case CardShape.Heart:
                return "Hearts";
            case CardShape.Spade:
                return "Spades";
            default:
                throw new ArgumentException("Invalid card shape.");
        }
    }

    public string CheckPokerHand()
    {
        if (selectedhands.Contains(null)) 
        {
            return HandRank.None.ToString();
        }
        var hand = selectedhands.OrderByDescending(card => card.number).ToList();
        var groups = hand.GroupBy(card => card.number).OrderByDescending(group => group.Count()).ToArray();


        bool isFlush = hand.All(card => card.Shape == hand[0].Shape);
        bool isMountain = hand.Select(card => card.number).Intersect(new[] { 9, 10, 11, 12, 13 }).Count() == 5 && !isFlush;
        bool isStraight = true;

        for (int i = 0; i < hand.Count - 1; i++)
        {
            if (hand[i].number - hand[i + 1].number != 1)
            {
                isStraight = false;
                break;
            }
        }

        bool isRoyalFlush = hand.OrderBy(card => card.number).Select(card => card.number).SequenceEqual(new[] { 9, 10, 11, 12, 13 }) && isFlush;

    switch (groups[0].Count())
        {
        case 4:
            return $"Four of a Kind: {GetCardNumberName(groups[0].Key)}s";
        case 3 when groups.Length == 2 && groups[1].Count() == 2:
            return $"Full House: {{{GetCardNumberName(groups[0].Key)}s High";
        case 3:
            return $"Three of a Kind: {GetCardNumberName(groups[0].Key)}s";
        case 2 when groups.Length == 3 && groups[1].Count() == 2 && groups[2].Count() == 2:
            return $"Two Pair: {GetCardNumberName(groups[0].Key)}s and {GetCardNumberName(groups[1].Key)}s";
        case 2 when groups.Length == 3 && groups[1].Count() == 2 && groups[2].Count() == 1:
            var highPairCardNumber = groups[0].Key > groups[1].Key ? groups[0].Key : groups[1].Key;
            var lowPairCardNumber = groups[0].Key < groups[1].Key ? groups[0].Key : groups[1].Key;
            var highPairCardName = GetCardNumberName(highPairCardNumber);
            var TopCardNumber = groups[2].Key;
            return $"Two Pair: {highPairCardName}, High";
        case 2:
            var pairCardNumber = groups[0].Key;
            var pairCardName = GetCardNumberName(pairCardNumber);
            return $"One Pair: {pairCardName}, High";
        default:
            if (isRoyalFlush)
            {
                return $"{GetSuitName(hand[0].Shape)} Royal Flush";
            }
            else if (isMountain)
            {
                return $"{GetSuitName(hand[0].Shape)} Mountain";
            }
            else if (isFlush && isStraight)
            {
                return $"Straight Flush: {{{GetSuitName(hand[4].Shape)} Flush";
            }
            else if (isFlush)
            {
                return $"Flush: {GetSuitName(hand[0].Shape)} High";
            }
            else if (isStraight)
            {
                return $"Straight: {GetCardNumberName(hand[0].number)} High";
            }
            else
            {
                return $"High Card: {GetCardNumberName(hand[0].number)} High";
            }
        }
    }   
}
