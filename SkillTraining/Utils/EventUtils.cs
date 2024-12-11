using System;
using ModoMods.Core.Data;
using XRL.World;

namespace ModoMods.SkillTraining.Utils {
  public static class EventUtils {
    public static GameObject? Actor(this Event ev) => ev.GetGameObjectParameter("Actor");
    public static GameObject? Attacker(this Event ev) => ev.GetGameObjectParameter("Attacker");
    public static GameObject? Defender(this Event ev) => ev.GetGameObjectParameter("Defender");
    public static GameObject? Weapon(this Event ev) => ev.GetGameObjectParameter("Weapon");
    public static String[] Properties(this Event ev) => 
      ev.GetStringParameter("Properties").Split(',', StringSplitOptions.RemoveEmptyEntries);
    public static Boolean HasProperty(this Event ev, QudEventProperties property) =>
      ev.Properties().Contains(property.ToString());

    public static Boolean IsChargeAttack(this Event ev) => ev.HasProperty(QudEventProperties.Charging);
  }
}