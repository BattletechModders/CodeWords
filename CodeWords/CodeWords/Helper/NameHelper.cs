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
            sb.Append($"  biome: {contract?.ContractBiome}");
            sb.Append($"  difficulty: {contract?.Difficulty}");
            sb.Append($"  targetSystem: {contract?.TargetSystem}");
            sb.Append($"  timeElapsed: {contract?.TimeElapsed}");
            sb.Append($"  == override ==");
            sb.Append($"    ID: {contract?.Override?.ID}");
            sb.Append($"    Name: {contract?.Override?.contractName}");
            sb.Append($"    Difficulty: {contract?.Override?.difficulty}");
            sb.Append($"    DifficultyUIMod: {contract?.Override?.difficultyUIModifier}");
            sb.Append($"    FinalDifficulty: {contract?.Override?.finalDifficulty}");
            sb.Append($"    Filename: {contract?.Override?.filename}");
            sb.Append($"    Folder: {contract?.Override?.folder}");
            sb.Append($"    negiotiatedSalary: {contract?.Override?.negotiatedSalary}");
            sb.Append($"    negiotiatedSalvage: {contract?.Override?.negotiatedSalvage}");
            sb.Append($"    salvagePotential: {contract?.Override?.salvagePotential}");
            sb.Append($"    travelSeed: {contract?.Override?.travelSeed}");
            sb.Append($"    weight: {contract?.Override?.weight}");

            return sb.ToString();
        }

        public static string SemiUniqueHash(this Contract contract)
        {

            // 21:48:51.900 Generating codename for contract:   name: Covert Supplies  type: AmbushConvoy  
            // guid: SRC<the one and only>_AG_4291  biome: lunarVacuum  difficulty: 5  
            //  targetSystem: starsystemdef_Itica  timeElapsed: 0  == override ==    
            //  ID: AmbushConvoy_CovertSupplies    Name: Covert Supplies    Difficulty: 5    
            // DifficultyUIMod: 0    FinalDifficulty: 5    Filename:     Folder:     
            //  negiotiatedSalary: 1    negiotiatedSalvage: 0    salvagePotential: 16    travelSeed: 0    weight: 1

            StringBuilder sb = new StringBuilder();
            sb.Append(contract.Override.ID);
            sb.Append("_");
            sb.Append(contract.ContractBiome);
            sb.Append("_");
            sb.Append(contract.Override.difficulty);
            sb.Append("_");
            sb.Append(contract.Override.difficultyUIModifier);
            sb.Append("_");
            sb.Append(contract.Override.finalDifficulty);
            sb.Append("_");
            sb.Append(contract.Override.negotiatedSalary);
            sb.Append("_");
            sb.Append(contract.Override.negotiatedSalvage);
            sb.Append("_");
            sb.Append(contract.Override.salvagePotential);
            sb.Append("_");
            sb.Append(contract.Override.travelSeed);
            sb.Append("_");
            sb.Append(contract.Override.weight);

            return sb.ToString();
        }

        public static string GenerateCodename(Contract contract)
        {
            Mod.Log.Info?.Write($"Generating codename for contract: {contract.DebugString()}");

            // See if there's a faction dictionary first
            FactionValue employerFaction = contract.GetTeamFaction(ModConsts.EmployerFactionId);
            Mod.Log.Debug?.Write($"  -- employer is: {employerFaction.Name}  with id: {employerFaction.ID}");

            List<string> namesToUse = null;
            if (!String.IsNullOrEmpty(employerFaction.Name))
            {
                bool hasFactionNames = Mod.LocalizedText.FactionNames.TryGetValue(employerFaction.Name, out Dictionary<string, List<string>> employerNames);
                if (hasFactionNames && employerNames.Count > 0)
                {
                    // Check for the contract type
                    bool hasFactionContractType = employerNames.TryGetValue(contract.ContractTypeValue.Name, out List<string> employerContractNames);
                    if (hasFactionContractType && employerContractNames.Count > 0)
                    {
                        Mod.Log.Debug?.Write(" -- Using employer contract type names.");
                        namesToUse = employerContractNames;
                    }
                }
            }

            if (namesToUse == null)
            {
                bool hasContractType = Mod.LocalizedText.DefaultNames.TryGetValue(contract.ContractTypeValue.Name, out List<string> defaultTypeNames);
                if (hasContractType && defaultTypeNames.Count > 0)
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
            string codename = ModConsts.DefaultCodeName;
            if (namesToUse.Count > 0)
            {
                int randomIdx = Mod.Random.Next(0, namesToUse.Count - 1);
                codename = namesToUse.ElementAt<string>(randomIdx);
            }
            Mod.Log.Info?.Write($" -- Generated name: {codename}");

            return codename;
        }

        public static string GetOrCreateCodename(Contract contract)
        {
            if (contract == null) return ModConsts.DefaultCodeName;

            string cacheKey = contract.SemiUniqueHash();
            string codename = ModConsts.DefaultCodeName;
            bool hasKey = ModState.ContractGUIDToCodeName.TryGetValue(cacheKey, out codename);
            if (!hasKey)
            {
                codename = GenerateCodename(contract);
                ModState.ContractGUIDToCodeName.Add(cacheKey, codename);
            }

            return codename;
        }


    }
}
