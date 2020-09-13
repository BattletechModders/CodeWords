using BattleTech;
using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using CodeWords.Helper;
using Harmony;

namespace CodeWords.Patches
{
    [HarmonyPatch(typeof(SGContractsListItem), "Init")]
    static class SGContractsListItem_Init
    {
        static void Postfix(SGContractsListItem __instance, Contract contract, SimGameState sim, LocalizableText ___contractName)
        {
            Mod.Log.Trace?.Write("SGCLI:I entered.");
            //System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            //Mod.Log.Debug?.Write($"SGCLI:I entered with stacktrace: {t}");

            Mod.Log.Debug?.Write(contract.DebugString());

            string codeName = NameHelper.GetOrCreateCodename(contract);
            Mod.Log.Debug?.Write($"SGCLI setting codename to: {codeName}");
            ___contractName.SetText(codeName);
        }
    }
}
