using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitButton : MonoBehaviour
{
    public List<GameObject> SelectHand;
    public Button SaveButton;

    void Start()
    {
        SaveButton.onClick.AddListener(SaveSelectedHand);
    }

    void SaveSelectedHand()
    {
        // SelectHand 리스트에 5개의 아이템이 있다면
        if (SelectHand.Count == 5)
        {
            // SelectHand 리스트 초기화
            SelectHand.Clear();

            // TODO: 족보 저장 로직 구현
            // ...
        }
    }
}
