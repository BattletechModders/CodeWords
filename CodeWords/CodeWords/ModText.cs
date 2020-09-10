using System.Collections.Generic;

namespace CodeWords
{
    public class ModText
    {
        public const string FT_InjuryResist = "INJURY_RESIST";

        public Dictionary<string, string> Floaties = new Dictionary<string, string>
        {
            { FT_InjuryResist, "INJURY RESISTED!" },
        };

        public Dictionary<string, Dictionary<string, List<string>>> FactionNames = new Dictionary<string, Dictionary<string, List<string>>>
        {

        };

        public Dictionary<string, List<string>> DefaultNames = new Dictionary<string, List<string>>()
        {
            {  "AmbushConvoy", new List<string>() { "Stopped Traffic", "Traffic Stop", "Arrested Development" } },
            {  "Assassinate", new List<string>() { "Heads Rolling", "Ignore Senate", "Impaired Leadership" } },
            {  "DefendBase", new List<string>() { "Infected Limb", "Lanced Boil", "Rusty Ship" } },
            {  "DestroyBase", new List<string>() { "Shadow Strike", "Burn it down", "Wildfire" } },
            {  "CaptureBase", new List<string>() { "Dejected Theory", "Diligent Restraint", "Cruel Destiny" } },
            {  "CaptureEscort", new List<string>() { "Acqusitions Inc.", "Pitched Battle", "Crystal Cathedral" } },
            {  "CoopBattle", new List<string>() { "Tag Team", "Iron Ring", "Bitter Pill" } },
            {  "EscortSingle", new List<string>() { "Night Out", "Bodyguard", "Interceptor", "Hawk Eyes" } },
            {  "Rescue", new List<string>() { "Water Landing", "Alpine Recovery" } },
            {  "SimpleBattle", new List<string>() { "Iron Fist", "Frozen Hell", "Bison Brawl" } },
            {  "ThreeWayBattle", new List<string>() { "Menage A Trois", "Three Way Stop", "Traffic Stop" } },
            {  "TravelOnly", new List<string>() { "Weary Wanderer", "Road Trip" } },
        };

        public List<string> FallbackNames = new List<string>()
        {
            "Above the Belt", "Below the Belt", "Arctic Rage", "Furious Assault", "Hidden Threat"
        };
    }
}
