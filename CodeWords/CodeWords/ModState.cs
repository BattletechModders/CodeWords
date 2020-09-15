
using BattleTech;
using System.Collections.Generic;

namespace CodeWords {

    public static class ModState {

        public static Dictionary<string, string> ContractGUIDToCodeName = new Dictionary<string, string>();
        public static string NullGuidCodename = ModConsts.DefaultCodeName;

        public static string ActiveContractGUID = null;
        public static SimGameState SimGameState = null;

        public static void Reset() {
            // Reinitialize state
            ContractGUIDToCodeName.Clear();
            NullGuidCodename = ModConsts.DefaultCodeName;
            ActiveContractGUID = null;
        }
    }

}


