using Decal.Adapter;
using Decal.Adapter.Wrappers;
using System;
using System.IO;

namespace ValHeelBanker
{
  internal class Logger
  {
    #region ... Enums ...

    public enum ChatColors
    {
      Green = 1,
      White = 2,
      BrightGold = 3,
      DullGold = 4,
      Purple = 5,
      Red = 6,
      Blue = 7,
      Pink = 8
    }

    #endregion

    #region ... Properties ...

    public static PluginHost Host;
    public static CoreManager Core;
    public static string PluginName = "ValHeel Banker";
    public static string PluginNameAbr = "VHB";

    public static bool IsDebuggerMode = false;
    public static bool LogToFile = true;

    public static string BasePath = string.Empty;

    #endregion

    #region ... Create ...

    public static void Create(PluginHost host, CoreManager core)
    {
      Host = host;
      Core = core;
      var filePath = string.Empty;
      try
      {
        filePath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString().Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.ToString().LastIndexOf("\\"));
      }
      catch
      {
        filePath = @"C:\Turbine\Decal Plugins\ValHeelBanker";
      }
      BasePath = filePath;
    }

    #endregion

    #region ... Destroy ...

    public static void Destroy()
    {
      Host = null;
      Core= null;
    }

    #endregion

    #region ... WriteMessage ...

    public static void WriteMessage(string message, int chatWindow) { WriteMessage(message, $"[{PluginNameAbr}]", chatWindow); }
    public static void WriteMessage(string message) { WriteMessage(message, $"[{PluginNameAbr}]"); }
    public static void WriteMessage(string message, string prepend) { WriteMessage(message, prepend, -1); }
    public static void WriteMessage(string message, string prepend, int chatWindow)
    {
      if (Host == null)
        return;
      try
      {
        string line = prepend + " " + message;
        if (chatWindow < 1)
          Host.Actions.AddChatText(line, (int)ChatColors.White);
        else
          Host.Actions.AddChatText(line, (int)ChatColors.White, chatWindow + 1);
      }
      catch {; }
    }

    #endregion

    #region ... WriteDebugger ...

    public static void WriteDebugger(Exception ex) { WriteDebugger(ex, LogToFile); }
    public static void WriteDebugger(Exception ex, bool toFile) { WriteDebugger(ex.ToString(), toFile); }
    public static void WriteDebugger(string text, bool toFile) { WriteDebugger(text, toFile, $"<{PluginName}>"); }
    public static void WriteDebugger(string text) { WriteDebugger(text, LogToFile, $"<{PluginName}>"); }
    public static void WriteDebugger(string text, string prepend) { WriteDebugger(text); }
    public static void WriteDebugger(string text, bool toFile, string prepend)
    {
      if (!IsDebuggerMode)
        return;
      var directory = Path.Combine(BasePath, "Logs");
      if(!Directory.Exists(directory))
        Directory.CreateDirectory(directory);

      string line = string.Format("{0} {1}", prepend, text).Trim();
      if (Host != null)
      {
        Host.Actions.AddChatText(line, (int)ChatColors.Red);
      }
      if (toFile)
      {
        string file = string.Empty;
        try
        {
          file = Path.Combine(directory, prepend.Replace("<", "").Replace(">", "") + "_" + DateTime.Now.ToString("yyyyMMdd") + ".log");
          using (StreamWriter sw = new StreamWriter(file, true))
          {
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": " + line);
          }
        }
        catch (Exception ex)
        {
          if (Host != null)
          {
            Host.Actions.AddChatText("LoggerFileException: " + ex.Message, (int)ChatColors.Red);
            Host.Actions.AddChatText("LoggerFileException FilePath: " + file, (int)ChatColors.Red);
          }
        }
      }
    }

    #endregion

    #region ... WriteToChat ...

    public static void WriteToChat(string message)
    {
      if (Host != null)
      {
        Logger.WriteDebugger("InvokeChatParser: " + message);
        Host.Actions.InvokeChatParser(message);
      }
    }

    #endregion
  }
}
