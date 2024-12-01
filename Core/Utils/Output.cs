using System;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using XRL.Messages;
using XRL.UI;
using Object = System.Object;

namespace ModoMods.Core.Utils {
  public static class Output {
    /// <summary>Log a debug message to the game's log file.</summary>
    public static void DebugLog(Object? message, Boolean inGame = false) {
      var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffzzz", CultureInfo.InvariantCulture);
      var mod = Assembly.GetCallingAssembly().GetName().Name.Replace(".dll", "");
      Debug.Log($"[{dt}][{mod}] {message}");
      if (inGame)
        Message("{{K|" + message + "}}");
    }

    /// <summary>Log a message to the game's log file.</summary>
    public static void Log(Object? message) {
      DebugLog(message, inGame: true);
    }

    /// <summary>
    /// Add a message to the in-game message log.
    /// </summary>
    public static void Message(String text) {
      MessageQueue.AddPlayerMessage(text);
    }

    /// <summary>
    /// Show a popup message to the player in game.
    /// </summary>
    public static void Alert(String? text) {
      Popup.Show(text ?? "");
    }
  }
}