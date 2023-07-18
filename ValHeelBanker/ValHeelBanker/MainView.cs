using Decal.Adapter.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Windows.Forms;
using VirindiViewService;
using VirindiViewService.Controls;
using static ValHeelBanker.Transfer;

namespace ValHeelBanker
{
  internal class MainView
  {

    #region ... Properties ...

    private PluginCore _instance;
    private Settings _settings;
    private bool EatBankingText = false;
    private Transfer _transfer = new Transfer();

    private Dictionary<int, CurrencyOptions> CurrencyMap= new Dictionary<int, CurrencyOptions>();

    private Timer _timer;
    private DateTime PyrealDepositTimeStamp;
    private DateTime LuminanceDepositTimeStamp;

    #endregion

    #region .. GUI Controls ...

    HudStaticText lblAccountNumber;
    HudStaticText lblPyreals;
    HudStaticText lblPyrealSavings;
    HudStaticText lblLuminance;
    HudStaticText lblAshCoins;

    HudTextBox txtDepositPyreals;
    HudTextBox txtDepositLuminance;
    HudTextBox txtDepositAshCoin;
    HudTextBox txtDepositPyrealSavings;
    
    HudButton cmdDepositPyreals;
    HudButton cmdDepositMaxPyreals;
    HudButton cmdDepositLuminance;
    HudButton cmdDepositMaxLuminance;
    HudButton cmdDepositAshCoin;
    HudButton cmdDepositMaxAshCoin;
    HudButton cmdDepositPyrealSavings;
    HudButton cmdDepositMaxPyrealSavings;

    HudCheckBox chkDepositPyrealsPeriodically;
    HudCheckBox chkDepositLuminancePeriodically;

    HudButton cmdRequestBalances;

    HudTextBox txtWithdrawPyreals;
    HudTextBox txtWithdrawLuminance;
    HudTextBox txtWithdrawAshCoin;
    HudTextBox txtWithdrawPyrealSavings;

    HudButton cmdWithdrawPyreals;
    HudButton cmdWithdrawMaxPyreals;
    HudButton cmdWithdrawLuminance;
    HudButton cmdWithdrawMaxLuminance;
    HudButton cmdWithdrawAshCoin;
    HudButton cmdWithdrawMaxAshCoin;
    HudButton cmdWithdrawPyrealSavings;
    HudButton cmdWithdrawMaxPyrealSavings;

    HudList lstCharacters;
    HudList lstFriends;

    HudTextBox txtFriendAccountNumber;
    HudTextBox txtFriend;
    HudButton cmdAddFriend;

    HudStaticText lblTransferToPlayer;
    HudCombo cboTransferCurrency;
    HudTextBox txtTransferAmount;
    HudButton cmdVerifyTransfer;
    HudButton cmdTransfer;
    HudStaticText lblTransferStatus;
    HudStaticText lblTransferStatusLine2;

    HudTextBox txtPyrealInterval;
    HudCombo cboPyrealDepositType;
    HudTextBox txtPyrealDepositAmount;
    HudTextBox txtLuminanceInterval;
    HudCombo cboLuminanceDepositType;
    HudTextBox txtLuminanceDepositAmount;

    HudButton cmdSaveSettings;

    #endregion

    #region ... Constructor ...

