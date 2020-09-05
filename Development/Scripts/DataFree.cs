using System;
using System.IO;
using System.Collections.Generic;
using RoR2;
using UnityEngine;
using R2API;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class DataFree : MonoBehaviour {
            static public List<Dictionary<int, int>> itemsPurchased = new List<Dictionary<int, int>>();
            static public int mode = 2;
            static private string itemsPurchasedFile = "ItemsFree.txt";

            static public List<string> itemsPurchasedName = new List<string>() { "itemsPurchasedFree" };


            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


            static public void RefreshInfo(Dictionary<string, string> configGlobal, Dictionary<string, string> configProfile) {
                Data.GetItemsPurchased(configProfile, itemsPurchasedName, itemsPurchasedFile, itemsPurchased, -1, mode);
            }

            static public void VerifyItemsPurchased() {

            }

            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


            static public void BuyItem(int itemID) {
                bool boughtItem = false;
                for (int transactionIndex = 0; transactionIndex < Data.buyMultiplier; transactionIndex++) {
                    if (!itemsPurchased[Data.profile[Data.mode]].ContainsKey(itemID)) {
                        itemsPurchased[Data.profile[Data.mode]].Add(itemID, 0);
                    }
                    itemsPurchased[Data.profile[Data.mode]][itemID] += 1;
                    boughtItem = true;
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
        }
    }
}
