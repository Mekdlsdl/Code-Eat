using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ListProblem : MonoBehaviour
{
    private ProblemManager pm;
    [SerializeField] private GameObject guide, elementBundle, arrow, optionUI;
    [SerializeField] private TextMeshProUGUI problemText;
    [SerializeField] private List<Sprite> optionSprites;
    [SerializeField] private List<RuntimeAnimatorController> animTypes;

    private List<Sprite> existingOptions = new List<Sprite>();
    private List<GameObject> spawnedOptions = new List<GameObject>();

    private Animator[] enemyAnims;
    private Sprite answerSprite;
    
    private bool showOnce = false;

    void OnEnable()
    {
        pm = ProblemManager.instance;
        enemyAnims = elementBundle.GetComponentsInChildren<Animator>();
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
            arrow.SetActive(true);
        }  
    }

    IEnumerator BeginProblem()
    {
        yield return new WaitForSeconds(3f);
        guide.SetActive(false);
        
        StartCoroutine(CreateProblem());
    }

    IEnumerator CreateProblem()
    {
        List<Sprite> candidates = new List<Sprite>();

        foreach (Animator anim in enemyAnims) {
            int randomIndex = Random.Range(0, animTypes.Count);
            var enemy = animTypes[randomIndex];
            anim.runtimeAnimatorController = enemy;
            candidates.Add(optionSprites[randomIndex]);
        }
        elementBundle.SetActive(true);
        yield return new WaitForSeconds(1.7f);

        int targetIndex = Random.Range(0, 5);
        problemText.text = $"A[{targetIndex}]";
        problemText.gameObject.SetActive(true);

        arrow.transform.localPosition = new Vector2(enemyAnims[targetIndex].transform.localPosition.x, arrow.transform.localPosition.y);
        
        answerSprite = candidates[targetIndex];
        existingOptions.Add(answerSprite);
        CreateOptions();
    }

    private void CreateOptions()
    {
        List<int> indexList = new List<int> { 0, 1, 2, 3 };

        for (int i = 0; i < 3; i++) {
            int idx = Random.Range(0, indexList.Count);
            int randomIdx = indexList[idx];
            indexList.RemoveAt(idx);

            GameObject option = Instantiate(optionUI, pm.optionTransforms[randomIdx]);
            spawnedOptions.Add(option);
            
            while (true) {
                Sprite enemy = optionSprites[Random.Range(0, optionSprites.Count)];

                if (IsUniqueOption(enemy)) {
                    option.GetComponent<Image>().sprite = enemy;
                    existingOptions.Add(enemy);
                    break;
                }
            }
        }
        int answerIndex = indexList[0];
        GameObject answerOption = Instantiate(optionUI, pm.optionTransforms[answerIndex]);
        spawnedOptions.Add(answerOption);
        answerOption.GetComponent<Image>().sprite = answerSprite;

        AnswerManager.instance.SetProblemAnswer(answerIndex);
        Debug.Log($"정답 인덱스 : {(AnswerButton) answerIndex}");
    }

    private bool IsUniqueOption(Sprite input_enemy)
    {
        return !existingOptions.Contains(input_enemy);
    }
}
