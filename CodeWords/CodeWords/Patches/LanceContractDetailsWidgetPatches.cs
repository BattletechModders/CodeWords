﻿using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using CodeWords.Helper;

namespace CodeWords.Patches
{
    [HarmonyPatch(typeof(LanceContractDetailsWidget), "PopulateContract")]
    static class LanceContractDetailsWidget_PopulateContract
    {
        static void Postfix(LanceContractDetailsWidget __instance, Contract contract, LocalizableText ___MetaMissionTitleField)
        {
            Mod.Log.Trace?.Write("LCDW:PC entered.");

            Mod.Log.Info?.Write($"LCWD looking up contract.GUID: {ModState.ActiveContractGUID}");

            if (contract.CanHaveCodename())
            {
                string codeName = NameHelper.GetOrCreateCodename(contract);
                Mod.Log.Info?.Write($"LCDW setting codename to: {codeName}");
                ___MetaMissionTitleField.SetText(codeName);
            }

        }
    }
}
