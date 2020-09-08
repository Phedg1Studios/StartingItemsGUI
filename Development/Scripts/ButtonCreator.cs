using BepInEx;
using RoR2;
using R2API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Data.data.GetItemPoints(givenID).ToString() + " ¢";

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class ButtonCreator : MonoBehaviour {
            static public GameObject SpawnBlueButton(GameObject parent, Vector2 givenPivot, Vector2 givenSize, string givenLabel, TMPro.TextAlignmentOptions alignment, List<Image> images, bool isFallback = false) {
                ColorBlock colourBlock = new ColorBlock();
                colourBlock.disabledColor = new Color(0.255f, 0.201f, 0.201f, 0.714f);
                colourBlock.highlightedColor = new Color(0.988f, 1.000f, 0.693f, 0.733f);
                colourBlock.normalColor = new Color(0.327f, 0.403f, 0.472f, 1.000f);
                colourBlock.pressedColor = new Color(0.740f, 0.755f, 0.445f, 0.984f);
                colourBlock.colorMultiplier = 1;

                GameObject button = ElementCreator.SpawnButtonSize(parent, Resources.panelTextures[0], colourBlock, givenPivot, givenSize, isFallback);
                ElementCreator.SpawnImageOffset(images, button, Resources.panelTextures[7], new Color(1, 1, 1, 1), new Vector2(0.5f, 0.5f), new Vector2(-6, -6), new Vector2(6, 6));
                images[images.Count - 1].gameObject.SetActive(false);
                ElementCreator.SpawnImageOffset(new List<Image>(), button, Resources.panelTextures[1], new Color(1, 1, 1, 0.286f), new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(0, 0));
                Image highlightImage = ElementCreator.SpawnImageOffset(new List<Image>(), button, Resources.panelTextures[2], new Color(1, 1, 1, 1), new Vector2(0.5f, 0.5f), new Vector2(-4, -12), new Vector2(12, 4));
                button.GetComponent<RoR2.UI.HGButton>().imageOnHover = highlightImage;
                List<TMPro.TextMeshProUGUI> buttonText = new List<TMPro.TextMeshProUGUI>();
                ElementCreator.SpawnTextOffset(buttonText, button, new Color(1, 1, 1, 1), 24, 0, new Vector2(12, 4), new Vector2(-12, -4));
                buttonText[0].alignment = alignment;
                buttonText[0].text = givenLabel;
                buttonText[0].lineSpacing = -25;
                return button;
            }

            static public GameObject SpawnBlackButton(GameObject parent, Vector2 givenSize, string givenLabel, List<TMPro.TextMeshProUGUI> texts, bool isFallback = false) {
                ColorBlock colourBlockA = new ColorBlock();
                colourBlockA.disabledColor = new Color(1, 1, 1, 1);
                colourBlockA.highlightedColor = new Color(1, 1, 1, 1);
                colourBlockA.normalColor = new Color(1, 1, 1, 1);
                colourBlockA.pressedColor = new Color(1, 1, 1, 1);
                colourBlockA.colorMultiplier = 1;

                ColorBlock colourBlockB = new ColorBlock();
                colourBlockB.disabledColor = new Color(0.094f, 0.094f, 0.094f, 0.286f);
                colourBlockB.highlightedColor = new Color(0.300f, 0.300f, 0.300f, 1.00f);
                colourBlockB.normalColor = new Color(0.300f, 0.300f, 0.300f, 1.00f);
                colourBlockB.pressedColor = new Color(0.500f, 0.500f, 0.500f, 1.000f);
                colourBlockB.colorMultiplier = 1;

                GameObject buttonA = ElementCreator.SpawnButtonSize(parent, Resources.panelTextures[4], colourBlockA, new Vector2(0, 1), givenSize);
                GameObject buttonB = ElementCreator.SpawnButtonOffset(buttonA, Resources.panelTextures[3], colourBlockB, isFallback);

                buttonA.GetComponent<RoR2.UI.HGButton>().interactable = false;
                Image highlightImageA = ElementCreator.SpawnImageOffset(new List<Image>(), buttonA, Resources.panelTextures[2], new Color(1, 1, 1, 1), new Vector2(0.5f, 0.5f), new Vector2(-4, -12), new Vector2(12, 4));
                buttonA.GetComponent<RoR2.UI.HGButton>().imageOnHover = buttonB.GetComponent<Image>();
                buttonB.GetComponent<RoR2.UI.HGButton>().imageOnHover = highlightImageA;
                ElementCreator.SpawnTextOffset(texts, buttonA, new Color(1, 1, 1, 1), 24, 0, new Vector2(12, 4), new Vector2(-12, -4));
                texts[0].text = givenLabel;
                return buttonB;
            }

            static public RectTransform SpawnItemButton(GameObject root, int textCount, int givenID, Dictionary<int, List<Image>> images, Dictionary<int, List<TMPro.TextMeshProUGUI>> texts, bool isFallback = false) {
                images.Add(givenID, new List<Image>());
                texts.Add(givenID, new List<TMPro.TextMeshProUGUI>());

                ColorBlock colourBlockA = new ColorBlock();
                colourBlockA.colorMultiplier = 1;
                colourBlockA.disabledColor = new Color(0, 0, 0, 0);
                colourBlockA.highlightedColor = new Color(0, 0, 0, 0);
                colourBlockA.normalColor = new Color(0, 0, 0, 0);
                colourBlockA.pressedColor = new Color(0, 0, 0, 0);

                Vector2 size = new Vector2();
                size.x = UIConfig.itemButtonWidth + UIConfig.itemPaddingOuter * 2;
                size.y = UIConfig.itemButtonWidth + textCount * UIConfig.itemTextHeight + UIConfig.itemPaddingOuter * 2;
                GameObject item = ElementCreator.SpawnButtonSize(root, null, colourBlockA, new Vector2(0, 1), size, isFallback);
                GameObject scaler = ElementCreator.SpawnImageOffset(new List<Image>(), item, null, new Color(0, 0, 0, 0), new Vector2(0, 1), new Vector2(UIConfig.itemPaddingOuter, UIConfig.itemPaddingOuter), new Vector2(-UIConfig.itemPaddingOuter, -UIConfig.itemPaddingOuter)).gameObject;

                ElementCreator.SpawnImageOffset(new List<Image>(), scaler, Resources.panelTextures[1], new Color(0.286f, 0.286f, 0.286f, 1), new Vector2(0, 1), new Vector2(-2, textCount * UIConfig.itemTextHeight - 2), new Vector2(2, 2));
                for (int textIndex = 0; textIndex < textCount; textIndex++) {
                    ElementCreator.SpawnImageOffset(new List<Image>(), scaler, Resources.panelTextures[1], new Color(0.286f, 0.286f, 0.286f, 1), new Vector2(0, 1), new Vector2(-2, textIndex * UIConfig.itemTextHeight - 2), new Vector2(2, -UIConfig.itemButtonWidth - (textCount - 1 - textIndex) * UIConfig.itemTextHeight + UIConfig.itemPaddingInner + 2));
                    ElementCreator.SpawnImageOffset(new List<Image>(), scaler, Resources.panelTextures[0], new Color(0.120f, 0.120f, 0.120f, 1), new Vector2(0, 1), new Vector2(-2, textIndex * UIConfig.itemTextHeight - 2), new Vector2(2, -UIConfig.itemButtonWidth - (textCount - 1 - textIndex) * UIConfig.itemTextHeight + UIConfig.itemPaddingInner + 2));
                }

                ElementCreator.SpawnImageOffset(images[givenID], scaler, Resources.tierTextures[Data.GetItemTier(givenID)], new Color(1, 1, 1), new Vector2(0, 1), new Vector2(UIConfig.itemPaddingInner, textCount * UIConfig.itemTextHeight + UIConfig.itemPaddingInner), new Vector2(-UIConfig.itemPaddingInner, -UIConfig.itemPaddingInner));
                Sprite itemImage = null;
                if (Data.allItemIDs.ContainsKey(givenID)) {
                    itemImage = RoR2.ItemCatalog.GetItemDef(Data.allItemIDs[givenID]).pickupIconSprite;
                } else if (Data.allEquipmentIDs.ContainsKey(givenID)) {
                    itemImage = RoR2.EquipmentCatalog.GetEquipmentDef(Data.allEquipmentIDs[givenID]).pickupIconSprite;
                }
                ElementCreator.SpawnImageOffset(images[givenID], scaler, itemImage, new Color(1, 1, 1), new Vector2(0, 1), new Vector2(UIConfig.itemPaddingInner, textCount * UIConfig.itemTextHeight + UIConfig.itemPaddingInner), new Vector2(-UIConfig.itemPaddingInner, -UIConfig.itemPaddingInner));

                ElementCreator.SpawnImageOffset(images[givenID], scaler, Resources.panelTextures[1], new Color(0.988f, 1.000f, 0.693f, 0.733f), new Vector2(0.5f, 0), new Vector2(1, textCount * UIConfig.itemTextHeight + 1), new Vector2(-1, -1));
                ElementCreator.SpawnImageSize(images[givenID], images[givenID][images[givenID].Count - 1].gameObject, Resources.panelTextures[6], new Color(0.988f, 1.000f, 0.693f, 0.733f),new Vector2(0.5f, 0.5f), new Vector2(20, 20), new Vector3(0, 3, 0));

                for (int textIndex = 0; textIndex < textCount; textIndex++) {
                    ElementCreator.SpawnTextOffset(texts[givenID], scaler, new Color(1, 1, 1), 24, 1, new Vector2(UIConfig.itemPaddingInner, textIndex * UIConfig.itemTextHeight + UIConfig.itemPaddingInner), new Vector2(-UIConfig.itemPaddingInner, -UIConfig.itemButtonWidth - (textCount - 1 - textIndex) * UIConfig.itemTextHeight - UIConfig.itemPaddingInner));
                    texts[givenID][texts[givenID].Count - 1].text = "";
                }

                ColorBlock colourBlockB = new ColorBlock();
                colourBlockB.colorMultiplier = 1;
                colourBlockB.disabledColor = new Color(1, 1, 1, 1);
                colourBlockB.highlightedColor = new Color(1, 1, 1, 1);
                colourBlockB.normalColor = new Color(1, 1, 1, 1);
                colourBlockB.pressedColor = new Color(0.8f, 0.8f, 0.8f, 1);

                /*
                GameObject highlight = ElementCreator.SpawnButtonOffset(item, Resources.resources.panelTextures[5], colourBlockB);
                highlight.GetComponent<RoR2.UI.HGButton>().showImageOnHover = false;
                RectTransform highlightTransform = highlight.GetComponent<RectTransform>();
                highlightTransform.offsetMin = new Vector2(-5, -5);
                highlightTransform.offsetMax = new Vector2(5, 5);
                item.GetComponent<RoR2.UI.HGButton>().imageOnHover = highlight.GetComponent<Image>();
                */

                Image highlight = ElementCreator.SpawnImageOffset(new List<Image>(), scaler, Resources.panelTextures[5], new Color(1, 1, 1, 1), new Vector2(0.5f, 0.5f), new Vector2(-5, -5), new Vector2(5, 5));
                item.GetComponent<RoR2.UI.HGButton>().imageOnHover = highlight.GetComponent<Image>();

                RoR2.UI.TooltipProvider tooltipProvider = item.AddComponent<RoR2.UI.TooltipProvider>();
                tooltipProvider.titleColor = Resources.colours[Data.GetItemTier(givenID)];
                tooltipProvider.bodyColor = Resources.colours[6];
                if (Data.allItemIDs.ContainsKey(givenID)) {
                    tooltipProvider.titleToken = RoR2.ItemCatalog.GetItemDef(Data.allItemIDs[givenID]).nameToken;
                    tooltipProvider.bodyToken = RoR2.ItemCatalog.GetItemDef(Data.allItemIDs[givenID]).descriptionToken;
                } else if (Data.allEquipmentIDs.ContainsKey(givenID)) {
                    tooltipProvider.titleToken = RoR2.EquipmentCatalog.GetEquipmentDef(Data.allEquipmentIDs[givenID]).nameToken;
                    tooltipProvider.bodyToken = RoR2.EquipmentCatalog.GetEquipmentDef(Data.allEquipmentIDs[givenID]).descriptionToken;
                }

                PointerClick pointerClick = item.AddComponent<PointerClick>();
                pointerClick.eventSystem = item.GetComponent<RoR2.UI.MPEventSystemLocator>().eventSystem;
                pointerClick.onLeftClick.AddListener(() => {
                    Data.LeftClick(givenID);
                });
                pointerClick.onRightClick.AddListener(() => {
                    Data.RightClick(givenID);
                });
                return item.GetComponent<RectTransform>();
            }
        }
    }
}
