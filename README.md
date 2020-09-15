# CodeWords 
This mod for the [HBS BattleTech](http://battletechgame.com/) game randomizes the displayed contract name for non-flashpoint contracts. These should be displayed on the Contracts Screen, as well on travel contracts. The mod has almost no localization settings, but does allow extensive customization of the contract names.

Within the `mod_localization.json` file, you can provide faction-specific, contract-specific, or default names that will be used when randomizing the pool. The priority of names are:

* Faction-specific name pools (`FactionNames`) are used if available
* Contract-type name pools (`DefaultNames`) are used if faction-name pools aren't available
* Finally the fallback name pool (`FallbackNames`) will be used

The pools are not additive; they are distinct. 