using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CorrectManager : MonoBehaviour
{
    public GameObject textPrefab;
    public Transform textParent;

    private List<TextMeshProUGUI> correct_text = new List<TextMeshProUGUI>();

    public void GetPlayerCorrectState()
    {
        // �ؽ�Ʈ �ʱ�ȭ
        foreach (TextMeshProUGUI text in correct_text)
        {
            Destroy(text.gameObject);
        }
        correct_text.Clear();

        Vector3 parentPosition = new Vector3(0, 0, 0);
        textParent.transform.position = parentPosition;
        float xOffset = 2.5f; //x������ ~~�� �̵��ϵ��� �Ϸ��� ����

        foreach (PlayerConfiguration playerConfig in PlayerConfigManager.instance.PlayerConfigs)
        {
            int playerIndex = playerConfig.PlayerIndex;
            int correct = playerConfig.CorrectProblemCount;
            int total = ProblemManager.totalProblemCount;
            GameObject newTextObject = Instantiate(textPrefab, textParent);

            Vector3 childPosition = parentPosition + new Vector3(xOffset * playerIndex, 0, 0);
            newTextObject.transform.position = childPosition;

            TextMeshProUGUI newText = newTextObject.GetComponent<TextMeshProUGUI>();
            newText.text = $"{correct} / {total}";
            correct_text.Add(newText);
        }

    }
}
