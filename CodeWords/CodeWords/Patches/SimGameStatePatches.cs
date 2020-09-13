using BattleTech;
using CodeWords.Helper;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWords.Patches
{
    [HarmonyPatch(typeof(SimGameState), "_OnInit")]
    static class SimGameState__OnInit
    {
        static void Postfix(SimGameState __instance)
        {
            ModState.SimGameState = __instance;
        }
    }

    [HarmonyPatch(typeof(SimGameState), "AddContract")]
    static class SimGameState_AddContract
    {
        static void Postfix(SimGameState __instance)
        {
            Mod.Log.Debug?.Write("SGS:AC entered.");
        }
    }

    [HarmonyPatch(typeof(SimGameState), "AddTravelContract")]
    static class SimGameState_AddTravelContract
    {
        static void Postfix(SimGameState __instance)
        {
            Mod.Log.Debug?.Write("SGS:ATC entered.");
        }
    }

    [HarmonyPatch(typeof(SimGameState), "CreateTravelContract")]
    static class SimGameState_CreateTravelContract
    {
        static void Postfix(SimGameState __instance)
        {
            Mod.Log.Debug?.Write("SGS:CTC entered.");
        }
    }

    [HarmonyPatch(typeof(SimGameState), "Rehydrate")]
    static class SimGameState_Rehydrate
    {
        static void Postfix(SimGameState __instance)
        {
            Mod.Log.Debug?.Write("SGS:R entered.");
        }
    }

    [HarmonyPatch(typeof(SimGameState), "CompleteLanceConfigurationPrep")]
    static class SimGameState_CompleteLanceConfigurationPrep
    {
        static void Prefix(SimGameState __instance)
        {
            Mod.Log.Info?.Write("SGS:CLCP invoked");

            Mod.Log.Debug?.Write($"  SGS.SelectedContract => {__instance?.SelectedContract?.DebugString()}");
            Mod.Log.Debug?.Write($"  SGS.ActiveTravelContract => {__instance?.ActiveTravelContract?.DebugString()}");
            Mod.Log.Debug?.Write($"  SGS.PotentialTravelContract => {__instance?.potentialTravelContract?.DebugString()}");
            Mod.Log.Debug?.Write($"  SGS GlobalContracts: {ModState.SimGameState.GlobalContracts.Count}");
            Mod.Log.Debug?.Write($"  SGS SystemContracts: {ModState.SimGameState.CurSystem.SystemContracts.Count}");
            Mod.Log.Debug?.Write($"  SGS SystemBreadcrumbs: {ModState.SimGameState.CurSystem.SystemBreadcrumbs.Count}");

            if (__instance.SelectedContract != null && String.IsNullOrEmpty(__instance.SelectedContract.GUID))
            {
                Mod.Log.Debug?.Write($"Contract was likely spawned from travel. Try to match it against available contracts.");
                foreach (Contract contract in ModState.SimGameState.GetAllCurrentlySelectableContracts())
                {
                    Mod.Log.Debug?.Write($" -- Contract: {contract.DebugString()}");

                }
                //Contract matchedContract = ModState.SimGameState.GetAllCurrentlySelectableContracts(false).Where(x => x.Override != null && x.Override.travelSeed != 0).First();
                //if (matchedContract != null)
                //{
                //    Mod.Log.Debug?.Write($"SGS matched contract is: {matchedContract.DebugString()}, updating GUID on SelectedContract to: {matchedContract.GUID}");
                //    ModState.ActiveContractGUID = matchedContract.GUID;
                //}
                //else
                //{
                //    Mod.Log.Warn?.Write("Failed to match a contract, I expect breakage!");
                //}

            }
        }
    }
}
