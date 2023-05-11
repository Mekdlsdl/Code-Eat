using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NameManager : MonoBehaviour
{
    public GameObject textPrefab;
    public Transform textParent;

    private List<TextMeshProUGUI> player_text = new List<TextMeshProUGUI>();

    public void GetPlayerNameState()
    {
        // 텍스트 초기화
        foreach (TextMeshProUGUI text in player_text)
        {
            Destroy(text.gameObject);
        }
        player_text.Clear();

        // 부모 객체의 위치 지정
        Vector3 parentPosition = new Vector3(0, 0, 0);
        textParent.transform.position = parentPosition;
        float xOffset = 2.5f; //x축으로 ~~씩 이동하도록 하려고 설정

        // 플레이어 수에 따라 새 텍스트 개체 만들기

        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs)
        {
            int playerIndex = playerConfig.PlayerIndex;
            GameObject newTextObject = Instantiate(textPrefab, textParent);

            //새로 생성된 텍스트 개체의 위치 설정
            Vector3 childPosition = parentPosition + new Vector3(xOffset * playerIndex, 0, 0);
            newTextObject.transform.position = childPosition;

            //새 택스트 개체의 구성요소 load
            TextMeshProUGUI newText = newTextObject.GetComponent<TextMeshProUGUI>();
            newText.text = $"P{playerIndex + 1}";
            player_text.Add(newText);
        }
   }
}

