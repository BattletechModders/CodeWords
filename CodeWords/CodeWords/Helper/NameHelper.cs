using BattleTech;
using Localize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CodeWords.ModText;

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

        static string GenerateRandomName(string prefix)
        {
            // Generate a random name from the adjective and nouns
            int adjIdx = Mod.Random.Next(Mod.LocalizedText.Adjectives.Count - 1);
            string adjective = Mod.LocalizedText.Adjectives[adjIdx];
            int nounIdx = Mod.Random.Next(Mod.LocalizedText.Nouns.Count - 1);
            string noun = Mod.LocalizedText.Adjectives[nounIdx];

            string name = new Text(Mod.LocalizedText.ContractNameFormat, new object[] { prefix, adjective, noun }).ToString();
            Mod.Log.Debug?.Write($"Generated name: {name} from adjective: {adjective} and noun: {noun}");

            return name;
        }

        public static string GenerateCodename(string employerFactionName)
        {
            Mod.Log.Info?.Write($"Generating codename for contract with employerName: {employerFactionName}");

            List<string> names = new List<string>(10);

            // See if there are any factional names to add
            int factionNameCount = (int)Math.Floor(Mod.Config.FactionNameWeight * 10);
            string prefix = Mod.LocalizedText.DefaultContractPrefix;
            if (!string.IsNullOrEmpty(employerFactionName))
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
                            string name = new Text(Mod.LocalizedText.ContractNameFormat, new object[] { prefix, "", fName}).ToString();
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

        public static string GetOrCreateCodename(Contract contract)
        {
            if (contract == null) return ModConsts.DefaultCodeName;

            string cacheKey = contract.SemiUniqueHash();
            string codename = ModConsts.DefaultCodeName;
            bool hasKey = ModState.ContractGUIDToCodeName.TryGetValue(cacheKey, out codename);
            if (!hasKey)
            {
                FactionValue employerFaction = contract.GetTeamFaction(ModConsts.EmployerFactionId);
                codename = GenerateCodename(employerFaction.Name);
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