    public MainView(PluginCore instance)
    {
      _instance = instance;
      _settings = Settings.Load(Logger.BasePath);

      PyrealDepositTimeStamp = DateTime.Now;
      LuminanceDepositTimeStamp = DateTime.Now;

      CurrencyMap.Add(0, CurrencyOptions.Pyreal);
      CurrencyMap.Add(1, CurrencyOptions.Ashcoin);
      CurrencyMap.Add(2, CurrencyOptions.Luminance);

      lblAccountNumber = (HudStaticText)_instance.MainControls["lblAccountNumber"];
      lblPyreals = (HudStaticText)_instance.MainControls["lblPyreals"];
      lblPyrealSavings = (HudStaticText)_instance.MainControls["lblPyrealSavings"];
      lblLuminance = (HudStaticText)_instance.MainControls["lblLuminance"];
      lblAshCoins = (HudStaticText)_instance.MainControls["lblAshCoins"];
      cmdRequestBalances = (HudButton)_instance.MainControls["cmdRequestBalances"];
      txtDepositPyreals = (HudTextBox)_instance.MainControls["txtDepositPyreals"];
      txtDepositLuminance = (HudTextBox)_instance.MainControls["txtDepositLuminance"];
      txtDepositAshCoin = (HudTextBox)_instance.MainControls["txtDepositAshCoin"];
      txtDepositPyrealSavings = (HudTextBox)_instance.MainControls["txtDepositPyrealSavings"];
      cmdDepositPyreals = (HudButton)_instance.MainControls["cmdDepositPyreals"];
      cmdDepositMaxPyreals = (HudButton)_instance.MainControls["cmdDepositMaxPyreals"];
      cmdDepositLuminance = (HudButton)_instance.MainControls["cmdDepositLuminance"];
      cmdDepositMaxLuminance = (HudButton)_instance.MainControls["cmdDepositMaxLuminance"];
      cmdDepositAshCoin = (HudButton)_instance.MainControls["cmdDepositAshCoin"];
      cmdDepositMaxAshCoin = (HudButton)_instance.MainControls["cmdDepositMaxAshCoin"];
      cmdDepositPyrealSavings = (HudButton)_instance.MainControls["cmdDepositPyrealSavings"];
      cmdDepositMaxPyrealSavings = (HudButton)_instance.MainControls["cmdDepositMaxPyrealSavings"];
      chkDepositPyrealsPeriodically = (HudCheckBox)_instance.MainControls["chkDepositPyrealsPeriodically"];
      chkDepositLuminancePeriodically = (HudCheckBox)_instance.MainControls["chkDepositLuminancePeriodically"];

      txtWithdrawPyreals = (HudTextBox)_instance.MainControls["txtWithdrawPyreals"];
      txtWithdrawLuminance = (HudTextBox)_instance.MainControls["txtWithdrawLuminance"];
      txtWithdrawAshCoin = (HudTextBox)_instance.MainControls["txtWithdrawAshCoin"];
      txtWithdrawPyrealSavings = (HudTextBox)_instance.MainControls["txtWithdrawPyrealSavings"];
      cmdWithdrawPyreals = (HudButton)_instance.MainControls["cmdWithdrawPyreals"];
      cmdWithdrawMaxPyreals = (HudButton)_instance.MainControls["cmdWithdrawMaxPyreals"];
      cmdWithdrawLuminance = (HudButton)_instance.MainControls["cmdWithdrawLuminance"];
      cmdWithdrawMaxLuminance = (HudButton)_instance.MainControls["cmdWithdrawMaxLuminance"];
      cmdWithdrawAshCoin = (HudButton)_instance.MainControls["cmdWithdrawAshCoin"];
      cmdWithdrawMaxAshCoin = (HudButton)_instance.MainControls["cmdWithdrawMaxAshCoin"];
      cmdWithdrawPyrealSavings = (HudButton)_instance.MainControls["cmdWithdrawPyrealSavings"];
      cmdWithdrawMaxPyrealSavings = (HudButton)_instance.MainControls["cmdWithdrawMaxPyrealSavings"];

      lstCharacters = (HudList)_instance.MainControls["lstCharacters"];
      lstFriends = (HudList)_instance.MainControls["lstFriends"];

      txtFriendAccountNumber = (HudTextBox)_instance.MainControls["txtFriendAccountNumber"];
      txtFriend = (HudTextBox)_instance.MainControls["txtFriend"];
      cmdAddFriend = (HudButton)_instance.MainControls["cmdAddFriend"];

      cmdVerifyTransfer = (HudButton)_instance.MainControls["cmdVerifyTransfer"];
      txtTransferAmount = (HudTextBox)_instance.MainControls["txtTransferAmount"];
      lblTransferToPlayer = (HudStaticText)_instance.MainControls["lblTransferToPlayer"];
      cboTransferCurrency = (HudCombo)_instance.MainControls["cboTransferCurrency"];
      cmdTransfer = (HudButton)_instance.MainControls["cmdTransfer"];
      lblTransferStatus = (HudStaticText)_instance.MainControls["lblTransferStatus"];
      lblTransferStatusLine2 = (HudStaticText)_instance.MainControls["lblTransferStatusLine2"];

      txtPyrealInterval         = (HudTextBox)_instance.MainControls["txtPyrealInterval"];
      cboPyrealDepositType      = (HudCombo  )_instance.MainControls["cboPyrealDepositType"];
      txtPyrealDepositAmount    = (HudTextBox)_instance.MainControls["txtPyrealDepositAmount"];
      txtLuminanceInterval      = (HudTextBox)_instance.MainControls["txtLuminanceInterval"];
      cboLuminanceDepositType   = (HudCombo  )_instance.MainControls["cboLuminanceDepositType"];
      txtLuminanceDepositAmount = (HudTextBox)_instance.MainControls["txtLuminanceDepositAmount"];

      cmdSaveSettings = (HudButton)_instance.MainControls["cmdSaveSettings"];

      cmdRequestBalances.Hit += CmdRequestBalances_Hit;
      cmdDepositPyreals.Hit += CmdDepositPyreals_Hit;
      cmdDepositMaxPyreals.Hit += CmdDepositMaxPyreals_Hit;
      cmdDepositLuminance.Hit += CmdDepositLuminance_Hit;
      cmdDepositMaxLuminance.Hit += CmdDepositMaxLuminance_Hit;
      cmdDepositAshCoin.Hit += CmdDepositAshCoin_Hit;
      cmdDepositMaxAshCoin.Hit += CmdDepositMaxAshCoin_Hit;
      cmdDepositPyrealSavings.Hit += CmdDepositPyrealSavings_Hit;
      cmdDepositMaxPyrealSavings.Hit += CmdDepositMaxPyrealSavings_Hit;

      cmdWithdrawPyreals.Hit += CmdWithdrawPyreals_Hit;
      cmdWithdrawMaxPyreals.Hit += CmdWithdrawMaxPyreals_Hit;
      cmdWithdrawLuminance.Hit += CmdWithdrawLuminance_Hit;
      cmdWithdrawMaxLuminance.Hit += CmdWithdrawMaxLuminance_Hit;
      cmdWithdrawAshCoin.Hit += CmdWithdrawAshCoin_Hit;
      cmdWithdrawMaxAshCoin.Hit += CmdWithdrawMaxAshCoin_Hit;
      cmdWithdrawPyrealSavings.Hit += CmdWithdrawPyrealSavings_Hit;
      cmdWithdrawMaxPyrealSavings.Hit += CmdWithdrawMaxPyrealSavings_Hit;

      chkDepositPyrealsPeriodically.Change += ChkDepositPyrealsPeriodically_Change;
      chkDepositLuminancePeriodically.Change += ChkDepositLuminancePeriodically_Change;

      cmdAddFriend.Hit += CmdAddFriend_Hit;
      lstCharacters.Click += LstCharacters_Click;
      lstFriends.Click += LstFriends_Click;

      cmdVerifyTransfer.Hit += CmdVerifyTransfer_Hit;
      cmdTransfer.Hit += CmdTransfer_Hit;

      cmdSaveSettings.Hit += CmdSaveSettings_Hit;

      Logger.Core.ChatBoxMessage += Core_ChatBoxMessage;

      ClearTransferStatus();

      _timer = new Timer()
      {
        Interval = 1000 * 5//1 min
      };
      _timer.Tick += _timer_Tick;
      _timer.Start();
    }

    #endregion

    #region ... Destroy ...

