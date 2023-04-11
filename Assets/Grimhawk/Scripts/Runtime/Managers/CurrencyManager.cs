using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : GameBehavior
{
    
}

public class Currency
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
}
