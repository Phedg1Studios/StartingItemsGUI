using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class ScrollCreator : MonoBehaviour {
            static public GameObject CreateScroll(RectTransform rootTransform, int rows, int textCount, List<int> items, float width, Vector3 position, Dictionary<int, List<Image>> images, Dictionary<int, List<TMPro.TextMeshProUGUI>> texts) {
                Vector2 size = new Vector2();
                size.x = width;
                size.y = (UIConfig.itemButtonWidth + textCount * UIConfig.itemTextHeight + UIConfig.itemPaddingOuter * 2) * rows + UIConfig.scrollPadding * 2 + UIConfig.panelPadding * 2;
                GameObject itemsScroll = ElementCreator.SpawnImageSize(new List<Image>(), rootTransform.gameObject, null, new Color(0, 0, 0, 0), new Vector2(0, 1), size, position);
                itemsScroll.name = "Scroll";
                ScrollRect itemsScrollScroll = itemsScroll.AddComponent<ScrollRect>();
                itemsScrollScroll.horizontal = true;
                itemsScrollScroll.vertical = false;
                itemsScrollScroll.horizontalScrollbar = null;
                itemsScrollScroll.verticalScrollbar = null;
                itemsScrollScroll.movementType = ScrollRect.MovementType.Clamped;
                itemsScrollScroll.inertia = false;
                itemsScrollScroll.scrollSensitivity = 50;
                itemsScroll.AddComponent<RoR2.UI.MPEventSystemLocator>();
                itemsScroll.AddComponent<RoR2.UI.ScrollToSelection>();

                GameObject scrollbar = new GameObject("Scroll Bar");
                scrollbar.transform.parent = itemsScroll.transform;
                CanvasGroup canvasGroup = scrollbar.AddComponent<CanvasGroup>();
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
                scrollbar.AddComponent<RoR2.UI.MPEventSystemLocator>();
                RoR2.UI.MPScrollbar scrollbarScrollbar = scrollbar.AddComponent<RoR2.UI.MPScrollbar>();
                scrollbarScrollbar.direction = Scrollbar.Direction.LeftToRight;
                itemsScrollScroll.horizontalScrollbar = scrollbarScrollbar;

                GameObject panel = PanelCreator.CreatePanel(itemsScroll.transform);
                itemsScrollScroll.viewport = panel.transform.GetChild(0).GetComponent<RectTransform>();

                Image itemsContentImage = ElementCreator.SpawnImageOffset(new List<Image>(), panel.transform.GetChild(0).gameObject, null, new Color(0, 0, 0, 0), new Vector2(0, 1), new Vector2(0, 0), new Vector2(0, 0));
                itemsContentImage.raycastTarget = true;
                GameObject itemsContent = itemsContentImage.gameObject;

                RectTransform itemsContentTransform = itemsContentImage.GetComponent<RectTransform>();
                int itemsPerRow = Mathf.CeilToInt(items.Count * 1f / rows);
                int itemsInBottomRow = items.Count - itemsPerRow * (rows - 1);
                itemsContentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Max(UIConfig.scrollPadding * 2 + (UIConfig.itemButtonWidth + UIConfig.itemPaddingOuter * 2) * itemsPerRow, itemsScrollScroll.viewport.rect.width));

                List<RectTransform> itemButtons = new List<RectTransform>();
                for (int index = 0; index < items.Count; index++) {
                    itemButtons.Add(ButtonCreator.SpawnItemButton(itemsContent, textCount, items[index], images, texts));
                }

                itemsScrollScroll.content = itemsContentTransform;
                itemsScrollScroll.horizontalNormalizedPosition = 0.5f;

                for (int row = 0; row < rows; row++) {
                    for (int collumn = 0; collumn < itemsPerRow; collumn++) {
                        if (row * itemsPerRow + collumn < itemButtons.Count) {
                            float offset = (itemsContentTransform.rect.width - UIConfig.scrollPadding * 2 - (UIConfig.itemButtonWidth + UIConfig.itemPaddingOuter * 2) * itemsPerRow) / 2f;
                            if (row == rows - 1) {
                                offset += (itemsPerRow - itemsInBottomRow) * (UIConfig.itemButtonWidth + UIConfig.itemPaddingOuter * 2) / 2f;
                            }
                            itemButtons[row * itemsPerRow + collumn].localPosition = new Vector3(UIConfig.scrollPadding + collumn * (UIConfig.itemButtonWidth + UIConfig.itemPaddingOuter * 2) + offset, -UIConfig.scrollPadding - row * (UIConfig.itemButtonWidth + textCount * UIConfig.itemTextHeight + UIConfig.itemPaddingOuter * 2), 0);
                        }
                    }
                }

                return itemsScroll;
            }
        }
    }
}