    ~MainView()
    {
      _timer.Stop();
      _timer.Tick -= _timer_Tick;
      _timer.Dispose();

      Logger.Core.ChatBoxMessage -= Core_ChatBoxMessage;

      cmdRequestBalances.Hit -= CmdRequestBalances_Hit;
      cmdDepositPyreals.Hit -= CmdDepositPyreals_Hit;
      cmdDepositMaxPyreals.Hit -= CmdDepositMaxPyreals_Hit;
      cmdDepositLuminance.Hit -= CmdDepositLuminance_Hit;
      cmdDepositMaxLuminance.Hit -= CmdDepositMaxLuminance_Hit;
      cmdDepositAshCoin.Hit -= CmdDepositAshCoin_Hit;
      cmdDepositMaxAshCoin.Hit -= CmdDepositMaxAshCoin_Hit;
      cmdDepositPyrealSavings.Hit -= CmdDepositPyrealSavings_Hit;
      cmdDepositMaxPyrealSavings.Hit -= CmdDepositMaxPyrealSavings_Hit;

      cmdWithdrawPyreals.Hit -= CmdWithdrawPyreals_Hit;
      cmdWithdrawMaxPyreals.Hit -= CmdWithdrawMaxPyreals_Hit;
      cmdWithdrawLuminance.Hit -= CmdWithdrawLuminance_Hit;
      cmdWithdrawMaxLuminance.Hit -= CmdWithdrawMaxLuminance_Hit;
      cmdWithdrawAshCoin.Hit -= CmdWithdrawAshCoin_Hit;
      cmdWithdrawMaxAshCoin.Hit -= CmdWithdrawMaxAshCoin_Hit;
      cmdWithdrawPyrealSavings.Hit -= CmdWithdrawPyrealSavings_Hit;
      cmdWithdrawMaxPyrealSavings.Hit -= CmdWithdrawMaxPyrealSavings_Hit;

      chkDepositPyrealsPeriodically.Change -= ChkDepositPyrealsPeriodically_Change;
      chkDepositLuminancePeriodically.Change -= ChkDepositLuminancePeriodically_Change;

      cmdAddFriend.Hit -= CmdAddFriend_Hit;
      lstCharacters.Click -= LstCharacters_Click;
      lstFriends.Click -= LstFriends_Click;
      cmdVerifyTransfer.Hit -= CmdVerifyTransfer_Hit;
      cmdTransfer.Hit -= CmdTransfer_Hit;

      cmdSaveSettings.Hit -= CmdSaveSettings_Hit;

      lblAccountNumber = null;
      lblPyreals = null;
      lblPyrealSavings = null;
      lblLuminance = null;
      lblAshCoins = null;
      cmdRequestBalances = null;
      txtDepositPyreals = null;
      txtDepositLuminance = null;
      txtDepositAshCoin = null;
      txtDepositPyrealSavings = null;
      cmdDepositPyreals = null;
      cmdDepositMaxPyreals = null;
      cmdDepositLuminance = null;
      cmdDepositMaxLuminance = null;
      cmdDepositAshCoin = null;
      cmdDepositMaxAshCoin = null;
      cmdDepositPyrealSavings = null;
      cmdDepositMaxPyrealSavings = null;
      chkDepositPyrealsPeriodically = null;
      chkDepositLuminancePeriodically = null;

      txtWithdrawPyreals = null;
      txtWithdrawLuminance = null;
      txtWithdrawAshCoin = null;
      txtWithdrawPyrealSavings = null;
      cmdWithdrawPyreals = null;
      cmdWithdrawMaxPyreals = null;
      cmdWithdrawLuminance = null;
      cmdWithdrawMaxLuminance = null;
      cmdWithdrawAshCoin = null;
      cmdWithdrawMaxAshCoin = null;
      cmdWithdrawPyrealSavings = null;
      cmdWithdrawMaxPyrealSavings = null;

      lstCharacters = null;
      lstFriends = null;

      txtFriendAccountNumber = null;
      txtFriend = null;
      cmdAddFriend = null;
      cmdVerifyTransfer = null;
      lblTransferStatus = null;
      lblTransferStatusLine2 = null;

      _instance = null;
      _settings = null;
    }

    #endregion

    #region ... StopTimer ...

    public void StopTimer()
    {
      _timer.Stop();
    }

    #endregion

    #region ... TryDepositPyreals ...

    public bool TryDepositPyreals(int amount)
    {
      try
      {
        EatBankingText = true;
        Logger.WriteToChat($"{BankCommands.DepositPyrealsCommand} {amount}");
        return true;
      }
      catch(Exception ex) { 
        Logger.WriteDebugger(ex);
        return false;
      }
    }

    #endregion

    #region ... TryWithdrawPyreals ...

    public bool TryWithdrawPyreals(int amount)
    {
      try
      {
        EatBankingText = true;
        Logger.WriteToChat($"{BankCommands.WithdrawPyrealsCommand} {amount}");
        return true;
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
        return false;
      }
    }

    #endregion

    #region ... TryDepositPyrealsInSavings ...

    public bool TryDepositPyrealsInSavings(int amount)
    {
      try
      {
        EatBankingText= true;
        Logger.WriteToChat($"{BankCommands.DepositPyrealsToSavingsCommand} {amount}");
        return true;
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
        return false;
      }
    }

    #endregion

    #region ... TryWithdrawPyrealsInSavings ...

    public bool TryWithdrawPyrealsInSavings(int amount)
    {
      try
      {
        EatBankingText = true;
        Logger.WriteToChat($"{BankCommands.WithdrawPyrealSavingsCommand} {amount}");
        return true;
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
        return false;
      }
    }

    #endregion

    #region ... TryDepositAshCoin ...

    public bool TryDepositAshCoin(int amount)
    {
      try
      {
        EatBankingText = true;
        Logger.WriteToChat($"{BankCommands.DepositAshCoinCommand} {amount}");
        return true;
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
        return false;
      }
    }

    #endregion

    #region ... TryWithdrawAshCoin ...

    public bool TryWithdrawAshCoin(int amount)
    {
      try
      {
        EatBankingText = true;
        Logger.WriteToChat($"{BankCommands.WithdrawAshCoinCommand} {amount}");
        return true;
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
        return false;
      }
    }

    #endregion

    #region ... TryDepositLuminance ...

    public bool TryDepositLuminance(int amount)
    {
      try
      {
        EatBankingText = true;
        Logger.WriteToChat($"{BankCommands.DepositLuminanceCommand} {amount}");
        return true;
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
        return false;
      }
    }

    #endregion

    #region ... TryWithdrawLuminance ...

    public bool TryWithdrawLuminance(int amount)
    {
      try
      {
        if (_instance.LuminanceMaximum == 0)
        {
          Logger.WriteMessage($"{amount} was requested, but character MaxLuminance was zero. Luminance not being set for character?");
          return false;
        }
        //always ensure we never go over our max luminance amount
        int difference = (int)(_instance.LuminanceMaximum - _instance.LuminanceCurrent);

        if(amount > difference)
          amount=difference; 

        EatBankingText = true;
        Logger.WriteToChat($"{BankCommands.WithdrawLuminanceCommand} {amount}");
        return true;
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
        return false;
      }
    }

    #endregion

    #region ... CheckPyrealBalanceInInventory ...

