using System;
using System.Globalization;
using UnityEngine;
using XRL.Messages;
using XRL.UI;
using Object = System.Object;

namespace ModoMods.SkillTraining.Utils {
  public static class Output {
    /// <summary>Log a debug message to the game's log file.</summary>
    public static void DebugLog(Object? message) {
      var dt = DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss.fffzzz", CultureInfo.InvariantCulture);
      Debug.Log($"[SkillTraining][{dt}] {message}");
      // For now, output everything in game as well for easier access.
      Message("{{K|" + message + "}}");
    }

    /// <summary>Log a message to the game's log file.</summary>
    public static void Log(Object? message) {
      DebugLog(message);
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