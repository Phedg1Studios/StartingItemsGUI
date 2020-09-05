using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RoR2;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class UIDrawerEarntPersistent : MonoBehaviour {
            static public void DrawUI() {
                foreach (int itemID in UIDrawer.itemTexts.Keys) {
                    UIDrawer.itemTexts[itemID][1].text = Data.GetItemPrice(itemID).ToString();
                }
            }

            static public void Refresh() {
                UIDrawer.pointText.text = "CREDITS: " + DataEarntPersistent.userPoints.ToString() + " ¢";

                foreach (int itemID in UIDrawer.itemImages.Keys) {
                    if (DataEarntPersistent.itemsPurchased[Data.profile[Data.mode]].ContainsKey(itemID)) {
                        UIDrawer.itemTexts[itemID][0].text = DataEarntPersistent.itemsPurchased[Data.profile[Data.mode]][itemID].ToString();
                        for (int imageIndex = 0; imageIndex < 2; imageIndex++) {
                            UIDrawer.itemImages[itemID][imageIndex].color = UIConfig.enabledColor;
                        }
                        for (int imageIndex = 0; imageIndex < 2; imageIndex++) {
                            UIDrawer.itemImages[itemID][imageIndex + 2].gameObject.SetActive(true);
                        }
                        foreach (TMPro.TextMeshProUGUI text in UIDrawer.itemTexts[itemID]) {
                            text.color = UIConfig.enabledColor;
                        }
                    } else {
                        UIDrawer.itemTexts[itemID][0].text = "0";
                        for (int imageIndex = 0; imageIndex < 2; imageIndex++) {
                            UIDrawer.itemImages[itemID][imageIndex].color = UIConfig.disabledColor;
                        }
                        for (int imageIndex = 0; imageIndex < 2; imageIndex++) {
                            UIDrawer.itemImages[itemID][imageIndex + 2].gameObject.SetActive(false);
                        }
                        foreach (TMPro.TextMeshProUGUI text in UIDrawer.itemTexts[itemID]) {
                            text.color = UIConfig.disabledColor;
                        }
                    }
                }
            }
        }
    }
}
