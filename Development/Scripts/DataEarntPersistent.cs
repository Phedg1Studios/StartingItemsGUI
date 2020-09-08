using System;
using System.IO;
using System.Collections.Generic;
using RoR2;
using UnityEngine;
using R2API;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class DataEarntPersistent : MonoBehaviour {
            static public float defaultMultiplier;
            static public float defaultResultMultiplier;
            static public float winMultiplier;
            static public float lossMutliplier;
            static public float obliterateMultiplier;
            static public float limboMultiplier;
            static public float easyMultiplier;
            static public float normalMultiplier;
            static public float hardMultiplier;
            static public bool pastPlay;

            static public float defaultMultiplierDefault = 1;
            static public float defaultResultMultiplierDefault = 4;
            static public float winMultiplierDefault = 2.5f;
            static public float lossMutliplierDefault = 1;
            static public float obliterateMultiplierDefault = 1.5f;
            static public float limboMultiplierDefault = 2;
            static public float easyMultiplierDefault = 1;
            static public float normalMultiplierDefault = 2;
            static public float hardMultiplierDefault = 4;
            static public int userPointsLockedDefault = -1;
            static public bool pastPlayDefault = false;

            static public int userPoints = -1;
            static public int userPointsBackup = -1;
            static public int userPointsRecent = 0;
            static public int userPointsLocked;
            
            static public int userPointsEarnt = -1;
            static public List<Dictionary<int, int>> itemsPurchased = new List<Dictionary<int, int>>();
            static public int mode = 1;

            static private string userPointsFile = "CreditsPersistent.txt";
            static private string itemsPurchasedFile = "ItemsEarntPersistent.txt";

            static private float totalStages = 0;
            static private float easyWinStages = 0;
            static private float normalWinStages = 0;
            static private float hardWinStages = 0;
            static private float easyLossStages = 0;
            static private float normalLossStages = 0;
            static private float hardLossStages = 0;
            static private float easyObliterateStages = 0;
            static private float normalObliterateStages = 0;
            static private float hardObliterateStages = 0;

            static private int userPointsAdjust = 0;

            static private string userPointsAdjustFile = "CreditsPersistentAdjust.txt";
            static private string userStagesFile = "UserStages.txt";

            static public List<string> userPointsName = new List<string>() { "userPointsPersistent" };
            static public List<string> userPointsRecentName = new List<string>() { "userPointsPersistentRecent" };
            static public List<string> itemsPurchasedName = new List<string>() { "itemsPurchasedPersistent" };

            static public List<string> defaultMultiplierName = new List<string>() { "defaultStagesMultiplier", "defaultMultiplierPersistent" };
            static public List<string> defaultResultMultiplierName = new List<string>() { "defaultResultMultiplier" };
            static public List<string> winMultiplierName = new List<string>() { "winMultiplierPersistent" };
            static public List<string> lossMultiplierName = new List<string>() { "lossMultiplierPersistent" };
            static public List<string> obliterateMultiplierName = new List<string>() { "obliterationMultiplierPersistent" };
            static public List<string> limboMultiplierName = new List<string>() { "limboMultiplierPersistent" };
            static public List<string> easyMultiplierName = new List<string>() { "easyMultiplierPersistent" };
            static public List<string> normalMultiplierName = new List<string>() { "hardMultiplierPersistent" };
            static public List<string> hardMultiplierName = new List<string>() { "itemsPurchasedPersistent" };
            static public List<string> pointsLockedName = new List<string>() { "earntPersistentCreditsLocked" };
            static public List<string> pastPlayName = new List<string>() { "pastPlay" };


            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


            static public void RefreshInfo(Dictionary<string, string> configGlobal, Dictionary<string, string> configProfile) {
                GetUserPointsNegate();
                GetUserStages();
                GetConfig(configGlobal);
                CalculatePoints();
                GetUserPoints(configProfile);
                userPointsRecent = Data.GetUserPoints(configProfile, userPointsRecentName, "null");
                CalculatePoints2();
                userPoints = Data.GetItemsPurchased(configProfile, itemsPurchasedName, itemsPurchasedFile, itemsPurchased, userPoints, mode);
                VerifyItemsPurchased();
            }

            static void GetConfig(Dictionary<string, string> config) {
                defaultMultiplier = Data.ParseFloat(defaultMultiplierDefault, Util.GetConfig(config, defaultMultiplierName));
                defaultResultMultiplier = Data.ParseFloat(defaultResultMultiplierDefault, Util.GetConfig(config, defaultResultMultiplierName));
                winMultiplier = Data.ParseFloat(winMultiplierDefault, Util.GetConfig(config, winMultiplierName));
                lossMutliplier = Data.ParseFloat(lossMutliplierDefault, Util.GetConfig(config, lossMultiplierName));
                obliterateMultiplier = Data.ParseFloat(obliterateMultiplierDefault, Util.GetConfig(config, obliterateMultiplierName));
                limboMultiplier = Data.ParseFloat(limboMultiplierDefault, Util.GetConfig(config, limboMultiplierName));
                easyMultiplier = Data.ParseFloat(easyMultiplierDefault, Util.GetConfig(config, easyMultiplierName));
                normalMultiplier = Data.ParseFloat(normalMultiplierDefault, Util.GetConfig(config, normalMultiplierName));
                hardMultiplier = Data.ParseFloat(hardMultiplierDefault, Util.GetConfig(config, hardMultiplierName));
                userPointsLocked = Data.ParseInt(userPointsLockedDefault, Util.GetConfig(config, pointsLockedName));
                pastPlay = Data.ParseBool(pastPlayDefault, Util.GetConfig(config, pastPlayName));
            }

            static void GetUserPointsNegate() {
                string userPointsAdjustPath = BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + Data.modFolder + "/" + Data.userProfile + "/" + userPointsAdjustFile;
                
                if (File.Exists(userPointsAdjustPath)) {
                    StreamReader reader = new StreamReader(userPointsAdjustPath);
                    string userPointsNegateString = reader.ReadToEnd();
                    reader.Close();
                    if (!string.IsNullOrEmpty(userPointsNegateString)) {
                        userPointsAdjust = Data.ParseInt(0, userPointsNegateString);
                    }
                }
            }

            static void GetUserStages() {
                totalStages = 0;
                if (Data.userProfile != "") {
                    int stagesCompleted = GetStat(RoR2.Stats.StatDef.totalStagesCompleted);
                    totalStages = Mathf.FloorToInt(stagesCompleted);
                }

                string userStagesPath = BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + Data.modFolder + "/" + Data.userProfile + "/" + userStagesFile;
                if (File.Exists(userStagesPath)) {
                    StreamReader reader = new StreamReader(userStagesPath);
                    string userStagesString = reader.ReadToEnd();
                    reader.Close();
                    if (!string.IsNullOrEmpty(userStagesString)) {
                        string[] splitString = userStagesString.Split(',');
                        if (splitString.Length == 9) {
                            List<int> newStages = new List<int>();
                            bool stagesParsed = true;
                            foreach (string price in splitString) {
                                int newStage = 0;
                                stagesParsed = int.TryParse(price, out newStage);
                                newStages.Add(Mathf.Max(0, newStage));
                                if (!stagesParsed) {
                                    break;
                                }
                            }
                            if (stagesParsed) {
                                easyWinStages = newStages[0];
                                easyLossStages = newStages[1];
                                easyObliterateStages = newStages[2];
                                normalWinStages = newStages[3];
                                normalLossStages = newStages[4];
                                normalObliterateStages = newStages[5];
                                hardWinStages = newStages[6];
                                hardLossStages = newStages[7];
                                hardObliterateStages = newStages[8];
                            }
                        }
                    }
                }
            }

            static void CalculatePoints() {
                float points = defaultMultiplier * totalStages;

                points += easyWinStages * (easyMultiplier * winMultiplier - defaultMultiplier);
                points += easyLossStages * (easyMultiplier * lossMutliplier - defaultMultiplier);
                points += easyObliterateStages * (easyMultiplier * obliterateMultiplier - defaultMultiplier);
                points += normalWinStages * (normalMultiplier * winMultiplier - defaultMultiplier);
                points += normalLossStages * (normalMultiplier * lossMutliplier - defaultMultiplier);
                points += normalObliterateStages * (normalMultiplier * obliterateMultiplier - defaultMultiplier);
                points += hardWinStages * (hardMultiplier * winMultiplier - defaultMultiplier);
                points += hardLossStages * (hardMultiplier * lossMutliplier - defaultMultiplier);
                points += hardObliterateStages * (hardMultiplier * obliterateMultiplier - defaultMultiplier);
                points += userPointsAdjust;

                userPointsEarnt = Mathf.FloorToInt(points);
            }

            static void GetUserPoints(Dictionary<string, string> config) {
                string line = Util.GetConfig(config, userPointsName);
                string userPointsPath = BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + Data.modFolder + "/" + Data.userProfile + "/" + userPointsFile;
                line = Util.MultilineToSingleLine(line, userPointsPath);

                userPointsEarnt = 0;
                if (!string.IsNullOrEmpty(line)) {
                    userPointsEarnt = Data.ParseInt(0, line);
                } else if (pastPlay) {
                    if (Data.earningMethod == 0) {
                        int stagesCompleted = GetStat(RoR2.Stats.StatDef.totalStagesCompleted);
                        userPointsEarnt = Mathf.FloorToInt(defaultMultiplier * Mathf.FloorToInt(stagesCompleted));
                    } else if (Data.earningMethod == 1) {
                        int lunarScavsKilled = 0;
                        lunarScavsKilled += GetStat(RoR2.Stats.PerBodyStatDef.killsAgainst.FindStatDef("ScavLunar1Body"));
                        lunarScavsKilled += GetStat(RoR2.Stats.PerBodyStatDef.killsAgainst.FindStatDef("ScavLunar2Body"));
                        lunarScavsKilled += GetStat(RoR2.Stats.PerBodyStatDef.killsAgainst.FindStatDef("ScavLunar3Body"));
                        lunarScavsKilled += GetStat(RoR2.Stats.PerBodyStatDef.killsAgainst.FindStatDef("ScavLunar4Body"));

                        userPointsEarnt = lunarScavsKilled * Data.lunarScavPoints;
                        int mithrixKilled = GetStat(RoR2.Stats.PerBodyStatDef.killsAgainst.FindStatDef("BrotherHurtBody"));
                        userPointsEarnt += mithrixKilled * Data.mithrixPoints;
                    } else if (Data.earningMethod == 2) {
                        int gamesPlayed = GetStat(RoR2.Stats.StatDef.totalGamesPlayed);
                        userPointsEarnt = Mathf.FloorToInt(gamesPlayed * defaultResultMultiplier);
                    }
                }
            }

            static void CalculatePoints2() {
                if (userPointsLocked < 0) {
                    userPointsBackup = userPointsEarnt;
                } else {
                    userPointsBackup = userPointsLocked;
                }
                userPoints = userPointsBackup;
            }

            static public void VerifyItemsPurchased() {
                userPoints = userPointsBackup;
                List<int> itemsToRemove = new List<int>();
                foreach (int itemID in itemsPurchased[Data.profile[mode]].Keys) {
                    if (userPoints >= Data.GetItemPrice(itemID) * itemsPurchased[Data.profile[mode]][itemID]) {
                        userPoints -= Data.GetItemPrice(itemID) * itemsPurchased[Data.profile[mode]][itemID];
                    } else {
                        itemsToRemove.Add(itemID);
                    }
                }
                foreach(int itemID in itemsToRemove) {
                    itemsPurchased[Data.profile[mode]].Remove(itemID);
                }
            }


            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


            static public void BuyItem(int itemID) {
                bool boughtItem = false;
                for (int transactionIndex = 0; transactionIndex < Data.buyMultiplier; transactionIndex++) {
                    if (userPoints >= Data.GetItemPrice(itemID)) {
                        if (!itemsPurchased[Data.profile[Data.mode]].ContainsKey(itemID)) {
                            itemsPurchased[Data.profile[Data.mode]].Add(itemID, 0);
                        }
                        itemsPurchased[Data.profile[Data.mode]][itemID] += 1;
                        userPoints -= Data.GetItemPrice(itemID);
                        boughtItem = true;
                    } else {
                        break;
                    }
                }
                if (boughtItem) {
                    Data.MakeDirectoryExist();
                    Data.SaveConfigProfile();
                    UIDrawer.Refresh();
                }
            }

            static public void SellItem(int itemID) {
                bool soldItem = false;
                for (int transactionIndex = 0; transactionIndex < Data.buyMultiplier; transactionIndex++) {
                    if (itemsPurchased[Data.profile[Data.mode]].ContainsKey(itemID)) {
                        itemsPurchased[Data.profile[Data.mode]][itemID] -= 1;
                        if (itemsPurchased[Data.profile[Data.mode]][itemID] == 0) {
                            itemsPurchased[Data.profile[Data.mode]].Remove(itemID);
                        }
                        userPoints += Data.GetItemPrice(itemID);
                        soldItem = true;
                    } else {
                        break;
                    }
                }
                if (soldItem) {
                    Data.MakeDirectoryExist();
                    Data.SaveConfigProfile();
                    UIDrawer.Refresh();
                }
            }

            static public void ClearRecentPoints() {
                if (Data.mode == mode) {
                    userPointsRecent = 0;
                }
            }

            static float GetDifficultyMultiplier(Run run) {
                List<float> functionValues = Util.GetDifficultyParabola(easyMultiplier, normalMultiplier, hardMultiplier);
                float scalingValue = DifficultyCatalog.GetDifficultyDef(run.selectedDifficulty).scalingValue;
                scalingValue += Data.GetEclipseScalingValueAdd(run);
                return Mathf.Max(functionValues[4], Mathf.Min(functionValues[3], functionValues[0] * Mathf.Pow(scalingValue, 2) + functionValues[1] * scalingValue + functionValues[2]));
            }

            static public void UpdateUserPointsStages(Run run, RunReport runReport) {
                if (Data.earningMethod == 0 || Data.earningMethod == 2) {
                    float pointsMultiplier = 1;
                    if (runReport.gameEnding.gameEndingIndex == RoR2Content.GameEndings.mainEnding.gameEndingIndex) {
                        pointsMultiplier = pointsMultiplier * winMultiplier;
                    } else if (runReport.gameEnding.gameEndingIndex == RoR2Content.GameEndings.standardLoss.gameEndingIndex) {
                        pointsMultiplier = pointsMultiplier * lossMutliplier;
                    } else if (runReport.gameEnding.gameEndingIndex == RoR2Content.GameEndings.obliterationEnding.gameEndingIndex) {
                        pointsMultiplier = pointsMultiplier * obliterateMultiplier;
                    } else if (runReport.gameEnding.gameEndingIndex == RoR2Content.GameEndings.limboEnding.gameEndingIndex) {
                        pointsMultiplier = pointsMultiplier * limboMultiplier;
                    }
                    pointsMultiplier = pointsMultiplier * GetDifficultyMultiplier(run);
                    if (Data.earningMethod == 0) {
                        pointsMultiplier = pointsMultiplier * run.stageClearCount;
                    }
                    foreach (string userID in Data.localUsers) {
                        Data.RefreshInfo(userID);
                        userPointsEarnt += Mathf.FloorToInt(pointsMultiplier);
                        userPointsRecent += Mathf.FloorToInt(pointsMultiplier);
                        Data.SaveConfigProfile();
                    }
                }
            }

            static public void UpdateUserPointsBoss(string givenName) {
                if (Data.earningMethod == 1) {
                    if (givenName.Contains("ScavLunar") || givenName.Contains("BrotherHurt")) {
                        float creditsEarned = GetDifficultyMultiplier(Run.instance);
                        if (givenName.Contains("ScavLunar")) {
                            creditsEarned = creditsEarned * Data.lunarScavPoints;
                        } else if (givenName.Contains("BrotherHurt")) {
                            creditsEarned = creditsEarned * Data.lunarScavPoints;
                        }
                        foreach (string userID in Data.localUsers) {
                            Data.RefreshInfo(userID);
                            userPointsEarnt += Mathf.FloorToInt(creditsEarned);
                            userPointsRecent += Mathf.FloorToInt(creditsEarned);
                            Data.SaveConfigProfile();
                        }
                    }
                }
            }

            static private int GetStat(RoR2.Stats.StatDef givenStatDef) {
                UInt64 statValue = RoR2.UserProfile.GetProfile(Data.userProfile).statSheet.GetStatValueULong(givenStatDef);
                int statValueAdjusted;
                if (statValue <= System.Int32.MaxValue) {
                    statValueAdjusted = System.Convert.ToInt32(statValue);
                } else {
                    statValueAdjusted = System.Int32.MaxValue;
                }
                return statValueAdjusted;
            }
        }
    }
}
