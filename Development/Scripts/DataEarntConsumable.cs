using System;
using System.IO;
using System.Collections.Generic;
using RoR2;
using UnityEngine;
using R2API;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class DataEarntConsumable : MonoBehaviour {
            static public float winMultiplier;
            static public float lossMutliplier;
            static public float obliterateMultiplier;
            static public float limboMultiplier;
            static public float easyMultiplier;
            static public float normalMultiplier;
            static public float hardMultiplier;

            static public float winMultiplierDefault = 3f;
            static public float lossMutliplierDefault = 1.5f;
            static public float obliterateMultiplierDefault = 2f;
            static public float limboMultiplierDefault = 2.5f;
            static public float easyMultiplierDefault = 2;
            static public float normalMultiplierDefault = 4;
            static public float hardMultiplierDefault = 8;

            static public int userPoints = -1;
            static public int userPointsBackup = -1;
            static public int userPointsRecent = 0;

            static public List<Dictionary<int, int>> itemsPurchased = new List<Dictionary<int, int>>();
            static public int mode = 0;

            static private string userPointsFile = "CreditsConsumable.txt";
            static private string itemsPurchasedFile = "ItemsEarntConsumable.txt";

            static public List<string> userPointsName = new List<string>() { "userPointsConsumable" };
            static public List<string> userPointsRecentName = new List<string>() { "userPointsConsumableRecent" };
            static public List<string> itemsPurchasedName = new List<string>() { "itemsPurchasedConsumable" };

            static public List<string> winMultiplierName = new List<string>() { "winMultiplierConsumable" };
            static public List<string> lossMultiplierName = new List<string>() { "lossMultiplierConsumable" };
            static public List<string> obliterateMultiplierName = new List<string>() { "obliterationMultiplierConsumable" };
            static public List<string> limboMultiplierName = new List<string>() { "limboMultiplierConsumable" };
            static public List<string> easyMultiplierName = new List<string>() { "easyMultiplierConsumable" };
            static public List<string> normalMultiplierName = new List<string>() { "normalMultiplierConsumable" };
            static public List<string> hardMultiplierName = new List<string>() { "hardMultiplierConsumable" };


            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            
            static public void RefreshInfo(Dictionary<string, string> configGlobal, Dictionary<string, string> configProfile) {
                GetConfig(configGlobal);
                userPointsBackup = Data.GetUserPoints(configProfile, userPointsName, userPointsFile);
                userPoints = userPointsBackup;
                userPointsRecent = Data.GetUserPoints(configProfile, userPointsRecentName, "null");
                userPoints = Data.GetItemsPurchased(configProfile, itemsPurchasedName, itemsPurchasedFile, itemsPurchased, userPoints, mode);
                VerifyItemsPurchased();
            }

            static void GetConfig(Dictionary<string, string> config) {
                winMultiplier = Data.ParseFloat(winMultiplierDefault, Util.GetConfig(config, winMultiplierName));
                lossMutliplier = Data.ParseFloat(lossMutliplierDefault, Util.GetConfig(config, lossMultiplierName));
                obliterateMultiplier = Data.ParseFloat(obliterateMultiplierDefault, Util.GetConfig(config, obliterateMultiplierName));
                limboMultiplier = Data.ParseFloat(limboMultiplierDefault, Util.GetConfig(config, limboMultiplierName));
                easyMultiplier = Data.ParseFloat(easyMultiplierDefault, Util.GetConfig(config, easyMultiplierName));
                normalMultiplier = Data.ParseFloat(normalMultiplierDefault, Util.GetConfig(config, normalMultiplierName));
                hardMultiplier = Data.ParseFloat(hardMultiplierDefault, Util.GetConfig(config, hardMultiplierName));
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
                foreach (int itemID in itemsToRemove) {
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


            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


            static float GetDifficultyMultiplier(Run run) {
                List<float> functionValues = Util.GetDifficultyParabola(easyMultiplier, normalMultiplier, hardMultiplier);
                float scalingValue = DifficultyCatalog.GetDifficultyDef(run.selectedDifficulty).scalingValue;
                scalingValue += Data.GetEclipseScalingValueAdd(run);
                return Mathf.Max(functionValues[4], Mathf.Min(functionValues[3], functionValues[0] * Mathf.Pow(scalingValue, 2) + functionValues[1] * scalingValue + functionValues[2]));
            }

            static public void UpdateUserPointsStages(Run run, RunReport runReport) {
                if (Data.earningMethod == 0 || Data.earningMethod == 2) {
                    float pointsMultiplier = 1;
                    if (runReport.gameEnding.gameEndingIndex == RoR2.RoR2Content.GameEndings.mainEnding.gameEndingIndex) {
                        pointsMultiplier = pointsMultiplier * winMultiplier;
                    } else if (runReport.gameEnding.gameEndingIndex == RoR2.RoR2Content.GameEndings.standardLoss.gameEndingIndex) {
                        pointsMultiplier = pointsMultiplier * lossMutliplier;
                    } else if (runReport.gameEnding.gameEndingIndex == RoR2.RoR2Content.GameEndings.obliterationEnding.gameEndingIndex) {
                        pointsMultiplier = pointsMultiplier * obliterateMultiplier;
                    } else if (runReport.gameEnding.gameEndingIndex == RoR2.RoR2Content.GameEndings.limboEnding.gameEndingIndex) {
                        pointsMultiplier = pointsMultiplier * limboMultiplier;
                    }
                    pointsMultiplier = pointsMultiplier * GetDifficultyMultiplier(run);
                    if (Data.earningMethod == 0) {
                        pointsMultiplier = pointsMultiplier * run.stageClearCount;
                    }
                    foreach (string userID in Data.localUsers) {
                        Data.RefreshInfo(userID);
                        userPointsBackup += Mathf.FloorToInt(pointsMultiplier);
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
                            userPointsBackup += Mathf.FloorToInt(creditsEarned);
                            userPointsRecent += Mathf.FloorToInt(creditsEarned);
                            Data.SaveConfigProfile();
                        }
                    }
                }
            }

            static public void FinalizePurchases() {
                userPointsBackup = userPoints;
                itemsPurchased[Data.profile[Data.mode]].Clear();
                Data.SaveConfigProfile();
            }
        }
    }
}
