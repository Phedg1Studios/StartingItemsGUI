using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RoR2;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class UIDrawerRandom : MonoBehaviour {
            static public void DrawUI() {
                foreach (int itemID in UIDrawer.itemTexts.Keys) {
                    UIDrawer.itemTexts[itemID][0].text = Data.GetItemPrice(itemID).ToString();
                    for (int imageIndex = 0; imageIndex < 2; imageIndex++) {
                        UIDrawer.itemImages[itemID][imageIndex + 2].gameObject.SetActive(false);
                    }

                    if (DataEarntPersistent.userPointsBackup < Data.GetItemPrice(itemID)) {
                        UIDrawer.itemTexts[itemID][0].color = UIConfig.disabledColor;
                        for (int imageIndex = 0; imageIndex < 2; imageIndex++) {
                            UIDrawer.itemImages[itemID][imageIndex].color = UIConfig.disabledColor;
                        }
                    }

                }

                UIDrawer.pointText.text = "CREDITS: " + DataRandom.GetPoints() + " ¢";
            }

            static public void Refresh() {
                
            }
        }
    }
}
