using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public GameObject textPrefab;
    public Transform textParent;

    private List<TextMeshProUGUI> score_text = new List<TextMeshProUGUI>();

    public void GetPlayerScoreState()
    {
        // 텍스트 초기화
        foreach (TextMeshProUGUI text in score_text)
        {
            Destroy(text.gameObject);
        }
        score_text.Clear();

        Vector3 parentPosition = new Vector3(0, 0, 0);
        textParent.transform.position = parentPosition;
        float xOffset = 2.5f; //x축으로 ~~씩 이동하도록 하려고 설정

        // 플레이어 구성을 반복하고 새 텍스트 개체 만들기
        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs)
        {
            int playerIndex = playerConfig.PlayerIndex;
            int score = playerConfig.PlayerScore;
            GameObject newTextObject = Instantiate(textPrefab, textParent);

            Vector3 childPosition = parentPosition + new Vector3(xOffset * playerIndex, 0, 0);
            newTextObject.transform.position = childPosition;

            TextMeshProUGUI newText = newTextObject.GetComponent<TextMeshProUGUI>();
            newText.text = $"{score}pt";
            score_text.Add(newText);
        }
    }
}
