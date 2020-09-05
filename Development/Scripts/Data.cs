using System;
using System.IO;
using System.Collections.Generic;
using RoR2;
using UnityEngine;
using R2API;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class Data : MonoBehaviour {
            static public Dictionary<int, ItemIndex> allItemIDs = new Dictionary<int, ItemIndex>();
            static public Dictionary<ItemIndex, int> allItemsIndexes = new Dictionary<ItemIndex, int>();
            static public List<ItemIndex> bossItems = new List<ItemIndex>();
            static public Dictionary<int, EquipmentIndex> allEquipmentIDs = new Dictionary<int, EquipmentIndex>();
            static public Dictionary<EquipmentIndex, int> allEquipmentIndexes = new Dictionary<EquipmentIndex, int>();
            static public List<EquipmentIndex> lunarEquipment = new List<EquipmentIndex>();
            static public List<EquipmentIndex> eliteEquipment = new List<EquipmentIndex>();
            static private List<RoR2.ItemIndex> badItems = new List<ItemIndex>() {
            };

            static private int tier1Price;
            static private int tier2Price;
            static private int tier3Price;
            static private int bossPrice;
            static private int lunarPrice;
            static private int equipmentPrice;
            static private int lunarEquipmentPrice;
            static private int eliteEquipmentPrice;

            static public int lunarScavPoints;
            static public int mithrixPoints;

            static private int tier1PriceDefault = 40;
            static private int tier2PriceDefault = 100;
            static private int tier3PriceDefault = 400;
            static private int bossPriceDefault = 550;
            static private int lunarPriceDefault = 750;
            static private int equipmentPriceDefault = 350;
            static private int lunarEquipmentPriceDefault = 750;
            static private int eliteEquipmentPriceDefault = 1000;

            static public int lunarScavPointsDefault = 40;
            static public int mithrixPointsDefault = 60;

            static public string userProfile = "";
            static public string developerName = "Phedg1 Studios";
            static public string modName = "Starting Items GUI";
            static public string modFolder;
            static public string configFile = "Config.cfg";
            static public string configProfileFile = ".txt";
            static private string profilesFile = "Profiles.txt";


            static public bool modEnabledDefault = true;
            static private bool showAllItemsDefault = false;
            static public List<bool> modesEnabledDefault = new List<bool>() { true, true, true };
            static public int modeDefault = 0;
            static public bool earningMethodDefault = true;

            static public readonly int configVersion = 4;
            static public int configFileVersion = -1;
            static public bool modEnabled;
            static private bool showAllItems;
            static public int mode;
            static public List<bool> modesEnabled = new List<bool>() { true, true, true };
            static public List<int> profile = new List<int>() { 0, 0, 0 };
            static public int profileCount = 3;
            static public int modeCount = 3;
            static public List<string> modeNames = new List<string>() {
                "Earnt Consumable",
                "Earnt Persistent",
                "Free",
            };
            static public bool earningMethod;
            static public char profileChar = '/';
            static public char variableChar = '=';
            static public char splitChar = ',';
            static public char dictChar = ':';
            static private int forcedMode = -1;
            static public int buyMultiplier = 1;
            static public int buyMultiplierMax = 1000;

            static public List<string> configVersionName = new List<string>() { "configVersion" };
            static public List<string> enabledName = new List<string>() { "enabled" };
            static public List<string> earntConsumableName = new List<string>() { "earntConsumable" };
            static public List<string> earntPersistentName = new List<string>() { "earntPersistent" };
            static public List<string> freeName = new List<string>() { "freePersistent" };
            static public List<string> showAllName = new List<string>() { "showAllItems" };
            static public List<string> modeName = new List<string>() { "mode" };
            static public List<string> earningMethodName = new List<string>() { "earningMethod" };
            static public List<string> lunarScavCreditsName = new List<string>() { "lunarScavCredits" };
            static public List<string> mithrixCreditsName = new List<string>() { "mithrixCredits" };
            static public List<string> tier1PriceName = new List<string>() { "tier1Price" };
            static public List<string> tier2PriceName = new List<string>() { "tier2Price" };
            static public List<string> tier3PriceName = new List<string>() { "tier3Price" };
            static public List<string> tierBossPriceName = new List<string>() { "bossPrice" };
            static public List<string> tierLunarPriceName = new List<string>() { "lunarPrice" };
            static public List<string> tierEquipmentPriceName = new List<string>() { "equipmentPrice" };
            static public List<string> tierLunarEquipmentPriceName = new List<string>() { "lunarEquipmentPrice" };
            static public List<string> tierEliteEquipmentPriceName = new List<string>() { "eliteEquipmentPrice" };

            static public List<string> profilesName = new List<string>() { "profiles" };

            static public List<string> localUsers = new List<string>();

            static public void PopulateItemCatalogues() {
                int index = 0;
                foreach (ItemIndex itemIndex in RoR2.ItemCatalog.allItems) {
                    allItemIDs.Add(index, itemIndex);
                    allItemsIndexes.Add(itemIndex, index);

                    if (!RoR2.ItemCatalog.tier1ItemList.Contains(itemIndex) &&
                        !RoR2.ItemCatalog.tier2ItemList.Contains(itemIndex) &&
                        !RoR2.ItemCatalog.tier3ItemList.Contains(itemIndex) &&
                        !RoR2.ItemCatalog.lunarItemList.Contains(itemIndex)) {
                        bossItems.Add(itemIndex);
                    }
                    index += 1;
                }
                index = 1000;
                foreach (EquipmentIndex equipmentIndex in RoR2.EquipmentCatalog.allEquipment) {
                    allEquipmentIDs.Add(index, equipmentIndex);
                    allEquipmentIndexes.Add(equipmentIndex, index);

                    if (!RoR2.EquipmentCatalog.equipmentList.Contains(equipmentIndex)) {
                        eliteEquipment.Add(equipmentIndex);
                    } else if (RoR2.EquipmentCatalog.GetEquipmentDef(equipmentIndex).isLunar) {
                        lunarEquipment.Add(equipmentIndex);
                    }
                    index += 1;
                }
            }

            static public void MakeDirectoryExist() {
                if (!Directory.Exists(BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + modFolder)) {
                    Directory.CreateDirectory(BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + modFolder);
                }
            }


            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


            static public void RefreshInfo(string givenProfileID = "") {
                modFolder = developerName + "/" + modName;
                GetUserProfileID(givenProfileID);
                MakeDirectoryExist();
                Dictionary<string, string> configGlobal = ReadConfig(BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + modFolder + "/" + configFile);
                GetConfig(configGlobal);
                Dictionary<string, string> configProfile = ReadConfig(BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + modFolder + "/" + userProfile + configProfileFile);
                GetProfiles(configProfile);
                DataEarntConsumable.RefreshInfo(configGlobal, configProfile);
                DataEarntPersistent.RefreshInfo(configGlobal, configProfile);
                DataFree.RefreshInfo(configGlobal, configProfile);
                CorrectConfig();
                SaveConfigProfile();
                DeleteOldConfig();
            }

            static void DeleteOldConfig() {
                string oldPath = BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + Data.modFolder + "/" + Data.userProfile;
                if (Directory.Exists(oldPath)) {
                    Directory.Delete(BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + Data.modFolder + "/" + Data.userProfile, true);
                }
            }

            static void GetUserProfileID(string givenProfileID = "") {
                if (givenProfileID == "") {
                    List<string> profileIDs = RoR2.UserProfile.GetAvailableProfileNames();
                    foreach (string userID in profileIDs) {
                        if (RoR2.UserProfile.GetProfile(userID) != null) {
                            if (RoR2.UserProfile.GetProfile(userID).loggedIn) {
                                userProfile = userID;
                            }
                        }
                    }
                } else {
                    userProfile = givenProfileID;
                }
            }

            static Dictionary<string, string> ReadConfig(string givenPath) {
                Dictionary<string, string> config = new Dictionary<string, string>();
                if (File.Exists(givenPath)) {
                    List<string> lines = new List<string>();
                    StreamReader reader = new StreamReader(givenPath);
                    while (reader.Peek() >= 0) {
                        lines.Add(reader.ReadLine());
                    }
                    reader.Close();
                    foreach (string lineRaw in lines) {
                        string line = lineRaw;
                        if ((line.Length >= 4 && line.Substring(0, 4) == "### ")) {
                            line = line.Substring(4, line.Length - 4);
                        }
                        if (!string.IsNullOrEmpty(line) && !new List<string>() { "#", " " }.Contains(line.Substring(0, 1))) {
                            string[] splitLine = line.Split(variableChar);
                            if (splitLine.Length == 2) {
                                for (int splitIndex = 0; splitIndex < splitLine.Length; splitIndex++) {
                                    splitLine[splitIndex] = splitLine[splitIndex].Replace(" ", "");
                                }
                                if (!config.ContainsKey(splitLine[0])) {
                                    config.Add(splitLine[0], splitLine[1]);
                                }
                            }
                        }
                    }
                }
                return config;
            }

            static void GetConfig(Dictionary<string, string> config) {
                configFileVersion = ParseInt(-1, Util.GetConfig(config, configVersionName));
                modEnabled = ParseBool(modEnabledDefault, Util.GetConfig(config, enabledName));
                modesEnabled[0] = ParseBool(modesEnabledDefault[0], Util.GetConfig(config, earntConsumableName));
                modesEnabled[1] = ParseBool(modesEnabledDefault[1], Util.GetConfig(config, earntPersistentName));
                modesEnabled[2] = ParseBool(modesEnabledDefault[2], Util.GetConfig(config, freeName));
                showAllItems = ParseBool(showAllItemsDefault, Util.GetConfig(config, showAllName));
                if (forcedMode == -1) {
                    mode = Mathf.Max(0, Mathf.Min(ParseInt(0, Util.GetConfig(config, modeName)), modeCount - 1));
                } else {
                    mode = forcedMode;
                }
                earningMethod = ParseBool(earningMethodDefault, Util.GetConfig(config, earningMethodName));
                lunarScavPoints = ParseInt(lunarScavPointsDefault, Util.GetConfig(config, lunarScavCreditsName));
                mithrixPoints = ParseInt(mithrixPointsDefault, Util.GetConfig(config, mithrixCreditsName));
                tier1Price = ParseInt(tier1PriceDefault, Util.GetConfig(config, tier1PriceName));
                tier2Price = ParseInt(tier2PriceDefault, Util.GetConfig(config, tier2PriceName));
                tier3Price = ParseInt(tier3PriceDefault, Util.GetConfig(config, tier3PriceName));
                bossPrice = ParseInt(bossPriceDefault, Util.GetConfig(config, tierBossPriceName));
                lunarPrice = ParseInt(lunarPriceDefault, Util.GetConfig(config, tierLunarPriceName));
                equipmentPrice = ParseInt(equipmentPriceDefault, Util.GetConfig(config, tierEquipmentPriceName));
                lunarEquipmentPrice = ParseInt(lunarEquipmentPriceDefault, Util.GetConfig(config, tierLunarEquipmentPriceName));
                eliteEquipmentPrice = ParseInt(eliteEquipmentPriceDefault, Util.GetConfig(config, tierEliteEquipmentPriceName));
            }

            static void CorrectConfig() {
                int firstEnabledMode = -1;
                for (int modeIndex = 0; modeIndex < modesEnabled.Count; modeIndex++) {
                    if (modesEnabled[modesEnabled.Count - 1 - modeIndex]) {
                        firstEnabledMode = modesEnabled.Count - 1 - modeIndex;
                    }
                }
                if (firstEnabledMode == -1) {
                    for (int modeIndex = 0; modeIndex < modesEnabled.Count; modeIndex++) {
                        modesEnabled[modeIndex] = true;
                    }
                } else if (!modesEnabled[mode]) {
                    mode = firstEnabledMode;
                    SaveConfig();
                }
                if (configFileVersion != configVersion || forcedMode != -1) {
                    SaveConfig();
                }
            }

            static public bool ParseBool(bool givenDefault, string givenString) {
                bool result = false;
                if (bool.TryParse(givenString, out result)) {
                    return result;
                }
                return givenDefault;
            }

            static public int ParseInt(int givenDefault, string givenString) {
                int result = 0;
                if (int.TryParse(givenString, out result)) {
                    return result;
                }
                return givenDefault;
            }

            static public float ParseFloat(float givenDefault, string givenString) {
                float result = 0;
                if (float.TryParse(givenString, out result)) {
                    return result;
                }
                return givenDefault;
            }

            static void GetProfiles(Dictionary<string, string> config) {
                string line = Util.GetConfig(config, profilesName);
                string profilesPath = BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + Data.modFolder + "/" + userProfile + "/" + profilesFile;
                line = Util.MultilineToSingleLine(line, profilesPath);
                profile.Clear();
                for (int profileIndex = 0; profileIndex < modeCount; profileIndex++) {
                    profile.Add(0);
                }
                string[] splitString = line.Split(splitChar);
                for (int profileIndex = 0; profileIndex < splitString.Length; profileIndex++) {
                    if (profileIndex < modeCount) {
                        int newProfile = 0;
                        if (int.TryParse(splitString[profileIndex], out newProfile)) {
                            if (newProfile >= 0 && newProfile < profileCount) {
                                profile[profileIndex] = newProfile;
                            }
                        }
                    }
                }
            }

            static public int GetUserPoints(Dictionary<string, string> config, List<string> configName, string fileName) {
                string line = Util.GetConfig(config, configName);
                string userPointsPath = fileName;
                if (fileName != "null") {
                    userPointsPath = BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + Data.modFolder + "/" + Data.userProfile + "/" + fileName;
                }
                line = Util.MultilineToSingleLine(line, userPointsPath);
                return Data.ParseInt(0, line);
            }

            static public int GetItemsPurchased(Dictionary<string, string> config, List<string> configName, string fileName, List<Dictionary<int, int>> dict, int points, int mode) {
                string line = Util.GetConfig(config, configName);
                string itemsPurchasedPath = BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + Data.modFolder + "/" + Data.userProfile + "/" + fileName;
                line = Util.MultilineToSingleLine(line, itemsPurchasedPath);
                dict.Clear();
                for (int profile = 0; profile < Data.profileCount; profile++) {
                    dict.Add(new Dictionary<int, int>());
                }
                
                int profileIndex = 0;
                string[] profiles = line.Split(Data.profileChar);
                foreach (string profile in profiles) {
                    if (profileIndex < dict.Count) {
                        string[] pairs = profile.Split(Data.splitChar);
                        foreach (string pair in pairs) {
                            if (!string.IsNullOrEmpty(pair)) {
                                string[] splitString = pair.Split(Data.dictChar);
                                if (splitString.Length == 2) {
                                    int itemID = 0;
                                    int itemCount = 0;
                                    bool itemIDParsed = int.TryParse(splitString[0], out itemID);
                                    bool itemCountParsed = int.TryParse(splitString[1], out itemCount);
                                    if (itemIDParsed && itemCountParsed) {
                                        if (ItemExists(itemID)) {
                                            if (UnlockedItem(itemID)) {
                                                if (!dict[profileIndex].ContainsKey(itemID)) {
                                                    if (profileIndex == Data.profile[mode] && (points < 0 || points >= Data.GetItemPrice(itemID) * itemCount)) {
                                                        dict[profileIndex].Add(itemID, itemCount);
                                                        points -= Data.GetItemPrice(itemID) * itemCount;
                                                    }
                                                    if (profileIndex != Data.profile[mode]) {
                                                        dict[profileIndex].Add(itemID, itemCount);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    profileIndex += 1;
                }
                return points;
            }

            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


            static public bool ItemExists(int itemID) {
                if (allItemIDs.ContainsKey(itemID)) {
                    if (RoR2.ItemCatalog.GetItemDef(allItemIDs[itemID]).pickupIconSprite != null && RoR2.ItemCatalog.GetItemDef(allItemIDs[itemID]).pickupIconSprite.name != "texNullIcon") {
                        if (RoR2.ItemCatalog.GetItemDef(allItemIDs[itemID]).tier != ItemTier.NoTier) {
                            if (!badItems.Contains(allItemIDs[itemID])) {
                                return true;
                            }
                        }
                    }
                } else if (allEquipmentIDs.ContainsKey(itemID)) {
                    if (RoR2.EquipmentCatalog.GetEquipmentDef(allEquipmentIDs[itemID]).pickupIconSprite != null && RoR2.EquipmentCatalog.GetEquipmentDef(allEquipmentIDs[itemID]).pickupIconSprite.name != "texNullIcon") {
                        return true;
                    }
                }
                return false;
            }

            static public bool UnlockedItem(int itemID) {
                if (ItemExists(itemID)) {
                    if (showAllItems) {
                        return true;
                    }
                    if (allItemIDs.ContainsKey(itemID)) {
                        if (RoR2.UserProfile.GetProfile(userProfile).HasUnlockable(RoR2.ItemCatalog.GetItemDef(allItemIDs[itemID]).unlockableName)) {
                            if (RoR2.UserProfile.GetProfile(userProfile).HasDiscoveredPickup(new PickupIndex(allItemIDs[itemID]))) {
                                return true;
                            }
                        }
                    } else if (allEquipmentIDs.ContainsKey(itemID)) {
                        if (RoR2.UserProfile.GetProfile(userProfile).HasUnlockable(RoR2.EquipmentCatalog.GetEquipmentDef(allEquipmentIDs[itemID]).unlockableName)) {
                            if (RoR2.UserProfile.GetProfile(userProfile).HasDiscoveredPickup(new PickupIndex(allEquipmentIDs[itemID]))) {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }

            static public int GetItemPrice(int itemID) {
                if (allItemIDs.ContainsKey(itemID)) {
                    if (RoR2.ItemCatalog.tier1ItemList.Contains(allItemIDs[itemID])) {
                        return tier1Price;
                    } else if (RoR2.ItemCatalog.tier2ItemList.Contains(allItemIDs[itemID])) {
                        return tier2Price;
                    } else if (RoR2.ItemCatalog.tier3ItemList.Contains(allItemIDs[itemID])) {
                        return tier3Price;
                    } else if (bossItems.Contains(allItemIDs[itemID])) {
                        return bossPrice;
                    } else if (RoR2.ItemCatalog.lunarItemList.Contains(allItemIDs[itemID])) {
                        return lunarPrice;
                    }
                } else if (allEquipmentIDs.ContainsKey(itemID)) {
                    if (lunarEquipment.Contains(allEquipmentIDs[itemID])) {
                        return lunarEquipmentPrice;
                    } else if (RoR2.EquipmentCatalog.equipmentList.Contains(allEquipmentIDs[itemID])) {
                        return equipmentPrice;
                    } else if (eliteEquipment.Contains(allEquipmentIDs[itemID])) {
                        return eliteEquipmentPrice;
                    }
                }
                return 1000;
            }

            static public int GetItemTier(int givenID) {
                if (allItemIDs.ContainsKey(givenID)) {
                    if (RoR2.ItemCatalog.tier1ItemList.Contains(allItemIDs[givenID])) {
                        return 0;
                    } else if (RoR2.ItemCatalog.tier2ItemList.Contains(allItemIDs[givenID])) {
                        return 1;
                    } else if (RoR2.ItemCatalog.tier3ItemList.Contains(allItemIDs[givenID])) {
                        return 2;
                    } else if (bossItems.Contains(allItemIDs[givenID])) {
                        return 3;
                    } else if (RoR2.ItemCatalog.lunarItemList.Contains(allItemIDs[givenID])) {
                        return 4;
                    }
                } else if (allEquipmentIDs.ContainsKey(givenID)) {
                    if (lunarEquipment.Contains(allEquipmentIDs[givenID])) {
                        return 4;
                    }
                    return 5;
                }
                return 5;
            }


            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


            static public void LeftClick(int itemID) {
                if (mode == DataEarntConsumable.mode) {
                    DataEarntConsumable.BuyItem(itemID);
                } else if (mode == DataEarntPersistent.mode) {
                    DataEarntPersistent.BuyItem(itemID);
                } else if (mode == DataFree.mode) {
                    DataFree.BuyItem(itemID);
                }
            }

            static public void RightClick(int itemID) {
                if (mode == DataEarntConsumable.mode) {
                    DataEarntConsumable.SellItem(itemID);
                } else if (mode == DataEarntPersistent.mode) {
                    DataEarntPersistent.SellItem(itemID);
                } else if (mode == DataFree.mode) {
                    DataFree.SellItem(itemID);
                }
            }


            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


            static public List<int> SortItems(List<int> givenItems) {
                List<int> sortedItems = new List<int>();
                foreach (ItemIndex itemIndex in RoR2.ItemCatalog.tier1ItemList) {
                    if (givenItems.Contains(allItemsIndexes[itemIndex])) {
                        sortedItems.Add(allItemsIndexes[itemIndex]);
                    }
                }
                foreach (ItemIndex itemIndex in RoR2.ItemCatalog.tier2ItemList) {
                    if (givenItems.Contains(allItemsIndexes[itemIndex])) {
                        sortedItems.Add(allItemsIndexes[itemIndex]);
                    }
                }
                foreach (ItemIndex itemIndex in RoR2.ItemCatalog.tier3ItemList) {
                    if (givenItems.Contains(allItemsIndexes[itemIndex])) {
                        sortedItems.Add(allItemsIndexes[itemIndex]);
                    }
                }
                foreach (ItemIndex itemIndex in bossItems) {
                    if (givenItems.Contains(allItemsIndexes[itemIndex])) {
                        sortedItems.Add(allItemsIndexes[itemIndex]);
                    }
                }
                foreach (ItemIndex itemIndex in RoR2.ItemCatalog.lunarItemList) {
                    if (givenItems.Contains(allItemsIndexes[itemIndex])) {
                        sortedItems.Add(allItemsIndexes[itemIndex]);
                    }
                }
                foreach (EquipmentIndex equipmentIndex in RoR2.EquipmentCatalog.equipmentList) {
                    if (givenItems.Contains(allEquipmentIndexes[equipmentIndex]) && !lunarEquipment.Contains(equipmentIndex)) {
                        sortedItems.Add(allEquipmentIndexes[equipmentIndex]);
                    }
                }
                foreach (EquipmentIndex equipmentIndex in lunarEquipment) {
                    if (givenItems.Contains(allEquipmentIndexes[equipmentIndex])) {
                        sortedItems.Add(allEquipmentIndexes[equipmentIndex]);
                    }
                }
                foreach (EquipmentIndex equipmentIndex in eliteEquipment) {
                    if (givenItems.Contains(allEquipmentIndexes[equipmentIndex])) {
                        sortedItems.Add(allEquipmentIndexes[equipmentIndex]);
                    }
                }
                return sortedItems;
            }

            static public void ToggleBuyMultiplier() {
                buyMultiplier = buyMultiplier * 10;
                if (buyMultiplier > buyMultiplierMax) {
                    buyMultiplier = 1;
                }
                UIDrawer.Refresh();
            }

            static public void ToggleEnabled() {
                modEnabled = !modEnabled;
                SaveConfig();
                UIDrawer.Refresh();
            }

            static public void SetForcedMode(int givenMode) {
                if (givenMode == -1 || forcedMode == -1) {
                    forcedMode = givenMode;
                }
            }

            static public void SetMode(int givenMode) {
                if (mode != givenMode) {
                    int oldMode = mode;
                    mode = givenMode;
                    SaveConfig();
                    VerifyData();
                    ChangeMenu(oldMode, mode);
                }
            }

            static public void SetProfile(int givenProfile) {
                profile[mode] = givenProfile;
                SaveConfigProfile();
                VerifyData();
                UIDrawer.Refresh();
            }

            static void VerifyData() {
                if (mode == 0) {
                    DataEarntConsumable.VerifyItemsPurchased();
                } else if (mode == 1) {
                    DataEarntPersistent.VerifyItemsPurchased();
                } else if (mode == 2) {
                    DataFree.VerifyItemsPurchased();
                }
            }

            static void ChangeMenu(int oldMode, int newMode) {
                if (UIConfig.storeRows[oldMode] == UIConfig.storeRows[newMode] && UIConfig.textCount[oldMode] == UIConfig.textCount[newMode]) {
                    UIDrawer.Refresh();
                } else {
                    UIDrawer.DrawUI();
                }
            }

            static void SaveConfig() {
                string spacing = " ";
                string variableCharUpdate = spacing + variableChar + spacing;
                string configPath = BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + modFolder + "/" + configFile;
                StreamWriter writer = new StreamWriter(configPath, false);
                string configString = "";
                configString += "### " + configVersionName[0] + " = " + configVersion.ToString();
                configString += "\n\n[General]";
                configString += "\n\n# Enable/disable the StartingItemsGUI mod\n#\n# Setting type: Boolean\n# Default value: " + modEnabledDefault.ToString() + "\n" + enabledName[0] + variableCharUpdate + modEnabled.ToString().ToLower();
                configString += "\n\n# Enable/disable the EarntConsumable mode\n#\n# Setting type: Boolean\n# Default value: " + modesEnabledDefault[DataEarntConsumable.mode].ToString() + "\n" + earntConsumableName[0] + variableCharUpdate + modesEnabled[0].ToString();
                configString += "\n\n# Enable/disable the EarntPersistent mode\n#\n# Setting type: Boolean\n# Default value: " + modesEnabledDefault[DataEarntPersistent.mode].ToString() + "\n" + earntPersistentName[0] + variableCharUpdate + modesEnabled[1].ToString();
                configString += "\n\n# Enable/disable the FreePersistentmode\n#\n# Setting type: Boolean\n# Default value: " + modesEnabledDefault[DataFree.mode].ToString() + "\n" + freeName[0] + variableCharUpdate + modesEnabled[2].ToString();
                configString += "\n\n# Enable/disable showing all items mod\n# When enabled all items and equipment will be listed, even those which the player has not unlocked and discovered\n#\n# Setting type: Boolean\n# Default value: " + showAllItemsDefault.ToString() + "\n" + showAllName[0] + variableCharUpdate + showAllItems.ToString().ToLower();
                configString += "\n\n# The mode currently in use\n# 0 is EarntConsumable, 1 is EarntPersistent and 2 is FreePersistent\n#\n# Setting type: Integer\n# Default value: " + modeDefault.ToString() + "\n" + modeName[0] + variableCharUpdate + mode.ToString();
                configString += "\n\n# Whether credits are earnt by killing endgame bosses (Lunar Scav only currently) or by clearing stages\n# True is bosses and False is stages\n#\n# Setting type: Boolean\n# Default value: " + earningMethodDefault.ToString() + "\n" + earningMethodName[0] + variableCharUpdate + earningMethod.ToString();
                configString += "\n\n[Prices]";
                configString += "\n\n# How many credits are awarded for killing a Lunar Scav\n#\n# Settings type: Integer\n# Default value: " + lunarScavPointsDefault.ToString() + "\n" + lunarScavCreditsName[0] + variableCharUpdate + lunarScavPoints.ToString();
                configString += "\n\n# How many credits are awarded for killing a Mithrix\n#\n# Settings type: Integer\n# Default value: " + mithrixPointsDefault.ToString() + "\n" + mithrixCreditsName[0] + variableCharUpdate + mithrixPoints.ToString();
                configString += "\n\n# How many credits a common item costs\n#\n# Setting type: Integer\n# Default value: " + tier1PriceDefault.ToString() + "\n" + tier1PriceName[0] + variableCharUpdate + tier1Price.ToString();
                configString += "\n\n# How many credits an uncommon item costs\n#\n# Setting type: Integer\n# Default value: " + tier2PriceDefault.ToString() + "\n" + tier2PriceName[0] + variableCharUpdate + tier2Price.ToString();
                configString += "\n\n# How many credits a rare item costs\n#\n# Setting type: Integer\n# Default value: " + tier3PriceDefault.ToString() + "\n" + tier3PriceName[0] + variableCharUpdate + tier3Price.ToString();
                configString += "\n\n# How many credits a boss item costs\n#\n# Setting type: Integer\n# Default value: " + bossPriceDefault.ToString() + "\n" + tierBossPriceName[0] + variableCharUpdate + bossPrice.ToString();
                configString += "\n\n# How many credits a lunar item costs\n#\n# Setting type: Integer\n# Default value: " + lunarPriceDefault.ToString() + "\n" + tierLunarPriceName[0] + variableCharUpdate + lunarPrice.ToString();
                configString += "\n\n# How many credits equipment costs\n#\n# Setting type: Integer\n# Default value: " + equipmentPriceDefault.ToString() + "\n" + tierEquipmentPriceName[0] + variableCharUpdate + equipmentPrice.ToString();
                configString += "\n\n# How many credits lunar equipment costs\n#\n# Setting type: Integer\n# Default value: " + lunarEquipmentPriceDefault.ToString() + "\n" + tierLunarEquipmentPriceName[0] + variableCharUpdate + lunarEquipmentPrice.ToString();
                configString += "\n\n# How many credits elite equipment costs\n#\n# Setting type: Integer\n# Default value: " + eliteEquipmentPriceDefault.ToString() + "\n" + tierEliteEquipmentPriceName[0] + variableCharUpdate + eliteEquipmentPrice.ToString();
                configString += "\n\n[Credit Multipliers EarntConsumable]";
                configString += "\n\n# The multiplier for how many EarntConsumable credits to award for a win\n#\n# Setting type: Float\n# Default value: " + DataEarntConsumable.winMultiplierDefault.ToString() + "\n" + DataEarntConsumable.winMultiplierName[0] + variableCharUpdate + DataEarntConsumable.winMultiplier.ToString();
                configString += "\n\n# The multiplier for how many EarntConsumable credits to award for a loss\n#\n# Setting type: Float\n# Default value: " + DataEarntConsumable.lossMultiplierName.ToString() + "\n" + DataEarntConsumable.lossMultiplierName[0] + variableCharUpdate + DataEarntConsumable.lossMutliplier.ToString();
                configString += "\n\n# The multiplier for how many EarntConsumable credits to award for an obliteration\n#\n# Setting type: Float\n# Default value: " + DataEarntConsumable.obliterateMultiplierDefault.ToString() + "\n" + DataEarntConsumable.obliterateMultiplierName[0] + variableCharUpdate + DataEarntConsumable.obliterateMultiplier.ToString();
                configString += "\n\n# The multiplier for how many EarntConsumable credits to award for limbo\n#\n# Setting type: Float\n# Default value: " + DataEarntConsumable.limboMultiplierDefault.ToString() + "\n" + DataEarntConsumable.limboMultiplierName[0] + variableCharUpdate + DataEarntConsumable.limboMultiplier.ToString();
                configString += "\n\n# The multiplier for how many EarntConsumable credits to award on easy\n#\n# Setting type: Float\n# Default value: " + DataEarntConsumable.easyMultiplierDefault.ToString() + "\n" + DataEarntConsumable.easyMultiplierName[0] + variableCharUpdate + DataEarntConsumable.easyMultiplier.ToString();
                configString += "\n\n# The multiplier for how many EarntConsumable credits to award on normal\n#\n# Setting type: Float\n# Default value: " + DataEarntConsumable.normalMultiplierDefault.ToString() + "\n" + DataEarntConsumable.normalMultiplierName[0] + variableCharUpdate + DataEarntConsumable.normalMultiplier.ToString();
                configString += "\n\n# The multiplier for how many EarntConsumable credits to award on hard\n#\n# Setting type: Float\n# Default value: " + DataEarntConsumable.hardMultiplierDefault.ToString() + "\n" + DataEarntConsumable.hardMultiplierName[0] + variableCharUpdate + DataEarntConsumable.hardMultiplier.ToString();
                configString += "\n\n[Credit Multipliers EarntPersistent]";
                configString += "\n\n# The multiplier for how many EarntPersistent credits to award for previously completed stages\n# Setting type: Float\n# Default value: " + DataEarntPersistent.defaultMultiplierDefault.ToString() + "\n" + DataEarntPersistent.defaultMultiplierName[0] + variableCharUpdate + DataEarntPersistent.defaultMultiplier.ToString();
                configString += "\n\n# The multiplier for how many EarntPersistent credits to award for a win\n#\n# Setting type: Float\n# Default value: " + DataEarntPersistent.winMultiplierDefault.ToString() + "\n" + DataEarntPersistent.winMultiplierName[0] + variableCharUpdate + DataEarntPersistent.winMultiplier.ToString();
                configString += "\n\n# The multiplier for how many EarntPersistent credits to award for a loss\n#\n# Setting type: Float\n# Default value: " + DataEarntPersistent.lossMutliplierDefault.ToString() + "\n" + DataEarntPersistent.lossMultiplierName[0] + variableCharUpdate + DataEarntPersistent.lossMutliplier.ToString();
                configString += "\n\n# The multiplier for how many EarntPersistent credits to award for an obliteration\n#\n# Setting type: Float\n# Default value: " + DataEarntPersistent.obliterateMultiplierDefault.ToString() + "\n" + DataEarntPersistent.obliterateMultiplierName[0] + variableCharUpdate + DataEarntPersistent.obliterateMultiplier.ToString();
                configString += "\n\n# The multiplier for how many EarntPersistent credits to award for limbo\n#\n# Setting type: Float\n# Default value: " + DataEarntPersistent.limboMultiplierDefault.ToString() + "\n" + DataEarntPersistent.limboMultiplierName[0] + variableCharUpdate + DataEarntPersistent.limboMultiplier.ToString();
                configString += "\n\n# The multiplier for how many EarntPersistent credits to award on easy\n#\n# Setting type: Float\n# Default value: " + DataEarntPersistent.easyMultiplierDefault.ToString() + "\n" + DataEarntPersistent.easyMultiplierName[0] + variableCharUpdate + DataEarntPersistent.easyMultiplier.ToString();
                configString += "\n\n# The multiplier for how many EarntPersistent credits to award on normal\n#\n# Setting type: Float\n# Default value: " + DataEarntPersistent.normalMultiplierDefault.ToString() + "\n" + DataEarntPersistent.normalMultiplierName[0] + variableCharUpdate + DataEarntPersistent.normalMultiplier.ToString();
                configString += "\n\n# The multiplier for how many EarntPersistent credits to award on hard\n#\n# Setting type: Float\n# Default value: " + DataEarntPersistent.hardMultiplierDefault.ToString() + "\n" + DataEarntPersistent.hardMultiplierName[0] + variableCharUpdate + DataEarntPersistent.hardMultiplier.ToString();
                configString += "\n\n# The amount of EarntPersistent credits available\n# The amount of EarntPersistent credits available will not grow as the player plays, it will stay locked to this amount\n# The amount of EarntPersistent credits a player has earned will continue to grow behind the scenes\n# If this value is smaller than 0 the EarntPersistent credits will function normally\n#\n# Setting type: Integer\n# Default value: " + DataEarntPersistent.userPointsLockedDefault.ToString() + "\n" + DataEarntPersistent.pointsLockedName[0] + variableCharUpdate + DataEarntPersistent.userPointsLocked.ToString();
                writer.Write(configString);
                writer.Close();
            }

            static public void SaveConfigProfile() {
                string spacing = " ";
                string variableCharUpdate = spacing + variableChar + spacing;
                string configPath = BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + modFolder + "/" + userProfile + configProfileFile;
                StreamWriter writer = new StreamWriter(configPath, false);
                string configString = "";
                configString += profilesName[0] + variableCharUpdate + Util.ListToString(profile);
                configString += "\n" + DataEarntConsumable.userPointsName[0] + variableCharUpdate + DataEarntConsumable.userPointsBackup;
                configString += "\n" + DataEarntConsumable.userPointsRecentName[0] + variableCharUpdate + DataEarntConsumable.userPointsRecent;
                configString += "\n" + DataEarntConsumable.itemsPurchasedName[0] + variableCharUpdate + Util.DictToString(DataEarntConsumable.itemsPurchased);
                configString += "\n" + DataEarntPersistent.userPointsName[0] + variableCharUpdate + DataEarntPersistent.userPointsEarnt;
                configString += "\n" + DataEarntPersistent.userPointsRecentName[0] + variableCharUpdate + DataEarntPersistent.userPointsRecent;
                configString += "\n" + DataEarntPersistent.itemsPurchasedName[0] + variableCharUpdate + Util.DictToString(DataEarntPersistent.itemsPurchased);
                configString += "\n" + DataFree.itemsPurchasedName[0] + variableCharUpdate + Util.DictToString(DataFree.itemsPurchased);
                writer.Write(configString);
                writer.Close();
            }
        }
    }
}

