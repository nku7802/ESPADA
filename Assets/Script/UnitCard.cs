using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitCard : MonoBehaviour
{
     public string unitName; // 유닛 이름
    public int attack;     // 유닛 공격력
    public string whatunitcard;
    private Player player;

    private Dictionary<string, UnitCardInfo> RANK_TO_UNITCARDINFO;

    private Field field;

    private Vector3 originalPosition;

    private static UnitCard currentSelectedUnitCard = null;

    private void Awake()
    {
        originalPosition = transform.position;

        player = FindObjectOfType<Player>();

        if (player == null)
        {
            Debug.LogError("Could not find Player object in scene!");
        }


        RANK_TO_UNITCARDINFO = new Dictionary<string, UnitCardInfo>
        {
        {"High Card: 6 High" ,      new UnitCardInfo("Cards/0", "CardShader/0", 10)},
        {"High Card: 7 High" ,      new UnitCardInfo("Cards/0", "CardShader/0", 20)},
        {"High Card: 8 High" ,      new UnitCardInfo("Cards/0", "CardShader/0", 30)},
        {"High Card: 9 High" ,      new UnitCardInfo("Cards/0", "CardShader/0", 40)},
        {"High Card: 10 High" ,     new UnitCardInfo("Cards/0", "CardShader/0", 50)},
        {"High Card: JACK High" ,   new UnitCardInfo("Cards/0", "CardShader/0", 60)},
        {"High Card: QUEEN High" ,  new UnitCardInfo("Cards/0", "CardShader/0", 70)},
        {"High Card: KING High" ,   new UnitCardInfo("Cards/0", "CardShader/0", 80)},
        {"High Card: ACE High" ,    new UnitCardInfo("Cards/0", "CardShader/0", 90)},

        {"One Pair: 2 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 100)},
        {"One Pair: 3 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 110)},
        {"One Pair: 4 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 120)},
        {"One Pair: 5 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 130)},
        {"One Pair: 6 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 140)},
        {"One Pair: 7 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 150)},
        {"One Pair: 8 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 160)},
        {"One Pair: 9 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 170)},
        {"One Pair: 10 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 180)},
        {"One Pair: JACK High" ,    new UnitCardInfo("Cards/0","CardShader/0", 190)},
        {"One Pair: QUEEN High" ,    new UnitCardInfo("Cards/0","CardShader/0", 200)},
        {"One Pair: KING High" ,    new UnitCardInfo("Cards/0","CardShader/0", 210)},
        {"One Pair: ACE High" ,    new UnitCardInfo("Cards/0","CardShader/0", 220)},

        {"Two Pair: 2 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 230)},
        {"Two Pair: 3 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 240)},
        {"Two Pair: 4 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 250)},
        {"Two Pair: 5 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 260)},
        {"Two Pair: 6 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 270)},
        {"Two Pair: 7 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 280)},
        {"Two Pair: 8 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 290)},
        {"Two Pair: 9 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 300)},
        {"Two Pair: 10 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 310)},
        {"Two Pair: JACK High" ,    new UnitCardInfo("Cards/0","CardShader/0", 320)},
        {"Two Pair: QUEEN High" ,    new UnitCardInfo("Cards/0","CardShader/0", 330)},
        {"Two Pair: KING High" ,    new UnitCardInfo("Cards/0","CardShader/0", 340)},
        {"Two Pair: ACE High" ,    new UnitCardInfo("Cards/0","CardShader/0", 350)},

        {"Three of a Kind: 2 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 360)},
        {"Three of a Kind: 3 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 370)},
        {"Three of a Kind: 4 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 380)},
        {"Three of a Kind: 5 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 390)},
        {"Three of a Kind: 6 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 400)},
        {"Three of a Kind: 7 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 410)},
        {"Three of a Kind: 8 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 420)},
        {"Three of a Kind: 9 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 430)},
        {"Three of a Kind: 10 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 440)},
        {"Three of a Kind: JACK High" ,    new UnitCardInfo("Cards/0","CardShader/0", 450)},
        {"Three of a Kind: QUEEN High" ,    new UnitCardInfo("Cards/0","CardShader/0", 460)},
        {"Three of a Kind: KING High" ,    new UnitCardInfo("Cards/0","CardShader/0", 470)},
        {"Three of a Kind: ACE High" ,    new UnitCardInfo("Cards/0","CardShader/0", 480)},

        {"Straight: 5 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 490)},
        {"Straight: 6 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 500)},
        {"Straight: 7 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 510)},
        {"Straight: 8 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 520)},
        {"Straight: 9 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 530)},
        {"Straight: 10 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 540)},
        {"Straight: JACK High" ,    new UnitCardInfo("Cards/0","CardShader/0", 550)},
        {"Straight: QUEEN High" ,    new UnitCardInfo("Cards/0","CardShader/0", 560)},
        {"Straight: KING High" ,    new UnitCardInfo("Cards/0","CardShader/0", 570)},
        {"Straight: ACE High" ,    new UnitCardInfo("Cards/0","CardShader/0", 580)},

        {"Flush: Diamonds High" ,    new UnitCardInfo("Cards/0","CardShader/0", 590)},
        {"Flush: Clubs High" ,    new UnitCardInfo("Cards/0","CardShader/0", 600)},
        {"Flush: Hearts High" ,    new UnitCardInfo("Cards/0","CardShader/0", 610)},
        {"Flush: Spades High" ,    new UnitCardInfo("Cards/0","CardShader/0", 620)},

        {"Full House: 2 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 630)},
        {"Full House: 3 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 630)},
        {"Full House: 4 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 640)},
        {"Full House: 5 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 650)},
        {"Full House: 6 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 660)},
        {"Full House: 7 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 670)},
        {"Full House: 8 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 680)},
        {"Full House: 9 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 690)},
        {"Full House: 10 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 700)},
        {"Full House: JACK High" ,    new UnitCardInfo("Cards/0","CardShader/0", 710)},
        {"Full House: QUEEN High" ,    new UnitCardInfo("Cards/0","CardShader/0", 720)},
        {"Full House: KING High" ,    new UnitCardInfo("Cards/0","CardShader/0", 730)},
        {"Full House: ACE High" ,    new UnitCardInfo("Cards/0","CardShader/0", 740)},

        {"Four of a Kind: 2 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 750)},
        {"Four of a Kind: 3 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 760)},
        {"Four of a Kind: 4 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 770)},
        {"Four of a Kind: 5 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 780)},
        {"Four of a Kind: 6 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 790)},
        {"Four of a Kind: 7 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 800)},
        {"Four of a Kind: 8 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 810)},
        {"Four of a Kind: 9 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 820)},
        {"Four of a Kind: 10 High" ,    new UnitCardInfo("Cards/0","CardShader/0", 830)},
        {"Four of a Kind: JACK High" ,    new UnitCardInfo("Cards/0","CardShader/0", 840)},
        {"Four of a Kind: QUEEN High" ,    new UnitCardInfo("Cards/0","CardShader/0", 850)},
        {"Four of a Kind: KING High" ,    new UnitCardInfo("Cards/0","CardShader/0", 860)},
        {"Four of a Kind: ACE High" ,    new UnitCardInfo("Cards/0","CardShader/0", 870)},

        {"Straight Flush: Diamonds Flush" ,    new UnitCardInfo("Cards/0","CardShader/0", 880)},
        {"Straight Flush: Clubs Flush" ,    new UnitCardInfo("Cards/0","CardShader/0", 890)},
        {"Straight Flush: Hearts Flush" ,    new UnitCardInfo("Cards/0","CardShader/0", 900)},
        {"Straight Flush: Spades Flush" ,    new UnitCardInfo("Cards/0","CardShader/0", 910)},

        {"Diamonds Mountain" ,    new UnitCardInfo("Cards/0","CardShader/0", 920)},
        {"Clubs: Mountain" ,    new UnitCardInfo("Cards/0","CardShader/0", 930)},
        {"Hearts: Mountain" ,    new UnitCardInfo("Cards/0","CardShader/0", 940)},
        {"Spades: Mountain" ,    new UnitCardInfo("Cards/0","CardShader/0", 950)},

        {"Diamonds Royal Straight Flush" ,    new UnitCardInfo("Cards/0","CardShader/0", 960)},
        {"Clubs Royal Straight Flush" ,    new UnitCardInfo("Cards/0","CardShader/0", 970)},
        {"Hearts Royal Straight Flush" ,    new UnitCardInfo("Cards/0","CardShader/0", 980)},
        {"Spades Royal Straight Flush" ,    new UnitCardInfo("Cards/0","CardShader/0", 990)},

        };
    }




    private void Update()
    {
        if (isDragging)
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos.z = 0;
            transform.position = newPos;
        }
        
    }

    private int number = 1;
    public void Init(string handRank, int number)
    {
        this.number = number;
        this.gameObject.name = $"UnitCard_0{number}";
        attack = RANK_TO_UNITCARDINFO[handRank].attack;

        whatunitcard = handRank;

        GetComponent<SpriteRenderer>().sprite = RANK_TO_UNITCARDINFO[handRank].sprite;
        GetComponent<SpriteRenderer>().material = RANK_TO_UNITCARDINFO[handRank].material;

    }

    private struct UnitCardInfo
    {
        public Sprite sprite;
        public Material material;
        public int attack;

        public UnitCardInfo(string spritePath, string materialPath, int attack)
        {
            this.sprite = Resources.Load<Sprite>(spritePath);
            this.material = Resources.Load<Material>(materialPath);
            this.attack = attack;
        }
    }

    private Vector3 screenPoint;
    private Vector3 offset;
    private bool isDragging = false;

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

/*void OnMouseEnter()
{
    if (isMoving) return;
    if (isDragging) return; // 드래그 중일 경우 return
    if (currentSelectedUnitCard != null) return;
    transform.SetAsLastSibling(); // 클릭된 오브젝트를 가장 위에 위치하도록 설정
    if (player != null && player.unithands.Count >= 3)
    {
        field?.RemovePlayerUnitCard(this);
        isDragging = true;
    }
}*/

 private void OnMouseDown()
    {
        transform.SetAsLastSibling(); // 클릭된 오브젝트를 가장 위에 위치하도록 설정

        if (player != null && player.unithands.Count >= 3)
    {field?.RemovePlayerUnitCard(this);
        isDragging = true;
        isMoving = false;

        }

    }


void OnMouseUp()
{
    if (!isDragging) return;

    isDragging = false;


    // 이동 완료 처리 코드
    // ...
}

    void OnMouseExit()
    {
        if (currentSelectedUnitCard == this)
            currentSelectedUnitCard = null;

        transform.SetAsFirstSibling(); // 클릭된 오브젝트를 가장 아래쪽에 위치하도록 설정

    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Field>(out Field field)) return;

        this.field = field;
        field.AddPlayerUnitCard(this);
    }

    public void MoveToOriginalPosition(float duration)
    {
        Move(originalPosition, duration);
    }

    public void Move(Vector3 targetPosition, float duration)
    {
        StartCoroutine(MoveToPosition(targetPosition, duration));
    }

    private bool isMoving = false;
    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration, GameObject obj = null)
    {
        isMoving = true;
        isDragging = false;
        Vector3 startPosition = obj == null ? transform.position : obj.transform.position;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            if (obj == null)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            }
            else
            {
                obj.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            }

            isDragging = false;
            isMoving = false;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (obj == null)
        {
            transform.position = targetPosition;
        }
        else
        {
            obj.transform.position = targetPosition;
        }
    }
}