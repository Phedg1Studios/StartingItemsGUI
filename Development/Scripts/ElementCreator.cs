using System;
using System.IO;
using System.Collections.Generic;
using R2API;
using UnityEngine;
using UnityEngine.UI;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class ElementCreator : MonoBehaviour {
            static public GameObject SpawnImageSize(List<Image> images, GameObject parent, Sprite sprite, Color colour, Vector2 pivot, Vector2 size, Vector3 localPosition) {
                GameObject image = new GameObject();
                image.name = "Image";
                image.transform.parent = parent.transform;
                RectTransform imageTransform = image.AddComponent<RectTransform>();
                imageTransform.pivot = pivot;
                imageTransform.anchorMin = new Vector2(0, 0);
                imageTransform.anchorMax = new Vector2(1, 1);
                imageTransform.localPosition = localPosition;
                imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
                imageTransform.localScale = new Vector3(1, 1, 1);
                Image imageImage = image.AddComponent<Image>();
                imageImage.color = colour;
                imageImage.sprite = sprite;
                imageImage.type = Image.Type.Sliced;
                imageImage.raycastTarget = false;
                images.Add(imageImage);
                return image;
            }

            static public Image SpawnImageOffset(List<Image> images, GameObject parent, Sprite sprite, Color colour, Vector2 pivot, Vector2 offsetMin, Vector2 offsetMax) {
                GameObject image = new GameObject();
                image.name = "Image";
                image.transform.parent = parent.transform;
                RectTransform imageTransform = image.AddComponent<RectTransform>();
                imageTransform.pivot = pivot;
                imageTransform.anchorMin = new Vector2(0, 0);
                imageTransform.anchorMax = new Vector2(1, 1);
                imageTransform.offsetMin = offsetMin;
                imageTransform.offsetMax = offsetMax;
                imageTransform.localScale = new Vector3(1, 1, 1);
                Image imageImage = image.AddComponent<Image>();
                imageImage.color = colour;
                imageImage.sprite = sprite;
                imageImage.type = Image.Type.Sliced;
                imageImage.raycastTarget = false;
                images.Add(imageImage);
                return imageImage;
            }

            static public void SpawnTextOffset(List<TMPro.TextMeshProUGUI> texts, GameObject parent, Color colour, float size, float textOffset, Vector2 offsetMin, Vector2 offsetMax) {
                GameObject text = new GameObject();
                text.name = "Text";
                text.transform.parent = parent.transform;
                TMPro.TextMeshProUGUI textText = text.AddComponent<RoR2.UI.HGTextMeshProUGUI>();
                textText.font = Resources.fonts[0];
                //textText.faceColor = colour;
                textText.color = colour;
                textText.fontSize = size;
                textText.alignment = TMPro.TextAlignmentOptions.Center;
                textText.text = "";
                textText.raycastTarget = false;

                RectTransform textTransform = text.GetComponent<RectTransform>();
                textTransform.pivot = new Vector2(0, 1);
                textTransform.anchorMin = new Vector2(0, 0);
                textTransform.anchorMax = new Vector2(1, 1);
                textTransform.offsetMin = new Vector2(offsetMin.x, offsetMin.y + textOffset);
                textTransform.offsetMax = new Vector2(offsetMax.x, offsetMax.y + textOffset);
                textTransform.localScale = new Vector3(1, 1, 1);
                texts.Add(textText);
            }

            static public void SpawnTextSize(List<TMPro.TextMeshProUGUI> texts, GameObject parent, Color colour, float fontSize, float textOffset, Vector2 pivot, Vector2 size, Vector3 localPosition) {
                GameObject text = new GameObject();
                text.name = "Text";
                text.transform.parent = parent.transform;
                TMPro.TextMeshProUGUI textText = text.AddComponent<TMPro.TextMeshProUGUI>();
                textText.font = Resources.fonts[0];
                //textText.faceColor = colour;
                textText.color = colour;
                textText.fontSize = fontSize;
                textText.alignment = TMPro.TextAlignmentOptions.Center;
                textText.text = "";
                textText.raycastTarget = false;
                RectTransform textTransform = text.GetComponent<RectTransform>();
                textTransform.pivot = pivot;
                textTransform.anchorMin = new Vector2(0, 0);
                textTransform.anchorMax = new Vector2(1, 1);
                textTransform.localPosition = localPosition;
                textTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                textTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
                textTransform.localScale = new Vector3(1, 1, 1);
                texts.Add(textText);
            }

            static public GameObject SpawnButtonOffset(GameObject parent, Sprite sprite, ColorBlock colourBlock, bool isFallback = false) {
                GameObject button = new GameObject();
                button.name = "Button";
                button.transform.parent = parent.transform;
                button.AddComponent<RoR2.UI.MPEventSystemLocator>();

                Image buttonImage = button.AddComponent<Image>();
                buttonImage.color = new Color(1, 1, 1, 1);
                buttonImage.sprite = sprite;
                buttonImage.type = Image.Type.Sliced;
                buttonImage.raycastTarget = true;

                RoR2.UI.HGButton buttonButton = button.AddComponent<RoR2.UI.HGButton>();
                buttonButton.showImageOnHover = true;
                buttonButton.targetGraphic = buttonImage;
                buttonButton.colors = colourBlock;
                if (isFallback) {
                    buttonButton.defaultFallbackButton = true;
                }
                buttonButton.disableGamepadClick = true;

                RectTransform buttonTransform = button.GetComponent<RectTransform>();
                buttonTransform.anchorMin = new Vector2(0, 0);
                buttonTransform.anchorMax = new Vector2(1, 1);
                buttonTransform.offsetMin = new Vector2(0, 0);
                buttonTransform.offsetMax = new Vector2(0, 0);
                buttonTransform.localScale = new Vector3(1, 1, 1);
                return button;
            }

            static public GameObject SpawnButtonSize(GameObject parent, Sprite sprite, ColorBlock colourBlock, Vector2 pivot, Vector2 givenSize, bool isFallback = false) {
                GameObject button = new GameObject();
                button.name = "Button";
                button.transform.parent = parent.transform;
                button.AddComponent<RoR2.UI.MPEventSystemLocator>();

                Image buttonImage = button.AddComponent<Image>();
                buttonImage.color = new Color(1, 1, 1, 1);
                buttonImage.sprite = sprite;
                buttonImage.type = Image.Type.Sliced;
                buttonImage.raycastTarget = true;

                RoR2.UI.HGButton buttonButton = button.AddComponent<RoR2.UI.HGButton>();
                buttonButton.showImageOnHover = true;
                buttonButton.targetGraphic = buttonImage;
                buttonButton.colors = colourBlock;
                if (isFallback) {
                    buttonButton.defaultFallbackButton = true;
                }
                buttonButton.disableGamepadClick = true;

                RectTransform buttonTransform = button.GetComponent<RectTransform>();
                buttonTransform.pivot = pivot;
                buttonTransform.anchorMin = new Vector2(0, 1);
                buttonTransform.anchorMax = new Vector2(0, 1);
                buttonTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, givenSize.x);
                buttonTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, givenSize.y);
                buttonTransform.localScale = new Vector3(1, 1, 1);
                return button;
            }
        }
    }
}
