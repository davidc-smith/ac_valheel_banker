using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValHeelBanker.Models
{
  internal class BankAccountBalance
  {
    public int Pyreals { get; set; }
    public int Luminance { get; set; }
    public int AshCoin { get; set; }
    public int PyrealSavings { get; set; }  

    public BankAccountBalance()
    {
      Pyreals= 0;
      Luminance= 0;
      AshCoin= 0;
      PyrealSavings= 0;
    }
  }
}
