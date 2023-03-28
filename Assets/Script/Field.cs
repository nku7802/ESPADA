using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public List<int> fieldList = new List<int>();



    private UnitCard playerUnitCard = null;
    private UnitCard enemyUnitCard = null;

    public SpriteRenderer spriteRenderer;
    private int number = 1;
    private Player player;

    public void Init(int number)
    {
        this.number = number;
        this.gameObject.name = $"Field_0{number}";
        spriteRenderer = GetComponent<SpriteRenderer>(); // 카드 sprite 설정
        string spriteName = $"Field_0{this.number}";
        Sprite newSprite = Resources.Load<Sprite>($"UI/Field/{spriteName}");
        spriteRenderer.sprite = newSprite;
    }

    public void RemovePlayerUnitCard(UnitCard unitCard) {
        if(playerUnitCard == unitCard)
        {
            playerUnitCard = null;
        }
        if (fieldList.Contains(unitCard.attack)) {
        fieldList.Remove(unitCard.attack);
    }
    }

    public void AddPlayerUnitCard(UnitCard unitCard) {
        if(playerUnitCard != null) {
            playerUnitCard.MoveToOriginalPosition(0.5f);
        }

        playerUnitCard = unitCard;
        print(playerUnitCard.whatunitcard);
        fieldList.Add(playerUnitCard.attack);
        unitCard.Move(new Vector3(transform.position.x, -1, 0), 0.5f);
    }

    public void NormalField()
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
                player.enemylife--;
            }
            else if (playerValue == enemyValue)
            {
                Debug.Log("비겼다!");
            }
            else
            {
                Debug.Log("졌다!");
                player.life--;
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
                player.enemylife--;
            }
            else if (playerValue == enemyValue)
            {
                Debug.Log("비겼다!");
            }
            else
            {
                Debug.Log("졌다!");
                player.life--;
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
            player.enemylife--;
        }
        else if (playerValue == enemyValue)
        {
            Debug.Log("비겼다!");
        }
        else
        {
            Debug.Log("졌다!");
            player.life--;
        }
        player.isClicked = false;
        player.ReStart = true;

    }
}
