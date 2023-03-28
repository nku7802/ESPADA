using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public readonly float[] xPositions = new float[] { -8f, -6f, -4f, -2f, 0f, 2f, 4f, 6f, 8f }; // 패의 X 포지션
    public readonly float[] SelectxPositions = new float[] { -7.75f, -6.15f, -4.51f, -2.89f, -1.32f };
    public readonly float[] UnityxPositions = new float[] { 3.5f, 5.5f, 7.5f };
    public readonly float[] FieldxPositions = new float[] { -5f, 0f, 5f };
    public readonly float[] yPositions = new float[] { -4f, -4f, -4f, -4f, -4f, -4f, -4f, -4f, -4f }; // 패의 Y 포지션
    public List<Card> firsthands = new List<Card>(new Card[9]); // 10개의 카드를 저장하는 리스트
    private Dictionary<Card, int> firsthandsIndex = new Dictionary<Card, int>();
    public List<Card> selectedhands = new List<Card>(new Card[5]); // 크기를 5로 고정
    public List<string> unithands = new List<string>();
    // public List<string> tagList;
    private int currentTagIndex = 0;

    // public static string HandRank = "";
    public static string handRank;
    private Enemy enemy;
    public Field field_01;
    public Field field_02;
    public Field field_03;


    public UnitCard unitCardPrefab;
    public Field fieldPrefab;

    // public List<Card> 
    [SerializeField]
    private Dealer dealer;
    private UnitCard unitCard;

    private Dealer dealerInstance;
    private Canvas canvas;
    [SerializeField]
    private Field fieldScript;
    [SerializeField]
    public int life = 8;
    public int enemylife = 8;
    public bool ReStart = false;


    void Awake()
    {
        // Canvas GameObject 찾기
        GameObject canvasObj = GameObject.Find("Canvas");
        // Canvas에서 Panel UI 찾기
        Image panel = canvasObj.GetComponentInChildren<Image>();
        canvasObj.SetActive(false);
        life = PlayerPrefs.GetInt("life", 8);
        enemylife = PlayerPrefs.GetInt("enemylife", 8);
        Debug.Log(life);
        Debug.Log(enemylife);
        enemy = FindObjectOfType<Enemy>();
        canvas = GetComponentInChildren<Canvas>();
        dealerInstance = FindObjectOfType<Dealer>();
    }

    public bool AddCard(Card card)
    {
        for (int i = 0; i < Mathf.Min(firsthands.Count, xPositions.Length, yPositions.Length); i++)
        {
            if (firsthands[i] == null)
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
        if (life <= 0)
        {
            // Canvas GameObject 찾기
            GameObject canvasObj = GameObject.Find("Canvas");
            // Canvas에서 Panel UI 찾기
            Image panel = canvasObj.GetComponentInChildren<Image>();
            canvasObj.SetActive(true);


            Debug.Log("gameover");
        }
        if (enemylife <= 0)
        {
            Debug.Log("Win");
        }
        List<Card> selectedHands = new List<Card>(); // 선택된 카드 리스트
        List<string> unitHands = new List<string>(); // 유닛핸드 리스트
        string handRank = string.Empty; // 핸드 랭크 문자열 변수
        GetPokerHand(selectedHands, out unitHands, out handRank);

        //ButtonImageChange(handRank);

        for (int i = 0; i < Mathf.Min(firsthands.Count, xPositions.Length, yPositions.Length); i++)
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
                if (success)
                {
                    firsthands[i] = null;
                }

                break;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (selectedhands[i] == null) continue;

            Vector2 currentCardPosition = new Vector2(SelectxPositions[i], -1.99f);
            Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distanceToCurrent = Vector2.Distance(currentMousePosition, currentCardPosition);

            if (Input.GetMouseButtonDown(0) && distanceToCurrent < 1)
            {
                // 카드 뺌
                Card selectedCard = selectedhands[i];                       // selectedhands에서 누른 카드
                int currentIndex = selectedhands.IndexOf(selectedCard);
                selectedhands[currentIndex] = null;

                // 카드 넣음
                int originalIndex = firsthandsIndex[selectedCard];
                firsthands[originalIndex] = selectedCard;
                selectedCard.Move(new Vector3(xPositions[originalIndex], yPositions[originalIndex]));
            }
        }
    }

    private bool AddSelectedHands(Card card)
    {
        for (int i = 0; i < selectedhands.Count; i++)
        {
            if (selectedhands[i] != null) continue;

            selectedhands[i] = card;
            card.Move(new Vector3(SelectxPositions[i], -1.99f));
            return true;
        }

        return false;   // 넣을 자리 없음
    }

    public GameObject buttonPrefab;

    private int click = 0;
    private int number = 1;

    public void OnClickButton()
    {
        GameObject[] uiObjects = GameObject.FindGameObjectsWithTag("UI");
        if (selectedhands[0] != null && selectedhands[1] != null && selectedhands[2] != null && selectedhands[3] != null && selectedhands[4] != null)
        {
            GetPokerHand(selectedhands, out List<string> unithands, out string handRank);
            Player.handRank = handRank;
            this.unithands.Add(handRank); // handRank 값을 unithand 리스트에 추가
            print(handRank);

            Field field = Instantiate<Field>(fieldPrefab, new Vector3(FieldxPositions[click], 1.5f, 0), Quaternion.identity);
            field.Init(number);
            Field fieldScript = FindObjectOfType<Field>();
            List<int> fieldList = fieldScript.fieldList;

            UnitCard unitCard = Instantiate<UnitCard>(unitCardPrefab, new Vector3(UnityxPositions[click], -1.3f, 0), Quaternion.identity);
            unitCard.Init(handRank, number);
            number++;
            currentTagIndex++;
            click++;
            // 리스트 안에 있는 모든 게임 오브젝트를 파괴합니다.
            foreach (Card card in selectedhands)
            {
                Destroy(card.gameObject);
            }

            // 리스트를 비웁니다.
            selectedhands = new List<Card>(new Card[5]);


            dealer.DrawCard();
            Button button = transform.GetComponentInChildren<Button>();
            if (button != null && this.unithands.Count >= 3)
            {
                foreach (Card card in firsthands)
                {
                    if (card != null)
                    {
                        Destroy(card.gameObject);
                    }
                }
                foreach (Card card in dealerInstance.cardList)
                {
                    if (card != null)
                    {
                        Destroy(card.gameObject);
                        dealer.cardList = new List<Card>(new Card[0]);
                    }
                }
                foreach (GameObject obj in uiObjects)
                {
                    if (obj != null)
                    {
                        Destroy(obj);
                    }
                }

                Image buttonImage = button.GetComponent<Image>();
                Color color = buttonImage.color;
                color.a = 0f;
                buttonImage.color = color;
                button.interactable = false;


                GameObject secondButtonmake = Instantiate(buttonPrefab, new Vector3(0, -450, 0), Quaternion.identity, canvas.transform);
                Button secondButton = secondButtonmake.GetComponent<Button>();
                secondButton.transform.GetChild(0).gameObject.SetActive(false);
                RectTransform secondButtonRect = secondButton.GetComponent<RectTransform>();
                Image secondbuttonImage = secondButton.GetComponent<Image>();
                secondbuttonImage.sprite = Resources.Load<Sprite>("Cards/0");


                secondButton.transition = Selectable.Transition.SpriteSwap;
                secondbuttonImage.type = Image.Type.Simple; // 생성된 버튼의 이미지 타입을 Simple로 변경
                // 버튼 위치 조정
                secondButtonRect.localPosition = new Vector3(0, -450, 0);

                // 버튼 크기 조정
                secondButtonRect.sizeDelta = new Vector2(100, 50);

                // 버튼 스케일 조정
                secondButtonRect.localScale = new Vector3(2, 2, 2);

                // 두 번째 버튼에 클릭 이벤트 추가
                Button secondButtonComponent = secondButton.GetComponent<Button>();
                secondButtonComponent.onClick.AddListener(OnClick);

            }
        }
    }



    public bool isClicked = false;
    public void OnClick()
    {
        Field fieldScript = FindObjectOfType<Field>();
        List<int> fieldList = fieldScript.fieldList;

        Enemy enemyScript = GetComponent<Enemy>();
        Enemy enemy = new Enemy();

        GameObject field01Obj = GameObject.Find("Field_01");
        GameObject field02Obj = GameObject.Find("Field_02");
        GameObject field03Obj = GameObject.Find("Field_03");

        Field fieldComponent01 = field01Obj.GetComponent<Field>();
        Field fieldComponent02 = field02Obj.GetComponent<Field>();
        Field fieldComponent03 = field03Obj.GetComponent<Field>();

        if (fieldComponent01 == null || fieldComponent02 == null || fieldComponent03 == null)
        {
            Debug.Log("Field 컴포넌트를 찾을 수 없습니다.");
        }
        else if (fieldList.Count == 0)
        {
            Debug.Log("리스트에 값이 없습니다.");
        }
        else // 전투 가능 상태
        {
            if (isClicked) return;

            for (int i = 0; i < fieldComponent01.fieldList.Count; i++)
            {
                Debug.Log("Field_01 List:" + fieldComponent01.fieldList[i]);
            }
            for (int i = 0; i < fieldComponent02.fieldList.Count; i++)
            {
                Debug.Log("Field_02 List:" + fieldComponent02.fieldList[i]);
            }
            for (int i = 0; i < fieldComponent03.fieldList.Count; i++)
            {
                Debug.Log("Field_03 List:" + fieldComponent03.fieldList[i]);
            }
            int randomValue1 = UnityEngine.Random.Range(1, 10);
            int randomValue2 = UnityEngine.Random.Range(1, 10);
            int randomValue3 = UnityEngine.Random.Range(1, 10);

            var enemyInfo1 = Enemy.RANK_TO_ENEMYINFO[randomValue1];
            var enemyInfo2 = Enemy.RANK_TO_ENEMYINFO[randomValue2];
            var enemyInfo3 = Enemy.RANK_TO_ENEMYINFO[randomValue3];

            fieldComponent01.fieldList.Add(enemyInfo1.attackPower);
            fieldComponent02.fieldList.Add(enemyInfo2.attackPower);
            fieldComponent03.fieldList.Add(enemyInfo3.attackPower);


            for (int i = 0; i < fieldComponent01.fieldList.Count; i++)
            {
                Debug.Log("Field_01 List:" + fieldComponent01.fieldList[i]);
            }

            for (int i = 0; i < fieldComponent02.fieldList.Count; i++)
            {
                Debug.Log("Field_02 List:" + fieldComponent02.fieldList[i]);
            }

            for (int i = 0; i < fieldComponent03.fieldList.Count; i++)
            {
                Debug.Log("Field_03 List:" + fieldComponent03.fieldList[i]);
            }

            isClicked = true;

            Invoke("Reload", 17f);
        }
    }

    private void Reload()
    {
        PlayerPrefs.SetInt("life", life);
        PlayerPrefs.SetInt("enemylife", enemylife);
        SceneManager.LoadScene("Scene_02");
    }

    /*public void NormalField()
    {

        GameObject field01Obj = GameObject.Find("Field_01");
        if (field01Obj == null)
        {
            Debug.LogError("Field_01 not found!");
            return;
        }

        Field fieldComponent01 = field01Obj.GetComponent<Field>();

        if (fieldComponent01.fieldList == null)
        {
            Debug.LogError("Field_01's fieldList is null!");
            return;
        }

        if (fieldComponent01.fieldList.Count > 0)
        {
            int playerValue = fieldComponent01.fieldList[0];
            int enemyValue = fieldComponent01.fieldList[1];

            if (playerValue > enemyValue)
            {
                Debug.Log("이겼다!");
                enemylife--;
            }
            else if (playerValue == enemyValue)
            {
                Debug.Log("비겼다!");
            }
            else
            {
                Debug.Log("졌다!");
                life--;
            }
        }
        else
        {
            Debug.Log("리스트가 비어 있습니다.");
        }

    }

    public void ReverseField()
    {

        GameObject field02Obj = GameObject.Find("Field_02");
        if (field02Obj == null)
        {
            Debug.LogError("Field_02 not found!");
            return;
        }

        Field fieldComponent02 = field02Obj.GetComponent<Field>();

        if (fieldComponent02.fieldList == null)
        {
            Debug.LogError("Field_02's fieldList is null!");
            return;
        }

        if (fieldComponent02.fieldList.Count > 0)
        {
            int playerValue = fieldComponent02.fieldList[0];
            int enemyValue = fieldComponent02.fieldList[1];

            if (playerValue < enemyValue)
            {
                Debug.Log("이겼다!");
                enemylife--;
            }
            else if (playerValue == enemyValue)
            {
                Debug.Log("비겼다!");
            }
            else
            {
                Debug.Log("졌다!");
                life--;
            }
        }
        else
        {
            Debug.Log("리스트가 비어 있습니다.");
        }

    }

    public void ChangeField()
    {

        GameObject field03Obj = GameObject.Find("Field_03");
        if (field03Obj == null)
        {
            Debug.LogError("Field_03 not found!");
            return;
        }

        Field fieldComponent03 = field03Obj.GetComponent<Field>();

        if (fieldComponent03.fieldList == null || fieldComponent03.fieldList.Count < 2)
        {
            Debug.LogError("Field_03's fieldList is null or does not have enough elements!");
            return;
        }

        int temp = fieldComponent03.fieldList[0];
        fieldComponent03.fieldList[0] = fieldComponent03.fieldList[1];
        fieldComponent03.fieldList[1] = temp;

        int playerValue = fieldComponent03.fieldList[0];
        int enemyValue = fieldComponent03.fieldList[1];

        if (playerValue > enemyValue)
        {
            Debug.Log("이겼다!");
            enemylife--;
        }
        else if (playerValue == enemyValue)
        {
            Debug.Log("비겼다!");
        }
        else
        {
            Debug.Log("졌다!");
            life--;
        }
        isClicked = false;
        ReStart = true;

    }*/

    public enum HandRank
    {
        None
    }

    private string GetCardNumberName(int number)
    {
        return number switch
        {
            1 => "2",
            2 => "3",
            3 => "4",
            4 => "5",
            5 => "6",
            6 => "7",
            7 => "8",
            8 => "9",
            9 => "10",
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

    public void GetPokerHand(List<Card> selecthands, out List<string> unithands, out string handRank)
    {
        unithands = new List<string>();
        handRank = "";

        if (selectedhands.Contains(null))
        {
            handRank = HandRank.None.ToString();
            return;
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

        bool isRoyalStraightFlush = hand.OrderBy(card => card.number).Select(card => card.number).SequenceEqual(new[] { 9, 10, 11, 12, 13 }) && isFlush;

        switch (groups[0].Count())
        {
            case 4:
                handRank = $"Four of a Kind: {GetCardNumberName(groups[0].Key)} High";
                break;
            case 3 when groups.Length == 2 && groups[1].Count() == 2:
                handRank = $"Full House: {{{GetCardNumberName(groups[0].Key)} High";
                break;
            case 3:
                handRank = $"Three of a Kind: {GetCardNumberName(groups[0].Key)} High";
                break;
            case 2 when groups.Length == 3 && groups[1].Count() == 2 && groups[2].Count() == 1:
                var highPairCardNumber = groups[0].Key > groups[1].Key ? groups[0].Key : groups[1].Key;
                var lowPairCardNumber = groups[0].Key < groups[1].Key ? groups[0].Key : groups[1].Key;
                var highPairCardName = GetCardNumberName(highPairCardNumber);
                var TopCardNumber = groups[2].Key;
                handRank = $"Two Pair: {highPairCardName} High";
                break;
            case 2:
                var pairCardNumber = groups[0].Key;
                var pairCardName = GetCardNumberName(pairCardNumber);
                handRank = $"One Pair: {pairCardName} High";
                break;
            default:
                if (isRoyalStraightFlush)
                {
                    handRank = $"{GetSuitName(hand[0].Shape)} Royal Straight Flush";
                }
                else if (isMountain)
                {
                    handRank = $"{GetSuitName(hand[0].Shape)} Mountain";
                }
                else if (isFlush && isStraight)
                {
                    handRank = $"Straight Flush: {GetSuitName(hand[4].Shape)} Flush";
                }
                else if (isFlush)
                {
                    handRank = $"Flush: {GetSuitName(hand[0].Shape)} High";
                }
                else if (isStraight)
                {
                    handRank = $"Straight: {GetCardNumberName(hand[0].number)} High";
                }
                else
                {
                    handRank = $"High Card: {GetCardNumberName(hand[0].number)} High";
                }
                break;
        }
    }




    /*private void ButtonImageChange(string handRank)
    {
        Button button = GetComponentInChildren<Button>();
        Image image = button.GetComponent<Image>();

        Sprite normalSprite, pressedSprite, selectedSprite, disabledSprite;

        switch (handRank)
        {
            case "None":
                normalSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_01");
                selectedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_01");
                pressedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_02");
                disabledSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_02");
                break;
            case "High Card: 6 High":
                normalSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                selectedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                pressedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                disabledSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                break;
            case "High Card: 7 High":
                normalSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                selectedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                pressedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                disabledSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                break;
            case "High Card: 8 High":
                normalSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                selectedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                pressedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                disabledSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                break;
            case "High Card: 9 High":
                normalSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                selectedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                pressedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                disabledSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                break;
            case "High Card: 10 High":
                normalSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                selectedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                pressedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                disabledSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                break;
            case "High Card: JACK High":
                normalSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                selectedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                pressedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                disabledSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                break;
            case "High Card: QUEEN High":
                normalSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                selectedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                pressedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                disabledSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                break;
            case "High Card: KING High":
                normalSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                selectedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                pressedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                disabledSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                break;
            case "High Card: ACE High":
                normalSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");
                selectedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_01");;
                pressedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                disabledSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_heart_02");
                break;
            default:
                // handRank가 알려지지 않은 다른 값인 경우 기본값을 사용한다.
                normalSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_01");
                selectedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_01");
                pressedSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_02");
                disabledSprite = Resources.Load<Sprite>("UI/Button/unitcard_button_02");
                break;
        }

        image.sprite = normalSprite;

        SpriteState spriteState = new SpriteState();
        spriteState.highlightedSprite = normalSprite;
        spriteState.selectedSprite = selectedSprite;
        spriteState.pressedSprite = pressedSprite;
        spriteState.disabledSprite = disabledSprite;

        button.spriteState = spriteState;*/

}