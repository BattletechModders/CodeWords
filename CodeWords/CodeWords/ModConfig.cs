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
