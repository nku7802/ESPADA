using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMaker : MonoBehaviour
{
    private Player player;  // Player 클래스의 인스턴스 선언

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();  // Player 게임 오브젝트에서 Player 컴포넌트 가져오기
    }

    void Update() 
    {
        string pokerHand = player.CheckPokerHand();  // Player 클래스의 인스턴스 사용
        if (pokerHand == "High Card: ACE High") {
            // move the position of the UnitMaker
            Vector3 newPosition = transform.position + new Vector3(1f, 0f, 0f);
            transform.position = newPosition;
            print("Move");
        }
    }

    public void UnitCardMove()
    {
        
    }
}
