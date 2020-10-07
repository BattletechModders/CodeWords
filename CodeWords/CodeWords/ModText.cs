using System.Collections.Generic;

namespace CodeWords
{
    public class ModText
    {
        public class FactionStrings
        {
            public string ContractPrefix = "Mission";
            public List<string> ContractNames = new List<string>();
        }

        public Dictionary<string, FactionStrings> FactionNames = new Dictionary<string, FactionStrings>()
        {
            {
                "AuriganPirates",
                new FactionStrings()
                    {
                        ContractPrefix = "Job:",
                        ContractNames = new List<string>()
                            {
                                "Jack 'Em Up", "Car Jamming", "Furious Road", "Bridge Troll", "Heads Rolling", "Teenage Lobotomy", "Shoulda Just Paid"
                            }
                    }
                }
        };

        public List<string> Adjectives = new List<string>()
        {
            "White", "Black", "Grey", "Red", "Green", "Blue", "Yellow", "Purple", "Orange",
            "Bright", "Dark", "Twilight", "Sunrise", "Sunset", "Dawn", "Dusk", "Noon", "Midnight",
        };

        public List<string> Nouns = new List<string>()
        {
            "Dog", "Cat", "Bird", "Fish", "Ferret", "Iguana", "Dragon", "Mouse", "Rat", "Tiger", "Lion", "Monkey"
        };

        // 0 = prefix, 1 = adjective, 2 = noun
        public string ContractNameFormat = "{0}: {1} {2}";

        public string DefaultContractPrefix = "Operation";

    }
}
