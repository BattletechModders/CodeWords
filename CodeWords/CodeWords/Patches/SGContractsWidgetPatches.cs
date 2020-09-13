using BattleTech;
using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using CodeWords.Helper;
using Harmony;
using System;
using System.Collections.Generic;

namespace CodeWords.Patches
{
    [HarmonyPatch(typeof(SGContractsWidget), "ListContracts")]
    static class SGContractsWidget_ListContracts
    {
        static void Prefix(List<Contract> contracts)
        {
            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            Mod.Log.Debug?.Write($"SGCW:LC entered with stacktrace: {t}");

            Mod.Log.Debug?.Write($"Listing contracts fetched from SGS");
            foreach (Contract contract in ModState.SimGameState.GetAllCurrentlySelectableContracts())
            {
                Mod.Log.Debug?.Write($" -- {contract.DebugString()}");
            }
            Mod.Log.Debug?.Write("  DONE");

            Mod.Log.Debug?.Write($"  SGS.SelectedContract => {ModState.SimGameState?.SelectedContract?.DebugString()}");
            Mod.Log.Debug?.Write($"  SGS.ActiveTravelContract => {ModState.SimGameState?.ActiveTravelContract?.DebugString()}");
            Mod.Log.Debug?.Write($"  SGS.PotentialTravelContract => {ModState.SimGameState?.potentialTravelContract?.DebugString()}");
            Mod.Log.Debug?.Write($"  SGS GlobalContracts: {ModState.SimGameState.GlobalContracts.Count}");
            Mod.Log.Debug?.Write($"  SGS SystemContracts: {ModState.SimGameState.CurSystem.SystemContracts.Count}");
            Mod.Log.Debug?.Write($"  SGS SystemBreadcrumbs: {ModState.SimGameState.CurSystem.SystemBreadcrumbs.Count}");


            Mod.Log.Debug?.Write("Listing parameter contracts: ");
            foreach (Contract contract in contracts)
            {
                Mod.Log.Debug?.Write($" -- {contract.DebugString()}");
            }
            Mod.Log.Debug?.Write("  DONE");
        }

    }


    [HarmonyPatch(typeof(SGContractsWidget), "PopulateContract")]
    static class SGContractsWidget_PopulateContract
    {
        static void Postfix(SGContractsWidget __instance, Contract contract, LocalizableText ___MetaMissionTitleField)
        {
            Mod.Log.Trace?.Write("SGCW:PC entered.");

            string codeName = "BRUTAL ERROR";
            if (contract != null && !String.IsNullOrEmpty(contract.GUID))
            {
                Mod.Log.Debug?.Write($"SGCW:PC Populating details for contract: {contract.DebugString()}");
                codeName = NameHelper.GetOrCreateCodename(contract);
            }
            else if (!String.IsNullOrEmpty(ModState.ActiveContractGUID))
            {
                bool hasCodename = ModState.ContractGUIDToCodeName.TryGetValue(ModState.ActiveContractGUID, out codeName);
                if (!hasCodename) codeName = "BRUTAL ERROR";
            }
            
            Mod.Log.Debug?.Write($"SGCW:PC setting codename to: {codeName}");
            ___MetaMissionTitleField.SetText(codeName);
        }
    }

    [HarmonyPatch(typeof(SGContractsWidget), "SelectDesiredContract")]
    static class SGContractsWidget_SelectDesiredContract
    {
        static void Postfix(SGContractsWidget __instance, Contract desiredContract, LocalizableText ___MetaMissionTitleField)
        {
            Mod.Log.Trace?.Write("SGCW:SDC entered.");

            Mod.Log.Debug?.Write($"SGCW:SDC Populating details for contract: {desiredContract.DebugString()}");

            string codeName = NameHelper.GetOrCreateCodename(desiredContract);
            Mod.Log.Debug?.Write($"SGCW:SDC setting codename to: {codeName}");
            ___MetaMissionTitleField.SetText(codeName);

            ModState.ActiveContractGUID = desiredContract.GUID;
        }
    }
}
