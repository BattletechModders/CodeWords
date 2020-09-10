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

            Mod.Log.Debug?.Write($"Populating details for contract: {contract.GUID} with typeID: {contract.ContractTypeValue.ID} with typeValue.name: {contract.ContractTypeValue.Name}");

            string codeName = NameHelper.GetOrCreateCodename(contract);
            ___contractName.SetText(codeName);
        }
    }
}
