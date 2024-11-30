using XRL.World;

namespace ModoMods.SkillTraining.Utils {
  public static class EventUtils {
    public static GameObject? Actor(this Event ev) => ev.GetGameObjectParameter("Actor");
    public static GameObject? Attacker(this Event ev) => ev.GetGameObjectParameter("Attacker");
    public static GameObject? Defender(this Event ev) => ev.GetGameObjectParameter("Defender");
    public static GameObject? Weapon(this Event ev) => ev.GetGameObjectParameter("Weapon");
  }
}