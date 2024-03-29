﻿using Localize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CodeWords.ModText;

namespace CodeWords.Helper
{
    public static class ContractExtensions
    {
        public static bool CanHaveCodename(this Contract contract)
        {


            if (contract != null)
            {
                Mod.Log.Debug?.Write($"Evaluating contract =>  overrideId: '{contract.OverrideID}'  name: '{contract.Name}'  internalName: '{contract.internalName}'  contractTypeId: '{contract.contractTypeID}'");
                Mod.Log.Debug?.Write($"    IsFlashpointCampaignContract: {contract.IsFlashpointCampaignContract}  IsFlashpointContract: {contract.IsFlashpointContract}  isStoryContract: {contract.IsStoryContract}" +
                    $"  isTutorial: {contract.IsTutorial}  isRestorationContract: {contract.IsRestorationContract}");

                // Check for default exclusiosn; no flashpoints, story contracts, tutorials, etc
                if (contract.IsFlashpointCampaignContract ||
                    contract.IsFlashpointContract ||
                    contract.IsStoryContract ||
                    contract.IsTutorial ||
                    contract.IsRestorationContract)
                {
                    Mod.Log.Info?.Write($"Contract is of an excluded type, cannot use a codename.");
                    return false;
                }

                if (contract.Override != null)
                {
                    Mod.Log.Debug?.Write($"  contractOverride =>  overrideId: '{contract.Override.ID}'  contractName: '{contract.Override.contractName}'  contractTypeValue: '{contract.Override.contractTypeValue}'");

                    string contractId = contract.Override.ID ?? contract.OverrideID;                    
                    foreach (string excluded in Mod.Config.ExcludeContractsWithId)
                    {
                        if (excluded.Equals(contract.Override.ID, StringComparison.InvariantCultureIgnoreCase))
                        {
                            Mod.Log.Info?.Write($"Found codename exclusions for contractID: {contract.Override.ID}, skipping.");
                            return false;
                        }
                    }

                    foreach (string excluded in Mod.Config.ExcludeContractsWithName)
                    {
                        string name = contract.Override.contractName ?? contract.Name;
                        if (excluded.ToLower().Equals(contract.Name.ToLower(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            Mod.Log.Info?.Write($"Found codename exclusions for contract with name: '{name}', skipping.");
                            return false;
                        }
                    }
                }
            }

            Mod.Log.Debug?.Write($"No exclusions found for contract name: `{contract?.Name}'  ID: '{contract?.Override?.ID}', enabling codenames.");
            return true;
        }
    }

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

        static string GenerateRandomName(string prefix)
        {
            // Generate a random name from the adjective and nouns
            int adjIdx = Mod.Random.Next(Mod.LocalizedText.Adjectives.Count - 1);
            string adjective = Mod.LocalizedText.Adjectives[adjIdx];
            int nounIdx = Mod.Random.Next(Mod.LocalizedText.Nouns.Count - 1);
            string noun = Mod.LocalizedText.Nouns[nounIdx];

            string name = new Text(Mod.LocalizedText.ContractNameFormat, new object[] { prefix, adjective, noun }).ToString();
            Mod.Log.Debug?.Write($"Generated name: {name} from adjective: {adjective} and noun: {noun}");

            return name;
        }

        public static string GenerateCodename(string employerFactionName, string ContractID)
        {
            Mod.Log.Info?.Write($"Generating codename for contract with employerName: {employerFactionName}");

            List<string> names = new List<string>(10);

            // See if there are any factional names to add
            int factionNameCount = (int)Math.Floor(Mod.Config.FactionNameWeight * 10);
            string prefix = GetPrefix(ContractID, out bool BypassFactionCheck);
            if (!BypassFactionCheck && !string.IsNullOrEmpty(employerFactionName))
            {
                bool hasFaction = Mod.LocalizedText.FactionNames.TryGetValue(employerFactionName, out FactionStrings employerStrings);
                if (hasFaction)
                {
                    if (!string.IsNullOrEmpty(employerStrings.ContractPrefix)) prefix = employerStrings.ContractPrefix;

                    List<string> fNames = new List<string>();
                    fNames.AddRange(employerStrings.ContractNames);

                    if (factionNameCount > employerStrings.ContractNames.Count) factionNameCount = employerStrings.ContractNames.Count;
                    Mod.Log.Debug?.Write($" -- Will generate {factionNameCount} factionNames");
                    for (int i = 0; i < factionNameCount; i++)
                    {
                        int fNameIdx = Mod.Random.Next(fNames.Count - 1);
                        string fName = fNames.ElementAt(fNameIdx);
                        if (!string.IsNullOrEmpty(fName))
                        {
                            string name = new Text(Mod.LocalizedText.ContractNameFormat, new object[] { prefix, "", fName }).ToString();
                            names.Add(name);
                        }
                        fNames.RemoveAt(fNameIdx);
                    }
                }
            }
            Mod.Log.Debug?.Write($" -- Contract prefix: {prefix}");

            // Build generic names next
            int genericNameCount = 10 - names.Count;
            Mod.Log.Debug?.Write($" -- Will generate {genericNameCount} generic names.");
            for (int i = 0; i < genericNameCount; i++)
            {
                names.Add(GenerateRandomName(prefix));
            }

            // Check names for a repeat
            string generatedName = ModConsts.DefaultCodeName;
            names.Shuffle();
            foreach (string name in names)
            {
                if (ModState.NameBlacklist.Contains(name)) continue;
                generatedName = name;
                break;
            }

            return generatedName;
        }

        private static string GetPrefix(string ContractID, out bool BypassFactionCheck)
        {
            if (Mod.LocalizedText.SpecificContractPrefixes.Count != 0)
            {
                foreach (var Mapping in Mod.LocalizedText.SpecificContractPrefixes)
                {
                    if (Mapping.Value.Contains(ContractID))
                    {
                        BypassFactionCheck = true;
                        return Mapping.Key;
                    }
                }

            }
            BypassFactionCheck = false;
            return Mod.LocalizedText.DefaultContractPrefix;
        }

        public static string GetOrCreateCodename(Contract contract)
        {
            if (contract == null) return ModConsts.DefaultCodeName;

            string cacheKey = contract.SemiUniqueHash();
            string codename = ModConsts.DefaultCodeName;
            bool hasKey = ModState.ContractGUIDToCodeName.TryGetValue(cacheKey, out codename);
            if (!hasKey)
            {
                FactionValue employerFaction = contract.GetTeamFaction(ModConsts.EmployerFactionId);
                codename = GenerateCodename(employerFaction.Name, contract.Override.ID);
                ModState.ContractGUIDToCodeName.Add(cacheKey, codename);
                ModState.NameBlacklist.Add(codename);
            }

            return codename;
        }

        // Do a Fisher-Yates shuffle; see https://stackoverflow.com/questions/273313/randomize-a-listt
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Mod.Random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }
}
