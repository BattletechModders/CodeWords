using BattleTech;
using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using CodeWords.Helper;
using Harmony;
using System;
using System.Linq;

namespace CodeWords.Patches
{
    [HarmonyPatch(typeof(LanceContractDetailsWidget), "PopulateContract")]
    static class LanceContractDetailsWidget_PopulateContract
    {
        static void Postfix(LanceContractDetailsWidget __instance, Contract contract, LocalizableText ___MetaMissionTitleField)
        {
            Mod.Log.Trace?.Write("LCDW:PC entered.");

            // We expect contract.GUID to always be set here
            if (String.IsNullOrEmpty(contract.GUID))
            {
                Mod.Log.Warn?.Write($"LCDW contract has a null GUID - bailing!.");
                return;
            }

            Mod.Log.Info?.Write($"LCWD looking up contract.GUID: {ModState.ActiveContractGUID}");
            bool hasCodename = ModState.ContractGUIDToCodeName.TryGetValue(ModState.ActiveContractGUID, out string codeName);
            if (!hasCodename) codeName = "BRUTAL ERROR";

            Mod.Log.Info?.Write($"LCDW setting codename to: {codeName}");
            ___MetaMissionTitleField.SetText(codeName);
        }
    }
}
