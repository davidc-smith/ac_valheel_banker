using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValHeelBanker.Models
{
  internal class BankAccountBalance
  {
    public int Pyreals { get; set; }
    public long Luminance { get; set; }
    public int AshCoin { get; set; }

    public BankAccountBalance()
    {
      Pyreals= 0;
      Luminance= 0;
      AshCoin= 0;
    }
  }
}
