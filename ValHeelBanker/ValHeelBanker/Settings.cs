using Decal.Adapter.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ValHeelBanker
{
  public class Settings
  {
    #region ... Properties ...

    public string CharacterName { get; set; }
    public string AccountNumber { get; set; }
    public int Pyreals { get; set; }
    public long Luminance { get; set; }
    public int AshCoin { get; set; }
    public int PyrealSavings { get; set; }

    public bool DepositPyrealsPeriodically { get; set; }
    public bool DepositLuminancePeriodically { get; set; }

    public int PyrealsSaveInterval { get; set; }
    public PyrealsDepositTypes PyrealDepositType { get; set; }
    public int PyrealsDepositAmount { get; set; }

    public int LuminanceSaveInterval { get; set; }
    public LuminanceDepositTypes LuminanceDepositType { get; set; }
    public int LuminanceDepositAmount { get; set; }

    public SerializableDictionary<string, string> FriendsList { get; set; }

    #endregion

    #region ... Constructor ...

    public Settings()
    {
      CharacterName= string.Empty;
      AccountNumber = string.Empty;
      Pyreals = 0;
      Luminance= 0;
      AshCoin= 0;
      PyrealSavings= 0;

      DepositPyrealsPeriodically= false;
      DepositLuminancePeriodically= false;

      PyrealsSaveInterval = 30;
      PyrealDepositType = PyrealsDepositTypes.Difference;
      PyrealsDepositAmount= 0;

      LuminanceSaveInterval = 5;
      LuminanceDepositType = LuminanceDepositTypes.Specified;
      LuminanceDepositAmount = 100000;

      FriendsList= new SerializableDictionary<string, string>();
    }

    #endregion

    #region ... Static Load ...

    public static Settings Load(string basePath)
    {
      try
      {
        string directory = Path.Combine(basePath, "Profiles");
        directory = Path.Combine(directory, Logger.Core.CharacterFilter.AccountName);
        if (!Directory.Exists(directory))
          Directory.CreateDirectory(directory);
        string fileName = Path.Combine(directory, $"{Logger.Core.CharacterFilter.Name}_ValHeelBankSettings.dat");
        if (!File.Exists(fileName))
        {
          Settings settings = new Settings();
          settings.CharacterName = Logger.Core.CharacterFilter.Name;
          Save(basePath, settings);
          return settings;
        }
        string data;
        using (StreamReader sr = new StreamReader(fileName))
        {
          data = sr.ReadToEnd();
        }
        return Serialization.Deserialize<Settings>(data);
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger("Error loading settings file. Exception: " + ex.ToString());
        return null;
      }
    }

    #endregion

    #region ... Static Save ...

    public static bool Save(string basePath, Settings settings)
    {
      try
      {
        string directory = Path.Combine(basePath, "Profiles");
        directory = Path.Combine(directory, Logger.Core.CharacterFilter.AccountName);
        if (!Directory.Exists(directory))
          Directory.CreateDirectory(directory);
        string fileName = Path.Combine(directory, $"{Logger.Core.CharacterFilter.Name}_ValHeelBankSettings.dat");
        string data = Serialization.Serialize<Settings>(settings);
        if (string.IsNullOrEmpty(data))
          return false;
        using (StreamWriter sw = new StreamWriter(fileName, false))
        {
          sw.Write(data);
          sw.Flush();
        }
        return true;
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger("Error saving settings file to disk. Exception: " + ex.ToString());
        return false;
      }
    }

    #endregion

    #region ... Static TryLoadProfileFromFile ...

    public static bool TryLoadProfileFromFile(string filename, out Settings profile)
    {
      try
      {
        if (!File.Exists(filename))
        {
          profile = new Settings();
          return false;
        }
        string data;
        using (StreamReader sr = new StreamReader(filename))
        {
          data = sr.ReadToEnd();
        }
        profile = Serialization.Deserialize<Settings>(data);
        return  true;
      }
      catch (Exception ex)
      {
        Logger.WriteDebugger("Error loading profile file. Exception: " + ex.ToString());
        profile = new Settings();
        return false;
      }
    }

    #endregion

    public enum PyrealsDepositTypes
    {
      Specified = 0,
      Maximum = 1,
      Difference = 2
    }

    public enum LuminanceDepositTypes
    {
      Specified = 0,
      Maximum = 1,
    }


  }
}
