using BattleTech;
using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using CodeWords.Helper;
using Harmony;
using System;

namespace CodeWords.Patches
{
    [HarmonyPatch(typeof(SGContractsListItem), "Init")]
    static class SGContractsListItem_Init
    {
        static void Postfix(SGContractsListItem __instance, Contract contract, SimGameState sim, LocalizableText ___contractName)
        {
            Mod.Log.Trace?.Write("SGCLI:I entered.");
            Mod.Log.Debug?.Write($"SGCLI:I => {contract.DebugString()}");

            if (contract.CanHaveCodename())
            {
                string codeName = ModConsts.DefaultCodeName;
                codeName = NameHelper.GetOrCreateCodename(contract);

                Mod.Log.Debug?.Write($"SGCLI setting codename to: {codeName}");
                ___contractName.SetText(codeName);
            }
        }
    }
}
