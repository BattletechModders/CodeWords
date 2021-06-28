# CodeWords 

This mod for the [HBS BattleTech](http://battletechgame.com/) game provides a randomized two-word name for standards contracts, reminiscent of the approach XCOM:Enemy Unknown took. Thus instead of **Attack & Defend** you instead will encounter **Angry Zebra** or **Bravo Foxtrot**. Additionally faction-specific strings can be used for contract names. This allows you to give Davion patriotic names like **Stalwart Shield** or **Eternal Crusade of Justice** if you so choose.

While the mod includes a complex dictionary of random names, courtesy of [Roguetech](http://github.com/battletechmodders/roguetech). Because RogueTech has many more factions that most mods, you likely will want to eliminate some of the provided faction-specific names.

## General Configuration

This mod has a limited number of configuration options beyond the names that are to be used. These are all configured in the `mod.json` file:

| Parameter | Description | Default |
| -- | -- | -- |
| FactionNameWeight | The percentage chance a faction-specific name will be used in-lieu of a randomized one. Keep this relatively low, unless you have have a very large corpus of faction-specific names that can be used. | 0.2 |
| ExcludeContractsWithId | Contracts with an ID listed in this file will never have their name replaced |  "itrom_attack", "itrom_defense", "panzyr_attack", "panzr_defense", "smithon_attack", "tyrlon_attack", "story_1a_retreat", "story_1b_retreat", "story_2_threeyearslater", "story_3_axylus", "story_4_liberationofweldry", "story_5_servedcold", "story_6a_treasuretrove", "story_6b_treasuretrove", "story_7_gunboatdiplomacy", "story_8_locura", "story_9_downfall" |

## Name Configuration

Contract names are configured in the `mod_localized_text.json` file in the mod directory. You can change the way the mod displays contract names and the names it will use through the parameters of this file. These configuration options are listed below.

| Parameter | Description | Default |
| -- | -- | -- |
| ContractNameFormat | A C# string format specification that will be used to format the name. It must contain three elements, {0} = prefix, {1} = adjective, {2} = noun. You can freely modify this string, but it must contain these three data values elements. The default generates a string formatted like so: `Operation: Fuzzy Bear` | {0}: {1} {2} |
| DefaultContractPrefix | The name that will be used for the prefix of the contract name. | Operation |
| Adjectives | The list of adjectives that will be used for standard contract name. | See `mod_localized_text.json` |
| Nouns | The list of nouns that will be used for standard contract names. | See `mod_localized_text.json` |
| FactionNames | A dictionary linking factionIDs to faction-specific configurations. Elements in this dictionary must be a Javascript object with the FactionID.ContractPrefix and FactionID.ContractNames element (see below) | See `mod_localized_text.json` |
| *FactionID.ContractPrefix* | The prefix that will be used for *ALL* contracts from this faction. | Operation |
| *FactionID.ContractNames* | The list of faction-specific contract names that will be used when a randomly rolled value is less than *FactionNameWeight* for a given contract (see above) | See `mod_localized_text.json` |

