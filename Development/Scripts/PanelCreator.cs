using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class PanelCreator : MonoBehaviour {
            static public GameObject CreatePanel(Transform givenParent) {
                Image outline = ElementCreator.SpawnImageOffset(new List<Image>(), givenParent.gameObject, Resources.panelTextures[7], new Color(0.357f, 0.373f, 0.471f, 1), new Vector2(0, 1), new Vector2(0, 0), new Vector2(0, 0));
                Image background = ElementCreator.SpawnImageOffset(new List<Image>(), outline.gameObject, null, new Color(0, 0, 0, 0.8f), new Vector2(0, 1), new Vector2(UIConfig.panelPadding, UIConfig.panelPadding), new Vector2(-UIConfig.panelPadding, -UIConfig.panelPadding));
                background.gameObject.AddComponent<RectMask2D>();
                return outline.gameObject;
            }

            static public GameObject CreatePanelSize(Transform givenParent) {
                List<Image> images = new List<Image>();
                ElementCreator.SpawnImageSize(images, givenParent.gameObject, Resources.panelTextures[7], new Color(0.357f, 0.373f, 0.471f, 1), new Vector2(0, 1), new Vector2(100, 100), new Vector3(0, 0, 0));
                Image background = ElementCreator.SpawnImageOffset(new List<Image>(), images[0].gameObject, null, new Color(0, 0, 0, 0.8f), new Vector2(0, 1), new Vector2(UIConfig.panelPadding, UIConfig.panelPadding), new Vector2(-UIConfig.panelPadding, -UIConfig.panelPadding));
                background.gameObject.AddComponent<RectMask2D>();
                return images[0].gameObject;
            }
        }
    }
}
