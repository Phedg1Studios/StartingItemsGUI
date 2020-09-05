using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RoR2;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class UiDrawerFree : MonoBehaviour {
            static public void DrawUI() {

            }

            static public void Refresh() {
                UIDrawer.pointText.text = "";

                foreach (int itemID in UIDrawer.itemImages.Keys) {
                    if (DataFree.itemsPurchased[Data.profile[Data.mode]].ContainsKey(itemID)) {
                        UIDrawer.itemTexts[itemID][0].text = DataFree.itemsPurchased[Data.profile[Data.mode]][itemID].ToString();
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
