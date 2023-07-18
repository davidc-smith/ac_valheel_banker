using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValHeelBanker
{
  internal class Transfer
  {
    public string AccountNumber { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }
    public CurrencyOptions Currency { get; set; }

    public Transfer()
    {
      AccountNumber= string.Empty;
      Name= string.Empty;
      Amount= 0;
      Currency = CurrencyOptions.Pyreal;
    }

    public enum CurrencyOptions
    {
      Pyreal = 0,
      Ashcoin = 1,
      Luminance = 2
    }
  }
}
