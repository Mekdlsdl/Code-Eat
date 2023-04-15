using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
public class StackProblem : MonoBehaviour
{
    [System.NonSerialized] public ProblemManager pm;

    [SerializeField] private GameObject guide, fruitBundle, codeUI, stackOption;
    [SerializeField] private Transform codeTransform;
    [SerializeField] private List<Sprite> fruitList;
    [SerializeField] private List<Image> fruitImages;

    private Stack<Sprite> fruitStack = new Stack<Sprite>();
    private static System.Random rng = new System.Random();
    private List<Stack<Sprite>> optionStackList;

    private const string PUSH = "PUSH(      )";
    private const string POP = "POP(      )";
    private string [][] arr = { 
        new string [] { PUSH, POP, POP },
        new string [] { PUSH, PUSH, POP, POP },
        new string [] { PUSH, POP, POP, POP },
        new string [] { PUSH, PUSH, POP, POP, POP }
    };

    /*
        StackFruit() : 보기의 스택을 랜덤 과일로 채움
        ShowFruit() : 보기 스택을 게임화면에 표시
        SetProblem() : 명령어 조합을 결정, 명령어 결과를 보기 스택에 반영, 명령어를 게임화면에 표시
        SetOptions() : 선택지 스택을 3개 생성, 선택지 스택 게임화면에 표시, 남은 선택지 => 보기 스택

        유의점 ) 
        1. 문제가 제시하는 명령어의 결과가 오류가 나는 결과여서는 안된다.
        2. 정답 인덱스가 항상 같아서는 안된다.
        3. 선택지끼리 달라야 한다.
        4. 선택지 중 하나만 정답이여야 한다.
    */

    void OnEnable()
    {
        StartCoroutine(BeginProblem());
    }

    IEnumerator BeginProblem()
    {
        yield return new WaitForSeconds(2f);
        guide.SetActive(false);

        StackFruit();
        ShowFruit(fruitStack, fruitImages);
        fruitBundle.SetActive(true);
        yield return new WaitForSeconds(1f);

        StartCoroutine(SetProblem());
    }

    private void StackFruit()
    {
        fruitStack.Clear();

        for (int i = 0; i < 3; i++) {
            int idx = Random.Range(0, fruitList.Count);
            fruitStack.Push(fruitList[idx]);
        }
    }

    private void ShowFruit(Stack<Sprite> fruit_stack, List<Image> image_list)
    {
        List<Sprite> fruit_list = fruit_stack.ToList();
        fruit_list.Reverse();
        var imageAndFruit = fruit_list.Zip(image_list, (s, i) => new { stack = s, image = i });

        foreach (var imageFruit in imageAndFruit) {
            imageFruit.image.sprite = imageFruit.stack;
            imageFruit.image.gameObject.SetActive(true);
        }
    }

    private IEnumerator SetProblem()
    {
        string [] selectedArr = arr[Random.Range(0, arr.Length)];
        var shuffledArr = selectedArr.OrderBy(a => rng.Next()).ToList();
        Sprite fruit;
        
        foreach (string code in shuffledArr) {
            GameObject code_ui = Instantiate(codeUI, codeTransform);
            code_ui.GetComponent<TextMeshProUGUI>().text = code;

            switch (code) {
                case PUSH:
                    fruit = fruitList[Random.Range(0, fruitList.Count)];
                    fruitStack.Push(fruit);

                    Transform target = code_ui.transform.GetChild(0);
                    target.GetComponent<Image>().sprite = fruit;
                    target.gameObject.SetActive(true);
                    break;
                
                case POP:
                    fruitStack.Pop();
                    break;
            }
            yield return new WaitForSeconds(0.2f);
        }
        SetOptions();
    }

    private void SetOptions()
    {
        optionStackList = new List<Stack<Sprite>>();
        List<int> indexList = new List<int> { 0, 1, 2, 3 };

        for (int i = 0; i < 3; i++) {
            int idx = Random.Range(0, indexList.Count);
            int randomIdx = indexList[idx];
            indexList.RemoveAt(idx);
            GameObject option = Instantiate(stackOption, pm.optionTransforms[randomIdx]);
            
            Stack<Sprite> optionStack = new Stack<Sprite>();
            int optionCount = 0;
            
            while (optionCount < 3) {
                for (int j = 0; j < 3; j++) {
                    int randIdx = Random.Range(-1, fruitList.Count);
                    if (randIdx < 0)
                        break;
                    
                    optionStack.Push(fruitList[randIdx]);
                }
                if (IsUniqueStack(optionStack)) {
                    StackOption optionInfo = option.GetComponent<StackOption>();
                    ShowFruit(optionStack, optionInfo.fruitImages);
                    optionStackList.Add(optionStack);
                    optionCount ++;
                }
            }
        }

        int answerIndex = indexList[0];
        GameObject answerOption = Instantiate(stackOption, pm.optionTransforms[answerIndex]);
        StackOption answerOptionInfo = answerOption.GetComponent<StackOption>();
        ShowFruit(fruitStack, answerOptionInfo.fruitImages);
        
        AnswerManager.instance.SetProblemAnswer(answerIndex);
        Debug.Log($"정답 인덱스 : {(AnswerButton) answerIndex}");
    }

    private bool IsUniqueStack(Stack<Sprite> input_stack)
    {
        foreach (Stack<Sprite> stack in optionStackList) {
            if (!CompareStacks(stack, input_stack))
                return true;
        }
        return !CompareStacks(fruitStack, input_stack);
    }
    private bool CompareStacks(Stack<Sprite> stack1, Stack<Sprite> stack2)
    {
        if (stack1.Count != stack2.Count)
            return false;

        if (stack1.Count == 0)
            return true;
        
        var arr1 = stack1.ToArray();
        var arr2 = stack2.ToArray();

        for (int i = 0; i < arr1.Length; i++) {
            if (arr1[i] != arr2[i])
                return false;
        }
        return true;
    }
}
