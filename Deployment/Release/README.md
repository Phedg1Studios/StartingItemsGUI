# Phedg1 Studios - Starting Items GUI

### TLDR ###
This mod will allow players to use a GUI to select which items will be given to them at the start of a run. There are 3 modes: Free, Earnt Persistent and Earnt Consumable. Works in both singleplayer and multiplayer (if the host and the client have the mod installed, enabled and are using the same mode. NOTE: Each client has to select their own items).

### INSPIRATION ###
This mod was inspired by features in other roguelike games such as Undermine, Dungreed and Dead Cells. The goal was to give a returning player a sense of progression, to give them a boost to make them more formidable the more they play.

## DESCRIPTION ##
The main menu will have a new button labelled "Starting Items". Clicking it will open up the shop interface. Every item and piece of equipment that has been unlocked (completed the requisite achievement) and discovered (been picked up at least once) will be listed here, alongside how many times it has been purchased. To purchase an item or piece of equipment, left click it. The same item can be purchased multiple times. To sell an item or piece of equipment, right click it. Three buttons, labelled "Earnt Consumable", "Earnt Persistent" and "Free", are displayed on the bottom right of the interface. Clicking these will allow the player to change the mods mode. The differences of these three modes are detailed below. Another three buttons, labelled "Profile: 1", "Profile: 2" and "Profile: 3", are displayed in the bottom centre of the interface. Clicking these will allow the player to change the current item loadout. Buying an item in one profile does not affect the others, this is so a player could configure each profile differently to easily switch between them between games. Each of the 3 profiles is unique to each mod mode. This mod can be enabled and disabled from this menu, should the player wish to play normally again. When a game begins the items from the currently selected mod mode and profile will be spawned into the players inventory.

### MODES ###

** Mode: Earnt Consumable **
Items in the shop interface will have a price associated with them. Buying an item will cost credits, selling an item will refund the full amount of credits. When a game is played while this mode is active the credits that were spent on items will be used up and consumed. The current profile will also have its items removed, they will have to be purchased again with the players remaining credits if they want to be spawned again in subsequent games.

** Mode: Earnt Persistent **
Items in the shop interface will have a price associated with them. Buying an item will cost credits, selling an item will refund the full amount of credits. When a game is played while this mode is active the credits that were spent on items will NOT be used up and consumed. The purchases will persist between games, as a kind of permanent upgrade to the players character.

** Mode: Free **
Items in the shop interface will have no price, they can be purchased as many times as the player wishes. The purchases will persist between games and will not be reset.

### CREDIT EARNING METHODS ###
Credits will be earnt regardless of the current mode of the mod. 

** Boss Mode: **
Players will earn credits for defeating endgame bosses. The amount of credits awarded per boss kill will vary based on the game's difficulty. Supported bosses in this mode are the: Lunar Scavanger and Mithrix.

** Stage Mode: **
Players will earn credits for every stage they have cleared. The amount of credits awarded per stage cleared will vary based on the game's difficulty and how it ended (win, loss, obliteration, limbo).

## CONFIGURATION ##
After a player has opened the item shop interface for the first time a number of configuration files will be created. These can be found at "<RISK OF RAIN 2 INSTALL LOCATION>/Risk of Rain 2/BepInEx/config/Phedg1 Studios/Starting Items GUI/. If you desire to alter the configuration of the mod it must be done by editing these files. The intention is that this would only be necessary once, with the rest of the player's interactions being done through the shop interface inside the game itself.

** Config.cfg **
This file contains all of the settings which persist between profiles. It stores the whether the mod is currently enabled or disabled, the mode the mod is currently using and whether all items (including items which have not been unlocked) should be listed. Prices for all 8 tiers of items (common, uncommon, rare, boss, lunar, equipment, lunar equipment and elite equipment) are read from this file. The amount of credits awarded for killing boss monsters are here. The 7 credit reward multipliers for the Earnt Persistent mode (default, win, loss, obliteration, easy, normal and hard) are here also, as well as the 6 credit reward multipliers for the Earnt Consumable mode (win, loss, obliteration, easy, normal and hard).

** <PROFILE ID>.txt **
This is the record of what items and equipment the player has chosen to use in the different modes and profiles, as well as any currency that has been earned and a few internal varialbes to improve the user experience, so that they can persist after the game has been closed. 

## Changelog ##
** v1.1.13 **
* Now compatible with the 1.0.1 update.
* Updated readme formatting.

** v1.1.12 **
* Fixed profiles 1 and 2 getting cleared.

** v1.1.11 **
* Fixed equipment price being set as elite equipment price.

** v1.1.10 **
* Fixed consumable credits not being awarded when killing Mithrix.
* Improved controller support.
* Added new flavor text panel detailing the earnt mode functions.
* Added new welcome screen displaying credits earned since last entering the menu.
* Streamlined the config which is not meant to be edited.
* Updated readme formatting.

** v1.1.9 **
* Fixed crash when reentering menu.

** v1.1.8 **
* Now compatible with the 1.0 update.
* Added credits multiplier for limbo ending.
* Added button to add/remove 1/10/100 items at a time.
* Added controller support.
* Increased required version of BepInEx and R2API.
* Removed dependency on MiniRpcLib.

** v1.1.7 **
* Fixed mod using consumable mode at first each time the game is launched.

** v1.1.6 **
* Added support for different starting item configs per player when using the FixedSplitscreen mod.
* Fixed mod disabled text not being greyed out when reentering the menu.
* Fixed corrupted font glitch.

** v1.1.5 **
* Added new credit earning method.
* Internal broken items should not be listed in the Item Drop List menu any longer.
* Changed how equipment id's are stored to try and mirror the source code in an effort to somewhat future proof.
* Changed how persistent credits were stored to match consumable credits.
* Prevented this gui from making other menus uninteractable any more (again).

** v1.1.4 **
* Increased required version of R2API so that custom items are now supported.
* Replaced some hooks with events. This fixes starting items not being given to multiplayer clients in some situations, as previously other mods were crashing this mods initialization thread on clients.
* Added version parameter to the config so changes to config files can be applied to existing config files.
* Added options to config to disable and hide any of the purchasing modes.
* Moved config for giving a fixed total of credits in the EarntPersistent mode into the main config file.
* Modified config descriptions so they show up cleaner in the mod manager.
* Prevented this gui from making other menus uninteractable any more.

** v1.1.3 **
* Changed configuration files and extensions to better support r2modman.

** v1.1.2 **
* Fixed GUI scaling for different resolutions and aspect ratios

** v1.1.1 **
* Added a new config file to adjust / augment the amount of credits a player has in the Earnt Persistent mode.

** v1.1.0 **
* Now compatible with the Artifacts update.
* Updated the visual style to match the new UI.
* Increased the scrolling speed of the starting items menu.
* Added support for additional difficulties added by other mods, so credits are now earned while playing them.
* Fixed a bug where items could not be removed when in the Free mode.
* Fixed a bug where unfinished items would show when show all items was enabled.
* Decoupled the mod from the base game to a much greater extent.

** v1.0.0 **
* Initial release.