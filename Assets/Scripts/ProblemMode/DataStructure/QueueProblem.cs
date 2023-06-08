using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class QueueProblem : MonoBehaviour
{
    private ProblemManager pm;
    
    [SerializeField] private float displayDelay;
    [SerializeField] List<GameObject> boxes;
    [SerializeField] List<Sprite> vegetables;
    [SerializeField] private GameObject guide, queueUI, optionUI, frontArrow;
    [SerializeField] private TextMeshProUGUI codeUI;

    private List<string> codeList = new List<string>();
    private Queue<Sprite> vegetableQueue = new Queue<Sprite>();
    private List<Sprite> vegetableOptions = new List<Sprite>();
    private List<GameObject> spawnedOptions = new List<GameObject>();

    private const string ENQUEUE = "<color=#49AB81>enqueue(   )</color>";
    private const string DEQUEUE = "<color=#FF647E>dequeue()</color>";
    private string[] arr = { ENQUEUE, DEQUEUE };

    int front = -1, rear = -1;
    private bool showOnce = false;


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
            DisplayQueue();
        }
    }

    IEnumerator BeginProblem()
    {
        yield return new WaitForSeconds(3f);

        guide.SetActive(false);
        queueUI.SetActive(true);

        yield return new WaitForSeconds(1f);

        StartCoroutine(CreateProblem());
    }

    private IEnumerator CreateProblem()
    {
        int totalCount = Random.Range(2, 5);

        while (vegetableQueue.Count < totalCount && rear < 3) {
            string code = arr[Random.Range(0, 2)];

            if (vegetableQueue.Count <= 1 && code == DEQUEUE)
                continue;
            
            if (code == ENQUEUE)
                EnQueue();
            else
                DeQueue();
            
            DisplayCode(code);
            yield return new WaitForSeconds(displayDelay);
        }

        codeUI.transform.GetChild(0).gameObject.SetActive(false);
        DOTween.Rewind("ShowQueueCode");
        CreateOptions();
    }

    private void EnQueue()
    {
        Sprite vegetable = vegetables[Random.Range(0, vegetables.Count)];
        vegetableQueue.Enqueue(vegetable);

        if (front < 0) {
            front++;
            frontArrow.SetActive(true);
        }
        rear++;

        boxes[rear].transform.GetChild(0).gameObject.SetActive(true);
        boxes[rear].transform.GetChild(1).gameObject.GetComponent<Image>().sprite = vegetable;

        codeUI.transform.GetChild(0).GetComponent<Image>().sprite = vegetable;
        codeUI.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void DeQueue()
    {
        vegetableQueue.Dequeue();
        boxes[front].transform.GetChild(0).gameObject.SetActive(false);
        
        codeUI.transform.GetChild(0).gameObject.SetActive(false);
        
        front++;
        frontArrow.transform.localPosition = new Vector2(boxes[front].transform.localPosition.x, frontArrow.transform.localPosition.y);
    }

    private void DisplayQueue()
    {
        for (int i = front; i <= rear; i++) {
            boxes[i].transform.GetChild(0).gameObject.SetActive(false);
            boxes[i].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void DisplayCode(string code)
    {
        codeUI.text = code;
        DOTween.Rewind("ShowQueueCode");
        DOTween.Play("ShowQueueCode");
    }

    private void CreateOptions()
    {
        List<int> indexList = new List<int> { 0, 1, 2, 3 };
        vegetableOptions.Add(vegetableQueue.Peek());

        for (int i = 0; i < 3; i++) {
            int idx = Random.Range(0, indexList.Count);
            int randomIdx = indexList[idx];
            indexList.RemoveAt(idx);

            GameObject option = Instantiate(optionUI, pm.optionTransforms[randomIdx]);
            spawnedOptions.Add(option);
            
            while (true) {
                Sprite vegetable = vegetables[Random.Range(0, vegetables.Count)];

                if (IsUniqueOption(vegetable)) {
                    option.GetComponent<Image>().sprite = vegetable;
                    vegetableOptions.Add(vegetable);
                    break;
                }
            }
        }

        int answerIndex = indexList[0];
        GameObject answerOption = Instantiate(optionUI, pm.optionTransforms[answerIndex]);
        spawnedOptions.Add(answerOption);
        answerOption.GetComponent<Image>().sprite = vegetableOptions[0];

        AnswerManager.instance.SetProblemAnswer(answerIndex);
        foreach (Sprite v in vegetableQueue)
            Debug.Log(v.name);
        Debug.Log($"정답 인덱스 : {(AnswerButton) answerIndex}");
    }

    private bool IsUniqueOption(Sprite input_vegetable)
    {
        return !vegetableOptions.Contains(input_vegetable);
    }
}