    private bool CheckPyrealBalanceInInventory(int amount, out int total)
    {
      total = 0;
      var items = Logger.Core.WorldFilter.GetByObjectClass(ObjectClass.Money).ToList();
      total = items.Where(w => w.Name.Equals("Pyreal")).Select(s => s.Values(LongValueKey.Value)).Sum();
      return amount <= total;
    }

    #endregion

    #region ... GetMaxPyrealsInInventory ...

    private int GetMaxPyrealsInInventory()
    {
      var items = Logger.Core.WorldFilter.GetByObjectClass(ObjectClass.Money).ToList();
      return items.Where(w => w.Name.Equals("Pyreal")).Select(s => s.Values(LongValueKey.Value)).Sum();
    }

    #endregion

    #region ... CheckAshCoinBalanceInInventory ...

    private bool CheckAshCoinBalanceInInventory(int amount, out int total)
    {
      total = 0;
      var items = Logger.Core.WorldFilter.GetByObjectClass(ObjectClass.Misc).ToList();
      total = items.Where(w => w.Name.Equals("AshCoin")).Select(s => s.Values(LongValueKey.StackCount)).Sum();
      return amount <= total;
    }

    #endregion

    #region ... GetMaxAshCoinsInInventory ...

    private int GetMaxAshCoinsInInventory()
    {
      var items = Logger.Core.WorldFilter.GetByObjectClass(ObjectClass.Misc).ToList();
     return items.Where(w => w.Name.Equals("AshCoin")).Select(s => s.Values(LongValueKey.StackCount)).Sum();
    }

    #endregion

    #region ... CheckCurrentLuminanceValueOnCharacter ...

    private bool CheckCurrentLuminanceValueOnCharacter(int amount, out long total)
    {
      total = _instance.LuminanceCurrent;
      return amount <= total;
    }

    #endregion

    #region ... PopulateAccountNumber ...

