using BattleTech;
using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using CodeWords.Helper;
using Harmony;
using System;
using System.Collections.Generic;

namespace CodeWords.Patches
{

    [HarmonyPatch(typeof(SGContractsWidget), "OnContractAccepted")]
    [HarmonyPatch(new Type[] { typeof(bool) })]
    static class SGContractsWidget_OnContractAccepted
    {
        static void Postfix(SimGameState __instance)
        {
            Mod.Log.Debug?.Write("SGCW:OCA entered.");
        }
    }

    [HarmonyPatch(typeof(SGContractsWidget), "PopulateContract")]
    static class SGContractsWidget_PopulateContract
    {
        static void Postfix(SGContractsWidget __instance, Contract contract, LocalizableText ___MetaMissionTitleField)
        {
            Mod.Log.Trace?.Write("SGCW:PC entered.");

            string codeName = NameHelper.GetOrCreateCodename(contract);

            //if (contract != null && !String.IsNullOrEmpty(contract.GUID))
            //{
            //    Mod.Log.Debug?.Write($"SGCW:PC Populating details for contract: {contract.DebugString()}");
            //    codeName = NameHelper.GetOrCreateCodename(contract);
            //}
            //else if (!String.IsNullOrEmpty(ModState.ActiveContractGUID))
            //{
            //    Mod.Log.Debug?.Write($"SGCW:PC looking up contract with GUID: {ModState.ActiveContractGUID}");
            //    bool hasCodename = ModState.ContractGUIDToCodeName.TryGetValue(ModState.ActiveContractGUID, out codeName);
            //    if (!hasCodename) codeName = ModConsts.DefaultCodeName;
            //}

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
