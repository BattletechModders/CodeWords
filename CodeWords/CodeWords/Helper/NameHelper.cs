using BattleTech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWords.Helper
{
    public static class NameHelper
    {
        public static string DebugString(this Contract contract)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"  name: {contract.Name}");
            sb.Append($"  type: {contract?.ContractTypeValue?.Name}");
            sb.Append($"  guid: {contract?.GUID}");
            sb.Append($"  encounterObjectGuid: {contract?.encounterObjectGuid}");
            sb.Append($"  ExpirationTime: {contract?.ExpirationTime}");
            sb.Append($"  serialziedContractOverride: {contract?.serialziedContractOverride}");
            sb.Append($"  hashCode: {contract?.GetHashCode()}");
            sb.Append("  == lanceDefs ==");
            foreach (string lanceDef in contract.LanceDefinitions)
            {
                sb.Append($"    lanceDef: {lanceDef}");
            }
            sb.Append($"  == override ==");
            sb.Append($"    ID: {contract?.Override?.ID}");
            sb.Append($"    mapMood: {contract?.Override?.mapMood}");
            sb.Append($"    travelSeed: {contract?.Override?.travelSeed}");
            sb.Append($"    expirationTimeOverride: {contract?.Override?.expirationTimeOverride}");

            return sb.ToString();
        }

        public static string GetOrCreateCodename(Contract contract)
        {
            if (contract == null) return "BRUTAL ERROR";

            string codename = "BRUTAL ERROR";
            bool hasCodename = ModState.ContractGUIDToCodeName.TryGetValue(contract.GUID, out codename);
            if (!hasCodename)
            {
                Mod.Log.Info?.Write($"Generating codename for contract with guid: {contract.GUID}");

                // See if there's a faction dictionary first
                FactionValue employerFaction = contract.GetTeamFaction(ModConsts.EmployerFactionId);
                Mod.Log.Debug?.Write($"  -- employer is: {employerFaction.Name}  with id: {employerFaction.ID}");

                List<string> namesToUse = null;
                if (! String.IsNullOrEmpty(employerFaction.Name))
                {
                    bool hasFactionNames = Mod.LocalizedText.FactionNames.TryGetValue(employerFaction.Name, out Dictionary<string, List<string>> employerNames);
                    if (hasFactionNames)
                    {
                        // Check for the contract type
                        bool hasFactionContractType = employerNames.TryGetValue(contract.ContractTypeValue.Name, out List<string> employerContractNames);
                        if (hasFactionContractType)
                        {
                            Mod.Log.Debug?.Write(" -- Using employer contract type names.");
                            namesToUse = employerContractNames;
                        }
                    }
                }

                if (namesToUse == null)
                {
                    bool hasContractType = Mod.LocalizedText.DefaultNames.TryGetValue(contract.ContractTypeValue.Name, out List<string> defaultTypeNames);
                    if (hasContractType)
                    {
                        Mod.Log.Debug?.Write(" -- Using default contract type names.");
                        namesToUse = defaultTypeNames;
                    }
                }

                if (namesToUse == null)
                {
                    Mod.Log.Debug?.Write(" -- Using fallback names");
                    namesToUse = Mod.LocalizedText.FallbackNames;
                }

                // Randomly select a name
                int randomIdx = Mod.Random.Next(0, namesToUse.Count - 1);
                codename = namesToUse.ElementAt<string>(randomIdx);
                Mod.Log.Info?.Write($" -- Generated name: {codename}");

                ModState.ContractGUIDToCodeName.Add(contract.GUID, codename);
            }

            return codename;
        }
    }
}
