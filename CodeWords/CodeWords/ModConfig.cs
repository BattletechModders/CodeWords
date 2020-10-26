namespace CodeWords
{

    public class ModConfig
    {

        // If true, many logs will be printed
        public bool Debug = false;
        // If true, all logs will be printed
        public bool Trace = false;

        // The % of names that should come from the employer
        public float FactionNameWeight = 0.2f;

        public string[] ExcludeContractsNamed = new string[]
        {
            "itrom_attack", "itrom_defense", "panzyr_attack", "panzr_defense",
            "smithon_attack", "tyrlon_attack", "story_1a_retreat", "story_1b_retreat",
            "story_2_threeyearslater", "story_3_axylus", "story_4_liberationofweldry",
            "story_5_servedcold", "story_6a_treasuretrove", "story_6b_treasuretrove",
            "story_7_gunboatdiplomacy", "story_8_locura", "story_9_downfall"
        };

        public void LogConfig()
        {
            Mod.Log.Info?.Write("=== MOD CONFIG BEGIN ===");
            Mod.Log.Info?.Write($"  DEBUG: {this.Debug} Trace: {this.Trace}");
        }

        public void Init()
        {

        }
    }
}
