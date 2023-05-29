using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Random = System.Random;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LectureProblem : MonoBehaviour
{
    [SerializeField] private GameObject guide, problemImage, optionUI, bar;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public List<Color32> barColors = new List<Color32>();

    private ProblemManager pm;
    private Random random = new Random();

    private List<List<int>> barArray = new List<List<int>>(); 
    private List<int> answerBarIndexes = new List<int>();

    private List<GameObject> spawnedOptions = new List<GameObject>();
    
    private bool showOnce = false;
    
    private const int ORIGIN_X = 60;
    private const int ORIGIN_Y = -190;
    private const int LENGTH = 150;
    private const int SPACING = 100;

    void OnEnable()
    {
        pm = ProblemManager.instance;
        StartCoroutine(BeginProblem());
    }

    void OnDisable()
    {
        foreach (GameObject option in spawnedOptions)
            Destroy(option);
    }

    void Update()
    {
        if (pm.isShowingAnswer && !showOnce) {
            showOnce = true;
            HighlightAnswerBars();
        }  
    }

    IEnumerator BeginProblem()
    {
        yield return new WaitForSeconds(3f);
        guide.SetActive(false);
        
        StartCoroutine(CreateBars());
    }

    private IEnumerator CreateBars()
    {
        int i = 0;
        for (int y = 0; y < 4; y++) {
            List<int> barPos = ReturnBarPos();
            barArray.Add(new List<int>());
            barArray.Add(new List<int>());

            DisplayBars(y, i, barPos[0], barPos[1]);
            barArray[i].Add(barPos[0]); // start
            barArray[i].Add(barPos[1]); // end
            barArray[i].Add(i);         // barIndex

            DisplayBars(y, i + 1, barPos[2], barPos[3]);
            barArray[i + 1].Add(barPos[2]);
            barArray[i + 1].Add(barPos[3]);
            barArray[i + 1].Add(i + 1);

            i += 2;
        }
        problemImage.SetActive(true);
        yield return new WaitForSeconds(1.7f);

        CreateOptions(CalculateAnswer());
    }
    private List<int> ReturnBarPos()
    {
        int dividingPos = random.Next(1, 6);

        List<int> numbers = ReturnRandomNumbers(0, dividingPos + 1);
        List<int> secondNumbers = ReturnRandomNumbers(dividingPos, 7);
        
        numbers.AddRange(secondNumbers);
        return numbers;
    }

    private void DisplayBars(int row, int barIndex, int start, int end)
    {
        var displayingBar = Instantiate(bar, spawnPoint);
        var barTransform = displayingBar.GetComponent<RectTransform>();

        barTransform.anchoredPosition = new Vector2(ORIGIN_X + (start * LENGTH), ORIGIN_Y + (row * SPACING));
        barTransform.sizeDelta = new Vector2(LENGTH * (end - start), barTransform.rect.height);

        displayingBar.GetComponent<Image>().color = barColors[barIndex];
    }
    
    private int CalculateAnswer()
    {
        // 최적해 : 끝나는 시간이 제일 빠른 강의부터 고름
        List<List<int>> sortedBarArray = barArray.OrderBy(list => list[1]).ToList();
        List<int> endTime = new List<int> { sortedBarArray[0][1] };
        answerBarIndexes.Add(sortedBarArray[0][2]);

        for (int i = 1; i < 8; i++) {
            if (endTime[^1] <= sortedBarArray[i][0]) {
                endTime.Add(sortedBarArray[i][1]);
                answerBarIndexes.Add(sortedBarArray[i][2]);
            }
        }
        return endTime.Count;
    }

    private List<int> ReturnRandomNumbers(int minVal, int maxVal, int count = 2)
    {
        // minVal 포함, maxVal 미포함
        return Enumerable.Range(minVal, maxVal - minVal)
            .OrderBy(x => random.Next())
            .Take(count)
            .OrderBy(x => x)
            .ToList();
    }

    private List<int> ReturnOptionValues(int answerValue)
    {
        List<int> numbers = Enumerable.Range(1, 8).ToList();
        numbers.Remove(answerValue);

        return numbers.OrderBy(x => random.Next())
            .Take(3)
            .OrderBy(x => x)
            .ToList();
    }

    private void CreateOptions(int answerValue)
    {
        List<int> indexList = new List<int> { 0, 1, 2, 3 };
        List<int> optionValues = ReturnOptionValues(answerValue);

        for (int i = 0; i < 3; i++) {
            int idx = random.Next(0, indexList.Count);
            int randomIdx = indexList[idx];
            indexList.RemoveAt(idx);

            GameObject option = Instantiate(optionUI, pm.optionTransforms[randomIdx]);
            option.GetComponent<TextMeshProUGUI>().text = $"{optionValues[i]}";
            spawnedOptions.Add(option);
        }
        int answerIndex = indexList[0];
        GameObject answerOption = Instantiate(optionUI, pm.optionTransforms[answerIndex]);
        answerOption.GetComponent<TextMeshProUGUI>().text = $"{answerValue}";
        spawnedOptions.Add(answerOption);

        AnswerManager.instance.SetProblemAnswer(answerIndex);
        Debug.Log($"정답 인덱스 : {(AnswerButton) answerIndex}");
    }

    private void HighlightAnswerBars()
    {
        var bars = spawnPoint.GetComponentsInChildren<Image>();

        for (int i = 0; i < 8; i++) {
            if (answerBarIndexes.Contains(i))
                continue;

            Color greyedColor = bars[i].color;
            greyedColor.a = 0.2f;
            bars[i].color = greyedColor;
        }
    }
}
