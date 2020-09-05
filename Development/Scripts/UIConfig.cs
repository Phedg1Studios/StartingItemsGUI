using System;
using System.Collections.Generic;
using UnityEngine;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class UIConfig : MonoBehaviour {
            static public float offsetVertical = 0;
            static public float offsetHorizontal = 75f;
            static public float spacingVertical = 20;
            static public float spacingHorizontal = 20;
            static public float pointsWidth = 350;
            static public float panelPadding = 4;
            static public float scrollPadding = 10;
            static public float itemButtonWidth = 100;
            static public float itemPaddingOuter = 4;
            static public float itemPaddingInner = 1;
            static public float itemSelectionPadding = 3;
            static public float itemTextHeight = 25;
            static public Dictionary<int, int>  storeRows = new Dictionary<int, int>() {
                { 0, 5 },
                { 1, 5 },
                { 2, 6 },
            };
            static public Dictionary<int, int> textCount = new Dictionary<int, int>() {
                { 0, 2 },
                { 1, 2 },
                { 2, 1 },
            };
            static public float profileWidth = 100;
            static public float profileHeight = 30;
            static public Color enabledColor = new Color(1, 1, 1, 1);
            static public Color disabledColor = new Color(0.4f, 0.4f, 0.4f, 1);
            static public float blueButtonWidth = 200;
            static public float blueButtonHeight = 48f;
            static public float blackButtonWidth = 200;
            static public float blackButtonHeight = 48f;
            static public List<int> blackButtons = new List<int>() { 1, 1, 0 };
        }
    }
}
