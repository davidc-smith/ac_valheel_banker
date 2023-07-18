using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using ValHeelBanker.Models;

namespace ValHeelBanker
{
  internal static class BankCommands
  {
    public const string BankAccount = @"/bank account";

    public const string BankAccountCallBack = @"[BANK] Account Number:";
    public const string BankAccountBalancesCallBack = @"[BANK] Account Balances: ";
    public const string BankNewAccountBalancesCallBack = @"[BANK] New Account Balances: ";

    public const string DepositPyrealsCommand = @"/bank deposit pyreals";
    public const string DepositPyrealsToSavingsCommand = @"/bank deposit pyrealsavings";
    public const string DepositAshCoinCommand = @"/bank deposit ashcoin";
    public const string DepositLuminanceCommand = @"/bank deposit luminance";
    public const string DepositAttemptNewBalanceCallBack = @"[BANK] New Account Balance: ";
    public const string TransferNewBalanceCallBack = @"[BANK] Your New Account Balance: ";
    

    public const string WithdrawPyrealsCommand = @"/bank withdraw pyreals";
    public const string WithdrawPyrealSavingsCommand = @"/bank withdraw pyrealsavings";
    public const string WithdrawAshCoinCommand = @"/bank withdraw ashcoin";
    public const string WithdrawLuminanceCommand = @"/bank withdraw luminance";
    public const string TransferCommand = @"/bank send <<ACCOUNTNUMBER>> <<CURRENCY>>";

    public const string IncomingTransferFirstPart = @"[BANK TRANSACTION] ";
    public const string IncomingTransferLastPart = @" sent you ";

    public const int MMDValue = 287500;


    public static string GetAccountNumberFromBankAccountResponse(string response)
    {
      return response.Replace(BankAccountCallBack, "").Replace(" ", "").Trim();
    }

    public static BankAccountBalance GetAccountBalancesFromBankAccountBalanceResponse(string response)
    {
      var balance = new BankAccountBalance();
      try
      {
        response = response.Replace(BankAccountBalancesCallBack, "").Replace(BankNewAccountBalancesCallBack,"").Replace(" ", "").Trim();
        var parts = response.Split(new string[] { "||" }, StringSplitOptions.None);
        if (parts.Length > 0)
          balance.Pyreals = Convert.ToInt32(parts[0].Replace("Pyreals", "").Replace(",", ""));

        if (parts.Length > 1)
          balance.Luminance = Convert.ToInt32(parts[1].Replace("Luminance", "").Replace(",", ""));

        if (parts.Length > 2)
          balance.AshCoin = Convert.ToInt32(parts[2].Replace("AshCoin", "").Replace(",", ""));

        if (parts.Length > 3)
          balance.PyrealSavings = Convert.ToInt32(parts[3].Replace("PyrealSavings", "").Replace(",", ""));
      }
      catch(Exception ex) 
      {
        Logger.WriteDebugger(ex);
        Logger.WriteMessage("Failed to parse your Bank Balances.");
      }
      return balance;
    }

    public static BankAccountBalance GetAccountBalanceFromDepositOfPyreals(string response)
    {
      var balance = new BankAccountBalance();
      try
      {
        response = response.Replace(DepositAttemptNewBalanceCallBack, "").Replace(TransferNewBalanceCallBack, "").Replace("Pyreals", "").Replace(",", "").Replace(" ", "").Trim();
        balance.Pyreals = Convert.ToInt32(response);
      }
      catch(Exception ex      )
      {
        Logger.WriteDebugger(ex);
        Logger.WriteMessage("Failed to parse your Pyreal Bank Balance.");
      }
      return balance;
    }

    public static BankAccountBalance GetAccountBalanceFromDepositOfAshCoin(string response)
    {
      var balance = new BankAccountBalance();
      try
      {
        response = response.Replace(DepositAttemptNewBalanceCallBack, "").Replace(TransferNewBalanceCallBack, "").Replace("AshCoin", "").Replace(",", "").Replace(" ", "").Trim();
        balance.AshCoin = Convert.ToInt32(response);
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
        Logger.WriteMessage("Failed to parse your AshCoin Bank Balance.");
      }
      return balance;
    }

    public static BankAccountBalance GetAccountBalanceFromDepositOfPyrealsIntoSavings(string response)
    {
      var balance = new BankAccountBalance();
      try
      {
        response = response.Replace(DepositAttemptNewBalanceCallBack, "").Replace(TransferNewBalanceCallBack, "").Replace("Pyreals in savings", "").Replace(",", "").Replace(" ", "").Trim();
        balance.PyrealSavings = Convert.ToInt32(response);
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
        Logger.WriteMessage("Failed to parse your Pyreal Savings Bank Balance.");
      }
      return balance;
    }

    public static BankAccountBalance GetAccountBalanceFromDepositOfLuminance(string response)
    {
      var balance = new BankAccountBalance();
      try
      {
        response = response.Replace(DepositAttemptNewBalanceCallBack, "").Replace(TransferNewBalanceCallBack, "").Replace("Luminance", "").Replace(",", "").Replace(" ", "").Trim();
        balance.Luminance = Convert.ToInt32(response);
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
        Logger.WriteMessage("Failed to parse your Luminance Bank Balance.");
      }
      return balance;
    }
  }
}
