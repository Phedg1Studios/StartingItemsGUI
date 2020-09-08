using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using RoR2;
using UnityEngine;
using R2API;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class DataRandom : MonoBehaviour {
            static public int mode = 3;
            static public int pointsMethod;
            static public int pointsLocked;
            static private List<int> itemCountRange = new List<int>() { 10, 5, 2, 4, 3, 1 };

            static public int pointsMethodDefault = 0;
            static public int pointsLockedDefault = 1000;

            static public List<string> pointsMethodName = new List<string>() { "randomCreditsMethod" };
            static public List<string> pointsLockedName = new List<string>() { "randomCreditsLocked" };


            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


            static public void RefreshInfo(Dictionary<string, string> configGlobal, Dictionary<string, string> configProfile) {
                GetConfig(configGlobal);
            }

            static public void VerifyItemsPurchased() {

            }

            static void GetConfig(Dictionary<string, string> config) {
                pointsMethod = Data.ParseInt(pointsMethodDefault, Util.GetConfig(config, pointsMethodName));
                pointsLocked = Data.ParseInt(pointsLockedDefault, Util.GetConfig(config, pointsLockedName));
            }

            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


            static public void BuyItem(int itemID) {

            }

            static public void SellItem(int itemID) {

            }

            static public Dictionary<int, int> GenerateRandomItemList() {
                List<int> storeItems = UIDrawer.GetStoreItems();
                Dictionary<int, int> itemPrices = new Dictionary<int, int>();
                List<int> allPrices = new List<int>() {
                    Data.tier1Price,
                    Data.tier2Price,
                    Data.tier3Price,
                    Data.bossPrice,
                    Data.lunarPrice,
                    Data.equipmentPrice,
                    Data.lunarEquipmentPrice,
                    Data.eliteEquipmentPrice,
                };
                List<int> equipmentPrices = new List<int>() {
                    Data.equipmentPrice,
                    Data.lunarEquipmentPrice,
                    Data.eliteEquipmentPrice,
                };
                foreach (int itemID in storeItems) {
                    int itemPrice = Data.GetItemPrice(itemID);
                    itemPrices.Add(itemID, itemPrice);
                }
                int points = GetPoints();
                Dictionary<int, int> itemsPurchased = new Dictionary<int, int>();
                bool equipmentGiven = false;
                System.Random random = new System.Random();

                ReduceItemList(points, equipmentGiven, allPrices, equipmentPrices, itemPrices);
                while (allPrices.Count > 0) {
                    List<int> availableItems = itemPrices.Keys.ToList();
                    int nextItem = availableItems[random.Next(availableItems.Count)];
                    if (!itemsPurchased.ContainsKey(nextItem)) {
                        itemsPurchased.Add(nextItem, 0);
                    }
                    int itemPrice = Data.GetItemPrice(nextItem);
                    int itemsGiven = random.Next(1, Mathf.Min(Mathf.FloorToInt(points / itemPrice) + 1 , GetCountRange(nextItem) + 1));
                    itemsPurchased[nextItem] += itemsGiven;
                    points -= itemPrice * itemsGiven;
                    if (Data.allEquipmentIDs.ContainsKey(nextItem)) {
                        equipmentGiven = true;
                    }
                    ReduceItemList(points, equipmentGiven, allPrices, equipmentPrices, itemPrices);
                }

                return itemsPurchased;
            }

            static void ReduceItemList(int points, bool equipmentGiven, List<int> allPrices, List<int> equipmentPrices, Dictionary<int, int> itemPrices) {
                bool cullList = false;
                List<int> indexesToRemove = new List<int>();
                for (int priceIndex = 0; priceIndex < allPrices.Count; priceIndex++) {
                    if (points < allPrices[priceIndex]) {
                        indexesToRemove.Add(priceIndex);
                        cullList = true;
                    }
                }

                indexesToRemove.Reverse();
                foreach (int indexToRemove in indexesToRemove) {
                    allPrices.RemoveAt(indexToRemove);
                }
                if (equipmentGiven) {
                    foreach (int equipmentPrice in equipmentPrices) {
                        if (allPrices.Contains(equipmentPrice)) {
                            allPrices.Remove(equipmentPrice);
                            cullList = true;
                        }
                    }
                }

                if (cullList) {
                    List<int> itemIDsOld = new List<int>();
                    foreach (int itemID in itemPrices.Keys) {
                        itemIDsOld.Add(itemID);
                    }
                    foreach (int itemID in itemIDsOld) {
                        if (points < itemPrices[itemID] || (Data.allEquipmentIDs.ContainsKey(itemID) && equipmentGiven)) {
                            itemPrices.Remove(itemID);
                        }
                    }
                }
            }

            static int GetCountRange(int itemID) {
                if (Data.allEquipmentIDs.ContainsKey(itemID)) {
                    return itemCountRange[5];
                } else {
                    return itemCountRange[Data.GetItemTier(itemID)];
                }
            }

            static public int GetPoints() {
                if (pointsMethod == 0) {
                    return pointsLocked;
                } else if (pointsMethod == 1) {
                    return DataEarntPersistent.userPointsBackup;
                }
                return 0;
            }
        }
    }
}
