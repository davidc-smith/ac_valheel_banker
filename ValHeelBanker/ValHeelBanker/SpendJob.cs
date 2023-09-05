using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ValHeelBanker
{
  internal class SpendJob
  {
    public SpendTypes SpendType { get; set; }
    public int Amount { get; set; }
    private MainView view;
    private bool JobStarted = false;
    private int AmountLeft = 0;
    private int ActionAmount = 0;

    private long RatingsCost => PluginCore.RatingsIncreasePerPoint;
    private long BankedLuminance => view.BankedLuminance;
    private long CurrentLuminance => PluginCore.Instance.LuminanceCurrent;
   

    public SpendJob(MainView view)
    {
      SpendType = SpendTypes.Vitals;
      Amount = 0;
      this.view=view;
    }

    public SpendJob()
    {
      SpendType = SpendTypes.Vitals;
      Amount = 0;
      view = null;
    }

    public Tuple<bool, string> Validate()
    {
      if(Amount == 0)
        return new Tuple<bool, string>(false, "You need to set how many rating points you wish to buy");

      if ((CurrentLuminance + BankedLuminance) < RatingsCost)
        return new Tuple<bool, string>(false, "You do not have enough luminance to perform this operation");

      var requiredLuminance = RatingsCost * Amount;
      if((CurrentLuminance + BankedLuminance) < requiredLuminance)
        return new Tuple<bool, string>(false, "You do not have enough luminance to perform this operation");

      return new Tuple<bool, string>(true, string.Empty);
    }

    public SpendResult PerformNextStep()
    {
      SpendResult result = null;
      if (!JobStarted)
      {
        JobStarted = true;
        AmountLeft = Amount;
        return PerformAction();
        //if (CanFulfilRequest(AmountLeft))
        //{
        //  ActionAmount = AmountLeft;
        //  return new SpendResult($"{GetBuyCommand()} {AmountLeft}", true, SpendActionTypes.Buy, AmountLeft);
        //}
        //int reqAmount;
        //if (AmountLeft > 30)
        //  reqAmount = 30;
        //else
        //  reqAmount = AmountLeft;

        //long lumRequired = GetRequiredLuminance(reqAmount);
        //if (lumRequired > 0)
        //{
        //  if (BankedLuminance >= lumRequired)
        //  {
        //    return new SpendResult($"{BankCommands.WithdrawLuminanceCommand} {lumRequired}", true, SpendActionTypes.Withdraw, lumRequired);
        //  }
        //  JobStarted = false;
        //  return new SpendResult(string.Empty, false, SpendActionTypes.None, 0, "You do not have enough banked luminance to perform this operation.");
        //}
        //ActionAmount = reqAmount;
        //return new SpendResult($"{GetBuyCommand()} {reqAmount}", true, SpendActionTypes.Buy, reqAmount);
      }
      if (JobStarted)
      {
        if (AmountLeft == 0)
        {
          JobStarted = false;
          return new SpendResult(string.Empty, true, SpendActionTypes.Complete);
        }
        return PerformAction();
      }
      return result;
    }

    private SpendResult PerformAction()
    {
      if (CanFulfilRequest(AmountLeft))
      {
        ActionAmount = AmountLeft;
        return new SpendResult($"{GetBuyCommand()} {AmountLeft}", true, SpendActionTypes.Buy, AmountLeft);
      }
      int reqAmount;
      if (AmountLeft > 30)
        reqAmount = 30;
      else
        reqAmount = AmountLeft;

      long lumRequired = GetRequiredLuminance(reqAmount);
      if (lumRequired > 0)
      {
        if (BankedLuminance >= lumRequired)
        {
          return new SpendResult($"{BankCommands.WithdrawLuminanceCommand} {lumRequired}", true, SpendActionTypes.Withdraw, lumRequired);
        }
        JobStarted = false;
        return new SpendResult(string.Empty, false, SpendActionTypes.None, 0, "You do not have enough banked luminance to perform this operation.");
      }
      ActionAmount = reqAmount;
      return new SpendResult($"{GetBuyCommand()} {reqAmount}", true, SpendActionTypes.Buy, reqAmount);
    }

    private bool CanFulfilRequest(int requestedAmount)
    {
      if (requestedAmount > 30)
        return false;
      if (requestedAmount > (int)(CurrentLuminance / RatingsCost))
        return false;
      return true;
    }

    private long GetRequiredLuminance(int requestedAmount)
    {
      long requiredLuminance = requestedAmount * RatingsCost;
      if(requiredLuminance > CurrentLuminance)
        return requiredLuminance - CurrentLuminance;
      return 0;
    }

    public void UpdateSpentRatings(int amount)
    {
      AmountLeft -= amount;
    }

    private string GetBuyCommand()
    {
      switch (SpendType)
      {
        case SpendTypes.Vitals:
          return BankCommands.BuyVitalityCommand;
        case SpendTypes.DamageIncrease:
          return BankCommands.BuyDestructionCommand;
        case SpendTypes.DamageReduction:
          return BankCommands.BuyInvulnerabilityCommand;
        case SpendTypes.CritDamageIncrease:
          return BankCommands.BuyGloryCommand;
        case SpendTypes.CritDamageReduction:
          return BankCommands.BuyTemperanceCommand;
      }
      return string.Empty;// 226 050 000
    }

    public void ClearJob()
    {
      SpendType = SpendTypes.None;
      Amount = 0;
      JobStarted= false;
      AmountLeft= 0;
      ActionAmount = 0;
    }
  }

  public enum SpendTypes
  { 
    None = -1,
    DamageIncrease = 0,
    DamageReduction,
    CritDamageIncrease,
    CritDamageReduction,
    Vitals
  }

  public enum SpendActionTypes
  { 
    None,
    Withdraw,
    Buy,
    Complete
  }

  public class SpendResult
  { 
    public string Command { get; private set; }
    public bool IsValid { get; private set; }
    public string ErrorMessage { get; private set; }
    public SpendActionTypes Action { get; private set; }
    public long Amount { get; private set; }
    public SpendResult(string command, bool isValid, SpendActionTypes action = SpendActionTypes.None, long amount = 0, string errorMessage = "")
    {
      Command = command;
      IsValid = isValid;
      ErrorMessage = errorMessage;
      Action = action;
      Amount = amount;
    }
  }

}
