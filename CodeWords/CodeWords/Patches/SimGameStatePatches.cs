using BattleTech.Save;
using CodeWords.Helper;
using System;
using System.Collections.Generic;

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
        static void Prefix(ref bool __runOriginal, SimGameState __instance, List<ContractData> ___contractBits)
        {
            if (!__runOriginal) return;

            Mod.Log.Debug?.Write("SGS:R entered.");

            Mod.Log.Debug?.Write(" == CONTRACT DATA ==");
            foreach (ContractData cd in ___contractBits)
            {
                Mod.Log.Debug?.Write($"  -- name: {cd.conName} guid: {cd.GUID}  target: {cd.conTarget}  employer: {cd.conEmployer}  location: {cd.conLocation} ");
            }
            Mod.Log.Debug?.Write(" == DONE ==");
        }
    }

    [HarmonyPatch(typeof(SimGameState), "PrepareBreadcrumb")]
    static class SimGameState_PrepareBreadcrumb
    {
        static void Postfix(SimGameState __instance)
        {
            Mod.Log.Debug?.Write("SGS:PB entered.");
        }
    }


    [HarmonyPatch(typeof(SimGameState), "CompleteLanceConfigurationPrep")]
    static class SimGameState_CompleteLanceConfigurationPrep
    {
        static void Prefix(ref bool __runOriginal, SimGameState __instance)
        {
            if (!__runOriginal) return;

            Mod.Log.Info?.Write("SGS:CLCP invoked");

            Mod.Log.Debug?.Write($"  SGS.SelectedContract => {__instance?.SelectedContract?.DebugString()}");
            Mod.Log.Debug?.Write($"  SGS.ActiveTravelContract => {__instance?.ActiveTravelContract?.DebugString()}");
            Mod.Log.Debug?.Write($"  SGS.PotentialTravelContract => {__instance?.potentialTravelContract?.DebugString()}");
            Mod.Log.Debug?.Write($"  SGS GlobalContracts: {ModState.SimGameState.GlobalContracts.Count}");
            Mod.Log.Debug?.Write($"  SGS SystemContracts: {ModState.SimGameState.CurSystem.SystemContracts.Count}");
            Mod.Log.Debug?.Write($"  SGS SystemBreadcrumbs: {ModState.SimGameState.CurSystem.SystemBreadcrumbs.Count}");

            if (__instance.SelectedContract != null && String.IsNullOrEmpty(__instance.SelectedContract.GUID))
            {
                Mod.Log.Debug?.Write($"SGS:CLCP Contract was likely spawned from travel. Try to match it against available contracts.");
                foreach (Contract contract in ModState.SimGameState.GetAllCurrentlySelectableContracts())
                {
                    Mod.Log.Debug?.Write($" -- Contract: {contract.DebugString()}");
                }


            }
        }
    }
}
