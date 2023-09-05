using Decal.Adapter;
using System;
using System.Collections.Generic;
using System.IO;
using VirindiViewService;
using VirindiViewService.XMLParsers;

namespace ValHeelBanker
{
  [FriendlyName("VH Banker")]
  public class PluginCore : PluginBase
  {

    #region ... Private Properties ...

    private const string ValHeelServerName = "ValHeel";
    public static PluginCore Instance { get; private set; }
    public bool IsEnabled { get; set; }
    private MainView View;
    public bool TestDeposits { get; set; }
    public long LuminanceMaximum { get; set; }
    public long LuminanceCurrent { get; set; }
    public static long RatingsIncreasePerPoint = 10000000;
    public static long ServerMaxLuminance = 300000000;

    public Dictionary<string, string> Characters { get; set; }

    public event EventHandler OnLuminanceChanged;


    #endregion

    #region ... View Controls ...

    public ViewProperties MainProperties;
    public ControlGroup MainControls;
    public HudView MainView;

    #endregion

    #region ... Startup ...

    protected override void Startup()
    {
      Logger.Create(Host, Core);
      Logger.IsDebuggerMode = false;
#if DEBUG
Logger.IsDebuggerMode = true;
#endif

      LuminanceCurrent = 0;
      LuminanceMaximum = 0;
      Characters = new Dictionary<string, string>();

      Core.CharacterFilter.LoginComplete += CharacterFilter_LoginComplete;
      Core.EchoFilter.ServerDispatch += EchoFilter_ServerDispatch;
      Instance = this;
    }

    #endregion

    #region ... CharacterFilter_LoginComplete ...

    private void CharacterFilter_LoginComplete(object sender, EventArgs e)
    {
      try
      {
        if (!Core.CharacterFilter.Server.Equals(ValHeelServerName, StringComparison.CurrentCultureIgnoreCase))
        {
          IsEnabled = false;
          Logger.WriteMessage($"This plugin only works on the '{ValHeelServerName}' server. Plugin will be disabled.");
          return;
        }
        IsEnabled = true;
        TestDeposits = true;
        CreateViews();
        PopulateCharacterList();
        View.PopulateAccountDetails();
        View.PopulateCharactersList();
        View.PopulateFriendsList();
        View.PopulateIntervalSettings();
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... EchoFilter_ServerDispatch ...

    private void EchoFilter_ServerDispatch(object sender, NetworkMessageEventArgs e)
    {
      if (e == null || e.Message == null) return;

      switch (e.Message.Type)
      {
        case 0x02CF://update qword
          try
          {
            if (e.Message.Value<long>("key") == 6)
            {
              LuminanceCurrent = e.Message.Value<long>("value");
              OnLuminanceChanged?.Invoke(this, EventArgs.Empty);
            }
          }
          catch
          {
            return;
          }
          break;
        case 0xF7B0://login
          try
          {
            if (e.Message.Value<int>("event") != 0x0013)
              return;

            var charProp = e.Message.Value<MessageStruct>("properties");

            var flags = charProp.Value<int>("flags");
            if ((flags & 0x00000080) == 0x00000080)
            {

              var qStruct = charProp.Value<MessageStruct>("qwords");
              if (qStruct.Count > 2)
              {
                LuminanceCurrent = Convert.ToInt64(qStruct.Struct(3)[1]);
                LuminanceMaximum = Convert.ToInt64(qStruct.Struct(4)[1]);
                OnLuminanceChanged?.Invoke(this, EventArgs.Empty);
              }
            }

          }
          catch (Exception ex)
          {
            Logger.WriteMessage($"EchoFilter_ServerDispatch::0xF7B0 Error: {ex.Message}");
            return;
          }
          break;
      }
    }

    #endregion

    #region ... Shutdown ...

    protected override void Shutdown()
    {
      Logger.Destroy();
      View.StopTimer();
      View = null;
      Core.CharacterFilter.LoginComplete -= CharacterFilter_LoginComplete;
      Core.EchoFilter.ServerDispatch -= EchoFilter_ServerDispatch;
    }

    #endregion

    #region ... CreateViews ...

    public void CreateViews()
    {
      try
      {
        Decal3XMLParser parser = new Decal3XMLParser();
        parser.ParseFromResource("ValHeelBanker.Views.MainView.xml", out MainProperties, out MainControls);
        MainView = new HudView(MainProperties, MainControls);
        View = new MainView(this);
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

    #region ... DestroyViews ...

    public void DestroyViews()
    {
      MainProperties = null;
      MainControls = null;
      MainView = null;
      View = null;
    }

    #endregion

    #region ... PopulateCharacterList ...

    private void PopulateCharacterList()
    {
      try
      {
        Characters.Clear();
        string directory = System.IO.Path.Combine(Logger.BasePath, "Profiles");
        foreach (var file in Directory.GetFiles(directory, "*.dat", SearchOption.AllDirectories))
        {
          Settings profile;
          if (Settings.TryLoadProfileFromFile(file, out profile))
          {
            if (!Characters.ContainsKey(profile.CharacterName) && profile.CharacterName != Logger.Core.CharacterFilter.Name)
              Characters.Add(profile.CharacterName, profile.AccountNumber);
          }
        }
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger(ex);
      }
    }

    #endregion

  }
}
