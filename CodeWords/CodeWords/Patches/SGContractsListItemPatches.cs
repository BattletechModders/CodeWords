﻿using BattleTech;
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

            string codeName = ModConsts.DefaultCodeName;
            //if (contract.Override != null && contract.Override.travelSeed != 0)
            //{
            //    // We are a travel contract, see if we can find the contract by the cache key first
            //    if (String.IsNullOrEmpty(ModState.NullGuidCodename))
            //    {
            //        ModState.NullGuidCodename = NameHelper.GenerateCodename(contract);
            //    }
            //    Mod.Log.Debug?.Write("Using null GUID codename for travel contract.");
            //    codeName = ModState.NullGuidCodename;
            //}
            //else
            //{
            //    codeName = NameHelper.GetOrCreateCodename(contract);
            //}
            codeName = NameHelper.GetOrCreateCodename(contract);

            Mod.Log.Debug?.Write($"SGCLI setting codename to: {codeName}");
            ___contractName.SetText(codeName);
        }
    }
}
