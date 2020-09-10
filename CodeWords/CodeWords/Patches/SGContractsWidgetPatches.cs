using BattleTech;
using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using CodeWords.Helper;
using Harmony;

namespace CodeWords.Patches
{
    [HarmonyPatch(typeof(SGContractsWidget), "PopulateContract")]
    static class SGContractsWidget_PopulateContract
    {
        static void Postfix(SGContractsWidget __instance, Contract contract, LocalizableText ___MetaMissionTitleField)
        {
            Mod.Log.Trace?.Write("SGCW:PC entered.");

            Mod.Log.Debug?.Write($"Populating details for contract GUID: '{contract.GUID}' with typeID: {contract.ContractTypeValue.ID} with typeValue.name: {contract.ContractTypeValue.Name}");

            string codeName = NameHelper.GetOrCreateCodename(contract);
            ___MetaMissionTitleField.SetText(codeName);
        }
    }
}
