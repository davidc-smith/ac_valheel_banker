using Decal.Adapter.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ValHeelBanker
{
  public static class AmountUtils
  {
    public static bool TryParseAmount(string amount, out int total)
    {
      amount = amount.Replace(",", "").Replace(".","").Replace(" ","").ToLower().Trim();

      if (!ContainsUnSupportedAlphaCharacters(amount) && ContainsSupportedAlphaCharacters(amount))
      {
        var format = MatchSupportedAlphaCharacters(amount);
        long value;
        if (!Int64.TryParse(amount.Replace(format, ""), out value))
        {
          total = 0;
          return false;
        }
        switch (format)
        {
          case "k":
            value = value * 1000;
            break;
          case "m":
            value = value * 1000000;
            break;
          case "b":
            value = value * 1000000000;
            break;
        }
        if (value > Int32.MaxValue)
        {
          total = 0;
          return false;
        }
        total = (int)value;
        return true;
      }
      else
      {
        int value;
        if (Int32.TryParse(amount, out value))
        {
          total= value;
          return true;
        }
      }
      total = 0;
      return false;
    }

    private static bool ContainsSupportedAlphaCharacters(string strToCheck)
    {
      Regex rg = new Regex(@"[bmk]$");
      return rg.IsMatch(strToCheck);
    }

    private static string MatchSupportedAlphaCharacters(string strToCheck)
    {
      Regex rg = new Regex(@"[bmk]$");
      return rg.Match(strToCheck).Value;
    }

    private static bool ContainsUnSupportedAlphaCharacters(string strToCheck)
    {
      Regex rg = new Regex(@"[ac-jln-z]$");
      return rg.IsMatch(strToCheck);
    }

    public static string FormattedNumber(int number)
    {
      return FormattedNumber((long)number);
    }
    public static string FormattedNumber(long number)
    {
      List<Tuple<int, int>> sets = new List<System.Tuple<int, int>>()
  {
    new Tuple<int,int>(0,3),
    new Tuple<int,int>(3,3),
    new Tuple<int,int>(6,3),
    new Tuple<int,int>(9,3),
    new Tuple<int,int>(12,3),
    new Tuple<int,int>(15,3)
  };
      int setsNumber = (int)(number.ToString().Length / 3);
      var workingSets = sets.Take(setsNumber);
      var workingNumber = Reverse(number.ToString());
      string tempNumber = string.Empty;
      int last = 0;
      foreach (var set in workingSets)
      {
        tempNumber += workingNumber.Substring(set.Item1, set.Item2);
        tempNumber += " ";
        last = set.Item1 + set.Item2;
      }
      tempNumber += workingNumber.Substring(last);
      tempNumber = Reverse(tempNumber);
      return tempNumber.Trim();
    }

    private static string Reverse(string value)
    {
      return new string(value.ToCharArray().Reverse().ToArray());
    }
  }
}
