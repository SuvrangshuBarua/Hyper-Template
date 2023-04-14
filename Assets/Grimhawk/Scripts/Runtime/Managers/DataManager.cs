using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Text = TMPro.TextMeshProUGUI;
using grimhawk.core;
using MyBox;
using DG.Tweening;

public class DataManager : GameBehavior
{
    private const string CURRENCY_KEY = "CURRENCY_KEY";
    private const string LEVEL_KEY = "LEVEL_KEY";
    [ReadOnly]
    private const int defaultCurrencyAmount = 500;
    private const int defaultLevelStartValue = 0;
    private IEnumerator _runningCoroutine;

    private PersistantData<ulong> _totalCurrency;
    private PersistantData<int> _level;

    public Text currencyTextUIElement;
    public Text deductTextUIElement;

    public int Level
    {
        private set
        {
            if (_level == null)
            {
                _level = new PersistantData<int>(LEVEL_KEY, defaultLevelStartValue);
            }
            _level.value = value;

        }

        get
        {
            if (_level == null)
            {
                _level = new PersistantData<int>(LEVEL_KEY, defaultLevelStartValue);
            }
            return _level;
        }
    }
    public ulong Currency
    {
        private set
        {
            if (_totalCurrency == null)
            {
                _totalCurrency = new PersistantData<ulong>(CURRENCY_KEY, defaultCurrencyAmount);
#if UNITY_EDITOR
                Debug.Log("<color=green>Currency Wasn't Saved || Just Decalred</color>");
#endif
            }
            _totalCurrency.value = value;

        }

        get
        {
            if (_totalCurrency == null)
            {
                _totalCurrency = new PersistantData<ulong>(CURRENCY_KEY, defaultCurrencyAmount);
#if UNITY_EDITOR
                Debug.Log("<color=green>Currency Wasn't Saved || Just Decalred</color>");
#endif
            }
            return _totalCurrency;
        }
    }
    
    public void Init(System.Action OnDataLoadedAction)
    {
        _level = new PersistantData<int>(LEVEL_KEY, defaultLevelStartValue);
        _totalCurrency = new PersistantData<ulong>(CURRENCY_KEY, defaultCurrencyAmount);
        currencyTextUIElement.text = CurrencyFormatter.FormattedCurrency(Currency);
        OnDataLoadedAction?.Invoke();
    }
    public void IncrementLevel() => Level++;
    public void AddCurrency(ulong amount, float duration = 2f)
    {
        IncreaseCurrency(amount, duration);
    }

    public void DeductCurrency(ulong amount, float durartion = 2f)
    {
        if(_runningCoroutine != null)
        {
            StopCoroutine(_runningCoroutine );  
            currencyTextUIElement.text = CurrencyFormatter.FormattedCurrency(Currency);
        }
        _runningCoroutine = DeductAnimationRoutine(amount);
        StartCoroutine(_runningCoroutine);
        Currency -= amount;

        currencyTextUIElement.text = CurrencyFormatter.FormattedCurrency(Currency);
    }

    private void IncreaseCurrency(ulong extraAmount, float duration = 2f)
    {
        if(_runningCoroutine!=null)
        {
            StopCoroutine(_runningCoroutine);
            currencyTextUIElement.text = CurrencyFormatter.FormattedCurrency(Currency);

        }
        _runningCoroutine = CurrencyFormatter.CountAnimation(Currency, Currency + extraAmount, currencyTextUIElement, duration);
        StartCoroutine(_runningCoroutine);

        Currency += extraAmount;

    }

    private IEnumerator DeductAnimationRoutine(ulong amount)
    {
        yield return null;
        Sequence deductSequence = DOTween.Sequence();
        deductSequence.PrependCallback(() =>
        {
            deductTextUIElement.SetAlpha(1);
            deductTextUIElement.text = $"-{CurrencyFormatter.FormattedCurrency(amount, 0)}";
        })
                      .Append(deductTextUIElement.rectTransform.DOAnchorPosY(-100, 1f))
                      .Join(deductTextUIElement.DOFade(0, 0.5f).SetDelay(0.5f))
                      .OnComplete(() =>
                      {
                          deductTextUIElement.SetAlpha(0);
                          deductTextUIElement.rectTransform.anchoredPosition = Vector2.zero;
                      });
        
                                         
    }
}

public class CurrencyFormatter
{
    public static string FormattedCurrency(int amount, int decimalPlace = 2)
    {
        if ((long)amount >= 1000000000000000)
            return $"{((double)amount / 1000000000000000.0).ToString($"F{decimalPlace}")}Q";
        else if ((long)amount >= 1000000000000)
            return $"{((double)amount / 1000000000000.0).ToString($"F{decimalPlace}")}T";
        else if ((long)amount >= 1000000000)
            return $"{((double)amount / 1000000000.0).ToString($"F{decimalPlace}")}B";
        else if ((long)amount >= 1000000)
            return $"{((double)amount / 1000000.0).ToString($"F{decimalPlace}")}M";
        else if ((long)amount >= 1000)
            return $"{((double)amount / 1000.0).ToString($"F{decimalPlace}")}K";
        else
            return amount.ToString();
    }
    public static string FormattedCurrency(ulong amount, int decimalPlace = 2)
    {
        if (amount >= 1000000000000000)
            return $"{((double)amount / 1000000000000000.0).ToString($"F{decimalPlace}")}Q";
        else if (amount >= 1000000000000)
            return $"{((double)amount / 1000000000000.0).ToString($"F{decimalPlace}")}T";
        else if (amount >= 1000000000)
            return $"{((double)amount / 1000000000.0).ToString($"F{decimalPlace}")}B";
        else if (amount >= 1000000)
            return $"{((double)amount / 1000000.0).ToString($"F{decimalPlace}")}M";
        else if (amount >= 1000)
            return $"{((double)amount / 1000.0).ToString($"F{decimalPlace}")}K";
        else
            return amount.ToString();
    }

    public static IEnumerator CountAnimation(int startAmount, int endAmount, Text text,  float duration, System.Action OnComplete = null)
    {
        int deltaAmount = endAmount - startAmount;

        int perStepCount = Mathf.RoundToInt((deltaAmount / duration) * Time.deltaTime);

        if(perStepCount < 1)
        {
            perStepCount = 1;
            duration = Time.deltaTime /  perStepCount * deltaAmount;
        }

        int currentAmount = startAmount;
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            currentAmount += perStepCount;
            if(currentAmount < endAmount) 
            {
                text.text = FormattedCurrency(currentAmount);
            }
        }
        text.text = FormattedCurrency(endAmount);

        OnComplete?.Invoke();
    }
    public static IEnumerator CountAnimation(ulong startAmount, ulong endAmount, Text textComponent, float duration, System.Action OnComplete = null)
    {
        ulong deltaAmount = endAmount - startAmount;

        int perStepCount = Mathf.RoundToInt((deltaAmount / duration) * Time.deltaTime);

        if (perStepCount < 1)
        {
            perStepCount = 1;
            duration = Time.deltaTime / deltaAmount * perStepCount;
        }

        ulong currentAmount = startAmount;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            currentAmount += (ulong)perStepCount;
            if (currentAmount < endAmount)
            {
                textComponent.text = FormattedCurrency(currentAmount);
            }
        }
        textComponent.text = FormattedCurrency(endAmount);

        OnComplete?.Invoke();
    }
}
