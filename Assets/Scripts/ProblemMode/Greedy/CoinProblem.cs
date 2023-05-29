using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Random = System.Random;

public class CoinProblem : MonoBehaviour
{
    [SerializeField] private GameObject guide, problemUI, optionUI, answerCoin;
    [SerializeField] private Transform coinTransform;
    [SerializeField] private Sprite won500, won100;
    [SerializeField] private List<GameObject> products;
    [SerializeField] private Animator anim;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] private float INCREASE_SPEED;

    private ProblemManager pm;
    private Random random = new Random();
    private List<GameObject> spawnedOptions = new List<GameObject>();

    private float displayingPrice = 0;
    private int productPrice = 0;

    private int count_500, count_100;
    private int answerValue;

    private bool showPrice = false;
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
        if (showPrice)
            DisplayPrice();

        if (pm.isShowingAnswer && !showOnce) {
            showOnce = true;
            StartCoroutine(ShowAnswerCoins());
        }  
    }

    IEnumerator BeginProblem()
    {
        yield return new WaitForSeconds(3f);
        guide.SetActive(false);
        problemUI.SetActive(true);

        yield return new WaitForSeconds(2.5f);
        ShowProduct();
        SetProductPrice();

        yield return new WaitForSeconds(1f);
        showPrice = true;

        yield return new WaitForSeconds(1.5f);
        DOTween.Rewind("ShowCoin");
        DOTween.Play("ShowCoin");
        
        yield return new WaitForSeconds(0.8f);
        CreateOptions();
    }

    private void ShowProduct()
    {
        products[random.Next(0, products.Count)].SetActive(true);
        anim.Play("ProductAppear");
    }

    private void SetProductPrice()
    {
        (count_500, count_100) = (random.Next(1, 5), random.Next(1, 5));
        productPrice = 500 * count_500 + 100 * count_100;
        answerValue = count_500 + count_100;
    }

    private void DisplayPrice()
    {
        displayingPrice += Time.deltaTime * INCREASE_SPEED;

        if (displayingPrice > productPrice)
        {
            displayingPrice = productPrice;
            showPrice = false;
        }
        priceText.text = displayingPrice.ToString("F0");
    }

    private List<int> ReturnOptionValues(int answerValue)
    {
        List<int> numbers = Enumerable.Range(1, 10).ToList();
        numbers.Remove(answerValue);

        return numbers.OrderBy(x => random.Next())
            .Take(3)
            .OrderBy(x => x)
            .ToList();
    }

    private void CreateOptions()
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

    IEnumerator ShowAnswerCoins()
    {
        for (int i = 0; i < count_500; i++) {
            GameObject coin = Instantiate(answerCoin, coinTransform);
            coin.GetComponent<Image>().sprite = won500;
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < count_100; i++) {
            GameObject coin = Instantiate(answerCoin, coinTransform);
            coin.GetComponent<Image>().sprite = won100;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