    public void PopulateAccountDetails()
    {
      try
      {
        if (string.IsNullOrEmpty(_settings.AccountNumber))
        {
          lblAccountNumber.Text = "Not Available";
          lblPyreals.Text = "0";
          lblPyrealSavings.Text = "0";
          lblLuminance.Text = "0";
          lblAshCoins.Text = "0";
          RequestAccountNumber();
          return;
        }
        lblAccountNumber.Text = _settings.AccountNumber;
        chkDepositPyrealsPeriodically.Checked = _settings.DepositPyrealsPeriodically;
        chkDepositLuminancePeriodically.Checked = _settings.DepositLuminancePeriodically;
        PopulateBalances();
      }
      catch(Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... PopulateBalances ...

    public void PopulateBalances()
    {
      try
      {
        PopulateBalancePyreals();
        PopulateBalancePyrealSavings();
        PopulateBalanceLuminance();
        PopulateBalanceAshCoin();
      }
      catch(Exception ex)
      {
        Logger.WriteDebugger(ex);
        
      }
    }

    public void PopulateBalancePyreals()
    {
      lblPyreals.Text = _settings.Pyreals.ToString("N0");
    }

    public void PopulateBalanceLuminance()
    {
      lblLuminance.Text = _settings.Luminance.ToString("N0");
    }

    public void PopulateBalanceAshCoin()
    {
      lblAshCoins.Text = _settings.AshCoin.ToString("N0");
    }

    public void PopulateBalancePyrealSavings()
    {
      lblPyrealSavings.Text = _settings.PyrealSavings.ToString("N0");
    }

    #endregion

    #region ... RequestAccountNumber ...

    public void RequestAccountNumber()
    {
      try
      {
        Logger.WriteToChat(BankCommands.BankAccount);
      }
      catch(Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... Core_ChatBoxMessage ...

    private void Core_ChatBoxMessage(object sender, Decal.Adapter.ChatTextInterceptEventArgs e)
    {
      if (!_instance.IsEnabled)
        return;

      if(e.Text.StartsWith(BankCommands.IncomingTransferFirstPart) && e.Text.Contains(BankCommands.IncomingTransferLastPart))
      {
        HandleIncomingTransfer(e.Text);
        return;
      }
      if (e.Text.StartsWith(BankCommands.BankAccountCallBack))
      {
        _settings.AccountNumber = BankCommands.GetAccountNumberFromBankAccountResponse(e.Text);
        Settings.Save(Logger.BasePath, _settings);
        PopulateAccountDetails();
        //e.Eat = EatBankingText;
        return;
      }
      if (e.Text.StartsWith(BankCommands.BankAccountBalancesCallBack) || e.Text.StartsWith(BankCommands.BankNewAccountBalancesCallBack))
      {
        var balance = BankCommands.GetAccountBalancesFromBankAccountBalanceResponse(e.Text);
        _settings.Pyreals = balance.Pyreals;
        _settings.Luminance = balance.Luminance;
        _settings.AshCoin = balance.AshCoin;
        _settings.PyrealSavings = balance.PyrealSavings;
        Settings.Save(Logger.BasePath, _settings);
        PopulateBalances();
        //e.Eat = EatBankingText;
        EatBankingText = false;
        return;
      }
      if ((e.Text.StartsWith(BankCommands.DepositAttemptNewBalanceCallBack) || e.Text.StartsWith(BankCommands.TransferNewBalanceCallBack)) && e.Text.Contains("Pyreals"))
      {
        var balance = BankCommands.GetAccountBalanceFromDepositOfPyreals(e.Text);
        _settings.Pyreals = balance.Pyreals;
        Settings.Save(Logger.BasePath, _settings);
        PopulateBalancePyreals();
        //e.Eat = EatBankingText;
        EatBankingText = false;
        return;
      }
      if ((e.Text.StartsWith(BankCommands.DepositAttemptNewBalanceCallBack) || e.Text.StartsWith(BankCommands.TransferNewBalanceCallBack)) && e.Text.Contains("Pyreals in savings"))
      {
        var balance = BankCommands.GetAccountBalanceFromDepositOfPyrealsIntoSavings(e.Text);
        _settings.PyrealSavings = balance.PyrealSavings;
        Settings.Save(Logger.BasePath, _settings);
        PopulateBalancePyrealSavings();
        //e.Eat = EatBankingText;
        EatBankingText = false;
        return;
      }
      if ((e.Text.StartsWith(BankCommands.DepositAttemptNewBalanceCallBack) || e.Text.StartsWith(BankCommands.TransferNewBalanceCallBack))&& e.Text.Contains("AshCoin"))
      {
        var balance = BankCommands.GetAccountBalanceFromDepositOfAshCoin(e.Text);
        _settings.AshCoin = balance.AshCoin;
        Settings.Save(Logger.BasePath, _settings);
        PopulateBalanceAshCoin();
        //e.Eat = EatBankingText;
        EatBankingText = false;
        return;
      }
      if ((e.Text.StartsWith(BankCommands.DepositAttemptNewBalanceCallBack) || e.Text.StartsWith(BankCommands.TransferNewBalanceCallBack)) && e.Text.Contains("Luminance"))
      {
        var balance = BankCommands.GetAccountBalanceFromDepositOfLuminance(e.Text);
        _settings.Luminance = balance.Luminance;
        Settings.Save(Logger.BasePath, _settings);
        PopulateBalanceLuminance();
        //e.Eat = EatBankingText;
        EatBankingText = false;
        return;
      }
    }

    #endregion

    #region ... ChkDepositLuminancePeriodically_Change ... 

    private void ChkDepositLuminancePeriodically_Change(object sender, EventArgs e)
    {
      _settings.DepositLuminancePeriodically = chkDepositLuminancePeriodically.Checked;
      Settings.Save(Logger.BasePath, _settings);
    }

    #endregion

    #region ... ChkDepositPyrealsPeriodically_Change ... 

    private void ChkDepositPyrealsPeriodically_Change(object sender, EventArgs e)
    {
      _settings.DepositPyrealsPeriodically = chkDepositPyrealsPeriodically.Checked;
      Settings.Save(Logger.BasePath, _settings);
    }

    #endregion

    #region ... CmdDepositMaxPyrealSavings_Hit ... 

    private void CmdDepositMaxPyrealSavings_Hit(object sender, EventArgs e)
    {
      var amount = GetMaxPyrealsInInventory();
      if (amount == 0 || !TryDepositPyrealsInSavings(amount))
      {
        Logger.WriteMessage("Unable to deposit requested amount. Transaction has been cancelled.");
        return;
      }
    }

    #endregion

    #region ... CmdDepositPyrealSavings_Hit ...

    private void CmdDepositPyrealSavings_Hit(object sender, EventArgs e)
    {
      try
      {
        int amount;
        if (AmountUtils.TryParseAmount(txtDepositPyrealSavings.Text, out amount))
        {
          if (amount > 0)
          {
            int total;
            if (!CheckPyrealBalanceInInventory(amount, out total))
            {
              Logger.WriteMessage($"Unable to deposit requested amount. You tried to deposit {amount.ToString("N0")} but you only have {total.ToString("N0")} available. Transaction has been cancelled.");
              return;
            }
            if (!TryDepositPyrealsInSavings(amount))
            {
              Logger.WriteMessage("Unable to deposit requested amount. Transaction has been cancelled.");
              return;
            }
          }
          return;
        }
        Logger.WriteDebugger($"Failed to convert pyreal value to int. Value: {txtDepositPyrealSavings.Text}");
        Logger.WriteMessage("Unable to deposit requested amount. Transaction has been cancelled.");
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... CmdDepositMaxAshCoin_Hit ...

    private void CmdDepositMaxAshCoin_Hit(object sender, EventArgs e)
    {
      var amount = GetMaxAshCoinsInInventory();
      if (amount == 0 || !TryDepositAshCoin(amount))
      {
        Logger.WriteMessage("Unable to deposit requested amount. Transaction has been cancelled.");
        return;
      }
    }

    #endregion

    #region ... CmdDepositAshCoin_Hit ...

    private void CmdDepositAshCoin_Hit(object sender, EventArgs e)
    {
      try
      {
        int amount;
        if (AmountUtils.TryParseAmount(txtDepositAshCoin.Text, out amount))
        {
          if (amount > 0)
          {
            int total;
            if (!CheckAshCoinBalanceInInventory(amount, out total))
            {
              Logger.WriteMessage($"Unable to deposit requested amount. You tried to deposit {amount.ToString("N0")} but you only have {total.ToString("N0")} available. Transaction has been cancelled.");
              return;
            }
            if (!TryDepositAshCoin(amount))
            {
              Logger.WriteMessage("Unable to deposit requested amount. Transaction has been cancelled.");
              return;
            }
          }
          return;
        }
        Logger.WriteDebugger($"Failed to convert AshCoin value to int. Value: {txtDepositAshCoin.Text}");
        Logger.WriteMessage("Unable to deposit requested amount. Transaction has been cancelled.");
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... CmdDepositMaxLuminance_Hit ...

    private void CmdDepositMaxLuminance_Hit(object sender, EventArgs e)
    {
      if (_instance.LuminanceCurrent == 0 || !TryDepositLuminance((int)_instance.LuminanceCurrent))
      {
        Logger.WriteMessage("Unable to deposit requested amount. Transaction has been cancelled.");
        return;
      }
    }

    #endregion

    #region ... CmdDepositLuminance_Hit ...

    private void CmdDepositLuminance_Hit(object sender, EventArgs e)
    {
      try
      {
        int amount;
        if (AmountUtils.TryParseAmount(txtDepositLuminance.Text, out amount))
        {
          if (amount > 0)
          {
            long total;
            if (!CheckCurrentLuminanceValueOnCharacter(amount, out total))
            {
              Logger.WriteMessage($"Unable to deposit requested amount. You tried to deposit {amount.ToString("N0")} but you only have {total.ToString("N0")} available. Transaction has been cancelled.");
              return;
            }
            if (!TryDepositLuminance(amount))
            {
              Logger.WriteMessage("Unable to deposit requested amount. Transaction has been cancelled.");
              return;
            }
          }
          return;
        }
        Logger.WriteDebugger($"Failed to convert current luminance value to int. Value: {txtDepositLuminance.Text}");
        Logger.WriteMessage("Unable to deposit requested amount. Transaction has been cancelled.");
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... CmdDepositMaxPyreals_Hit ...

    private void CmdDepositMaxPyreals_Hit(object sender, EventArgs e)
    {
      var amount = GetMaxPyrealsInInventory();
      if (amount == 0 || !TryDepositPyreals(amount))
      {
        Logger.WriteMessage("Unable to deposit requested amount. Transaction has been cancelled.");
        return;
      }
    }

    #endregion

    #region ... CmdDepositPyreals_Hit ...

    private void CmdDepositPyreals_Hit(object sender, EventArgs e)
    {
      try
      {
        int amount;
        if (AmountUtils.TryParseAmount(txtDepositPyreals.Text, out amount))
        {
          if (amount > 0)
          {
            int total;
            if (!CheckPyrealBalanceInInventory(amount, out total))
            {
              Logger.WriteMessage($"Unable to deposit requested amount. You tried to deposit {amount.ToString("N0")} but you only have {total.ToString("N0")} available. Transaction has been cancelled.");
              return;
            }
            if (!TryDepositPyreals(amount))
            {
              Logger.WriteMessage("Unable to deposit requested amount. Transaction has been cancelled.");
              return;
            }
          }
          return;
        }
        Logger.WriteDebugger($"Failed to convert pyreal value to int. Value: {txtDepositPyreals.Text}");
        Logger.WriteMessage("Unable to deposit requested amount. Transaction has been cancelled.");
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... CmdRequestBalances_Hit ...

    private void CmdRequestBalances_Hit(object sender, EventArgs e)
    {
      try
      {
        EatBankingText = true;
        Logger.WriteToChat(BankCommands.BankAccount);
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... CmdWithdrawMaxPyrealSavings_Hit ... 

    private void CmdWithdrawMaxPyrealSavings_Hit(object sender, EventArgs e)
    {
      if (_settings.PyrealSavings == 0 || !TryWithdrawPyreals(_settings.PyrealSavings))
      {
        Logger.WriteMessage("Unable to withdraw requested amount. Transaction has been cancelled.");
        return;
      }
    }

    #endregion

    #region ... CmdWithdrawPyrealSavings_Hit ...

    private void CmdWithdrawPyrealSavings_Hit(object sender, EventArgs e)
    {
      try
      {
        int amount;
        if (AmountUtils.TryParseAmount(txtWithdrawPyrealSavings.Text, out amount))
        {
          if (amount > 0)
          {
            if (amount > _settings.PyrealSavings)
            {
              Logger.WriteMessage($"Unable to withdraw requested amount. You tried to withdraw {amount.ToString("N0")} but you only have {_settings.PyrealSavings.ToString("N0")} available. Transaction has been cancelled.");
              return;
            }
            if (!TryWithdrawPyrealsInSavings(amount))
            {
              Logger.WriteMessage("Unable to withdraw requested amount. Transaction has been cancelled.");
              return;
            }
          }
          return;
        }
        Logger.WriteDebugger($"Failed to convert pyreal value to int. Value: {txtWithdrawPyrealSavings.Text}");
        Logger.WriteMessage("Unable to withdraw requested amount. Transaction has been cancelled.");
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... CmdWithdrawMaxAshCoin_Hit ...

    private void CmdWithdrawMaxAshCoin_Hit(object sender, EventArgs e)
    {
      if (_settings.AshCoin == 0 || !TryWithdrawAshCoin(_settings.AshCoin))
      {
        Logger.WriteMessage("Unable to withdraw requested amount. Transaction has been cancelled.");
        return;
      }
    }

    #endregion

    #region ... CmdWithdrawAshCoin_Hit ...

    private void CmdWithdrawAshCoin_Hit(object sender, EventArgs e)
    {
      try
      {
        int amount;
        if (AmountUtils.TryParseAmount(txtWithdrawAshCoin.Text, out amount))
        {
          if (amount > 0)
          {
            if (amount > _settings.AshCoin)
            {
              Logger.WriteMessage($"Unable to withdraw requested amount. You tried to withdraw {amount.ToString("N0")} but you only have {_settings.AshCoin.ToString("N0")} available. Transaction has been cancelled.");
              return;
            }
            if (!TryWithdrawAshCoin(amount))
            {
              Logger.WriteMessage("Unable to withdraw requested amount. Transaction has been cancelled.");
              return;
            }
          }
          return;
        }
        Logger.WriteDebugger($"Failed to convert AshCoin value to int. Value: {txtWithdrawAshCoin.Text}");
        Logger.WriteMessage("Unable to withdraw requested amount. Transaction has been cancelled.");
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... CmdWithdrawMaxLuminance_Hit ...

    private void CmdWithdrawMaxLuminance_Hit(object sender, EventArgs e)
    {
      if (_settings.Luminance == 0 || !TryWithdrawLuminance((int)_settings.Luminance))
      {
        Logger.WriteMessage("Unable to withdraw requested amount. Transaction has been cancelled.");
        return;
      }
    }

    #endregion

    #region ... CmdWithdrawLuminance_Hit ...

    private void CmdWithdrawLuminance_Hit(object sender, EventArgs e)
    {
      try
      {
        int amount;
        if (AmountUtils.TryParseAmount(txtWithdrawLuminance.Text, out amount))
        {
          if (amount > 0)
          {
            if (amount > (int)_settings.Luminance)
            {
              Logger.WriteMessage($"Unable to withdraw requested amount. You tried to withdraw {amount.ToString("N0")} but you only have {_settings.Luminance.ToString("N0")} available. Transaction has been cancelled.");
              return;
            }
            if (!TryWithdrawLuminance(amount))
            {
              Logger.WriteMessage("Unable to withdraw requested amount. Transaction has been cancelled.");
              return;
            }
          }
          return;
        }
        Logger.WriteDebugger($"Failed to convert current luminance value to int. Value: {txtWithdrawLuminance.Text}");
        Logger.WriteMessage("Unable to withdraw requested amount. Transaction has been cancelled.");
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... CmdWithdrawMaxPyreals_Hit ...

    private void CmdWithdrawMaxPyreals_Hit(object sender, EventArgs e)
    {
      if (_settings.Pyreals == 0 || !TryWithdrawPyreals(_settings.Pyreals))
      {
        Logger.WriteMessage("Unable to withdraw requested amount. Transaction has been cancelled.");
        return;
      }
    }

    #endregion

    #region ... CmdWithdrawPyreals_Hit ...

    private void CmdWithdrawPyreals_Hit(object sender, EventArgs e)
    {
      try
      {
        int amount;
        if (AmountUtils.TryParseAmount(txtWithdrawPyreals.Text, out amount))
        {
          if (amount > 0)
          {
            if (amount > _settings.Pyreals)
            {
              Logger.WriteMessage($"Unable to wthdraw requested amount. You tried to withdraw {amount.ToString("N0")} but you only have {_settings.Pyreals.ToString("N0")} available in your bank. Transaction has been cancelled.");
              return;
            }
            if (!TryWithdrawPyreals(amount))
            {
              Logger.WriteMessage("Unable to withdraw requested amount. Transaction has been cancelled.");
              return;
            }
          }
          return;
        }
        Logger.WriteDebugger($"Failed to convert pyreal value to int. Value: {txtWithdrawPyreals.Text}");
        Logger.WriteMessage("Unable to withdraw requested amount. Transaction has been cancelled.");
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... PopulateCharactersList ...

    public void PopulateCharactersList()
    {
      try
      {
        lstCharacters.ClearRows();
        foreach (var item in _instance.Characters)
        {
          HudList.HudListRowAccessor row = lstCharacters.AddRow();
          row[ListCharacters.Name] = new HudStaticText() { Text = item.Key };
          row[ListCharacters.AccountNumber] = new HudStaticText() { Text = item.Value, Visible = false };
        }
      }
      catch(Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... PopulateFriendsList ...

    public void PopulateFriendsList()
    {
      try
      {
        lstFriends.ClearRows();
        foreach (var item in _settings.FriendsList)
        {
          HudList.HudListRowAccessor row = lstFriends.AddRow();
          row[ListFriends.Name] = new HudStaticText() { Text = item.Key };
          row[ListFriends.AccountNumber] = new HudStaticText() { Text = item.Value, Visible = false };
          row[ListFriends.Remove] = new HudPictureBox() { Image = new ACImage(ListFriends.RemoveIcon) };
        }
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... CmdAddFriend_Hit ...

    private void CmdAddFriend_Hit(object sender, EventArgs e)
    {

      try
      {
        if (!string.IsNullOrEmpty(txtFriend.Text) && !string.IsNullOrEmpty(txtFriendAccountNumber.Text) && !_settings.FriendsList.ContainsKey(txtFriend.Text))
        {
          _settings.FriendsList.Add(txtFriend.Text, txtFriendAccountNumber.Text);
          Settings.Save(_instance.Path, _settings);
          PopulateFriendsList();
        }
      }
      catch(Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... LstFriends_Click ...
    private void LstFriends_Click(object sender, int row, int col)
    {
      try
      {
        if(col == ListFriends.Remove)
        {
          var name = ((HudStaticText)lstFriends[row][ListFriends.Name]).Text;
          if(_settings.FriendsList.ContainsKey(name))
          {
            _settings.FriendsList.Remove(name);
            Settings.Save(Logger.BasePath, _settings);
            PopulateFriendsList();
            return;
          }
        }
        _transfer.AccountNumber = ((HudStaticText)lstFriends[row][ListFriends.AccountNumber]).Text;
        _transfer.Name = ((HudStaticText)lstFriends[row][ListFriends.Name]).Text;
        lblTransferToPlayer.Text = $"{_transfer.Name} (Acc #: {_transfer.AccountNumber})";
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... LstCharacters_Click ...

    private void LstCharacters_Click(object sender, int row, int col)
    {
      try
      {
        _transfer.AccountNumber = ((HudStaticText)lstCharacters[row][ListCharacters.AccountNumber]).Text;
        _transfer.Name = ((HudStaticText)lstCharacters[row][ListCharacters.Name]).Text;
        lblTransferToPlayer.Text = $"{_transfer.Name} (Acc #: {_transfer.AccountNumber})";
      }
      catch(Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... CmdVerifyTransfer_Hit ...

    private void CmdVerifyTransfer_Hit(object sender, EventArgs e)
    {
      try
      {
        SetTransferDetails();
        lblTransferStatus.Text = $"Transferring {_transfer.Amount} {_transfer.Currency.ToString().ToLower()} to ";
        lblTransferStatusLine2.Text = $"{_transfer.Name} with Acc# {_transfer.AccountNumber}.";
      }
      catch(Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... SetTransferDetails ...

    private void SetTransferDetails()
    {
      int amount;
      if (!Int32.TryParse(txtTransferAmount.Text.Replace(",", "").Replace(" ", "").Trim(), out amount))
        amount = 0;
      _transfer.Amount = amount;
      _transfer.Currency = (CurrencyOptions)cboTransferCurrency.Current;
    }

    #endregion

    #region ... CmdTransfer_Hit ...

    private void CmdTransfer_Hit(object sender, EventArgs e)
    {
      try
      {
        if(ValidateTransfer())
        {
          var currency = string.Empty;
          switch(_transfer.Currency)
          {
            case CurrencyOptions.Pyreal:
              currency = "pyreals";
              break;
            case CurrencyOptions.Luminance:
              currency = "luminance";
              break;
            case CurrencyOptions.Ashcoin:
              currency = "ashcoin";
              break;
          }
          
          lblTransferStatus.Text = $"Transferring {_transfer.Amount} {_transfer.Currency.ToString().ToLower()} to";
          lblTransferStatusLine2.Text = $"{_transfer.Name} with Acc# {_transfer.AccountNumber}.";
          Logger.WriteToChat($"{BankCommands.TransferCommand.Replace("<<ACCOUNTNUMBER>>", _transfer.AccountNumber).Replace("<<CURRENCY>>", currency)} {_transfer.Amount}");
          //EatBankingText = true;
        }
      }
      catch(      Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... ValidateTransfer ...

    private bool ValidateTransfer()
    {
      ClearTransferStatus();
      if (string.IsNullOrEmpty(_transfer.AccountNumber))
      {
        lblTransferStatus.Text = "Please select a player to transfer too.";
        return false;
      }
      SetTransferDetails();
      int pool = 0;
      switch(_transfer.Currency)
      {
        case CurrencyOptions.Pyreal:
          pool = _settings.Pyreals;
          break;
        case CurrencyOptions.Luminance:
          pool = (int) _settings.Luminance;
          break;
        case CurrencyOptions.Ashcoin:
          pool = _settings.AshCoin;
          break;
      }
      if(_transfer.Amount > pool)
      {
        lblTransferStatus.Text = $"You are trying to transfer {_transfer.Amount.ToString("N0")} {_transfer.Currency.ToString().ToLower()}";
        lblTransferStatusLine2.Text = $"but you only have {pool.ToString("N0")} available.";

        return false;
      }
      return true;
    }

    #endregion

    #region ... HandleIncomingTransfer ...

    private void HandleIncomingTransfer(string response)
    {
      var origResponse = response;
      response = response.Substring(response.IndexOf(BankCommands.IncomingTransferLastPart)).Replace(BankCommands.IncomingTransferLastPart, "").Trim();
      var amountStr = response.Substring(0, response.IndexOf(" ")).Trim();
      response = response.Replace(amountStr, "").Trim().ToLower();
      int amount;
      if (!Int32.TryParse(amountStr, out amount))
      {
        Logger.WriteDebugger($"HandleIncomingTransfer::Text: '{origResponse}'. Unable to parse amount.");
        return;
      }
      if (response.Equals("pyreals"))
      {
        _settings.Pyreals += amount;
      }
      else if (response.Equals("ashcoin"))
      {
        _settings.AshCoin += amount;

      }
      else if (response.Equals("luminance"))
      {
        _settings.Luminance += amount;
      }
      else
      {
        Logger.WriteDebugger($"HandleIncomingTransfer::Text: '{origResponse}'. Unable to parse currency type.");
        return;
      }
      Settings.Save(Logger.BasePath, _settings);
    }

    #endregion

    #region ... ClearTransferStatus ...

    public void ClearTransferStatus()
    {
      lblTransferStatus.Text = string.Empty;
      lblTransferStatusLine2.Text = string.Empty;
    }

    #endregion

    #region ... CmdSaveSettings_Hit ...

    private void CmdSaveSettings_Hit(object sender, EventArgs e)
    {
      //pyreals
      int pInterval;
      if(!Int32.TryParse(txtPyrealInterval.Text, out pInterval))
      {
        pInterval = 30;
      }
      int pAmount;
      if(!AmountUtils.TryParseAmount(txtPyrealDepositAmount.Text,out pAmount))
      {
        pAmount = 0;
      }
      Settings.PyrealsDepositTypes pDeposits = (Settings.PyrealsDepositTypes)cboPyrealDepositType.Current;

      //luminance
      int lInterval;
      if (!Int32.TryParse(txtLuminanceInterval.Text, out lInterval))
      {
        lInterval = 30;
      }
      int lAmount;
      if (!AmountUtils.TryParseAmount(txtLuminanceDepositAmount.Text, out lAmount))
      {
        lAmount = 0;
      }
      Settings.LuminanceDepositTypes lDeposits = (Settings.LuminanceDepositTypes)cboLuminanceDepositType.Current;

      if(_settings.PyrealsSaveInterval != pInterval)
        _settings.PyrealsSaveInterval = pInterval;

      if (_settings.PyrealsDepositAmount != pAmount)
      {
        _settings.PyrealsDepositAmount = pAmount;
        txtPyrealDepositAmount.Text = pAmount.ToString();
      }

      if(_settings.PyrealDepositType != pDeposits)
        _settings.PyrealDepositType= pDeposits;


      if (_settings.LuminanceSaveInterval != lInterval)
        _settings.LuminanceSaveInterval = lInterval;

      if (_settings.LuminanceDepositAmount != lAmount)
      {
        _settings.LuminanceDepositAmount = lAmount;
        txtLuminanceDepositAmount.Text = lAmount.ToString();
      }

      if (_settings.LuminanceDepositType != lDeposits)
        _settings.LuminanceDepositType = lDeposits;

      Settings.Save(Logger.BasePath, _settings);
    }

    #endregion

    #region ... PopulateIntervalSettings ...

    public void PopulateIntervalSettings()
    {
      txtPyrealInterval.Text = _settings.PyrealsSaveInterval.ToString();
      cboPyrealDepositType.Current = (int)_settings.PyrealDepositType;
      txtPyrealDepositAmount.Text = _settings.PyrealsDepositAmount.ToString();

      txtLuminanceInterval.Text = _settings.LuminanceSaveInterval.ToString();
      cboLuminanceDepositType.Current = (int)_settings.LuminanceDepositType;
      txtLuminanceDepositAmount.Text = _settings.LuminanceDepositAmount.ToString();
    }

    #endregion

    #region ... _timer_Tick ...

    private void _timer_Tick(object sender, EventArgs e)
    {
      try
      {
        _timer.Stop();
        if (_settings.DepositPyrealsPeriodically && DateTime.Now.Subtract(PyrealDepositTimeStamp).TotalMinutes >= _settings.PyrealsSaveInterval)
        {
          int maxP = GetMaxPyrealsInInventory();
          int depositP = 0;
          switch (_settings.PyrealDepositType)
          {
            case Settings.PyrealsDepositTypes.Specified:
              if(_settings.PyrealsDepositAmount > 0 && maxP > 0)
              {
                if (maxP > _settings.PyrealsDepositAmount)
                  depositP = _settings.PyrealsDepositAmount;
                else 
                  depositP = maxP;
              }
              break;
            case Settings.PyrealsDepositTypes.Difference:
              depositP = maxP % BankCommands.MMDValue;
              break;
            case Settings.PyrealsDepositTypes.Maximum:
              depositP = maxP;
              break;
          }
          PyrealDepositTimeStamp = DateTime.Now;
          if (depositP == 0)
            return;
          TryDepositPyreals(depositP);
        }
        if (_settings.DepositLuminancePeriodically && DateTime.Now.Subtract(LuminanceDepositTimeStamp).TotalMinutes >= _settings.LuminanceSaveInterval)
        {
          int maxL = (int)_instance.LuminanceCurrent;
          int depositLuminanceP = 0;
          if (_settings.LuminanceDepositType == Settings.LuminanceDepositTypes.Specified)
          {
            if (_settings.LuminanceDepositAmount > 0 && maxL > 0)
            {
              if (maxL > _settings.LuminanceDepositAmount)
                depositLuminanceP = _settings.LuminanceDepositAmount;
              else
                depositLuminanceP = maxL;
            }
          }
          else
            depositLuminanceP = maxL;

          LuminanceDepositTimeStamp = DateTime.Now;
          if (depositLuminanceP == 0)
            return;
          TryDepositLuminance(depositLuminanceP);
        }
      }
      catch(Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
      finally
      {
        _timer.Start();
      }
    }

    #endregion

  }

  public struct ListCharacters
  {
    public const int Name = 0;
    public const int AccountNumber = 1;
  }

  public struct ListFriends
  {
    public const int Name = 0;
    public const int AccountNumber = 1;
    public const int Remove = 2;
    public const int RemoveIcon = 4600;
  }
}
