using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class PointerClick : MonoBehaviour, IPointerClickHandler {
            public RoR2.UI.MPEventSystem eventSystem;
            public UnityEvent onLeftClick = new UnityEvent();
            public UnityEvent onRightClick = new UnityEvent();
            private bool leftHeld = false;
            private bool rightHeld = false;
            private int leftClick = 14;
            private int rightClick = 15;

            public void OnPointerClick(PointerEventData eventData) {
                if (eventData.button == PointerEventData.InputButton.Left) {
                    onLeftClick.Invoke();
                } else if (eventData.button == PointerEventData.InputButton.Middle) {

                } else if (eventData.button == PointerEventData.InputButton.Right) {
                    onRightClick.Invoke();
                }
            }

            void Update() {
                if (!(bool)(UnityEngine.Object)eventSystem || eventSystem.player == null) {
                    return;
                }
                if (leftHeld &&! eventSystem.player.GetButtonDown(leftClick)) {
                    leftHeld = false;
                }
                if (rightHeld && !eventSystem.player.GetButtonDown(rightClick)) {
                    rightHeld = false;
                }

                if ((UnityEngine.Object)eventSystem.currentSelectedGameObject == (UnityEngine.Object)gameObject) {
                    if (eventSystem.player.GetButtonDown(leftClick) && !leftHeld) {
                        onLeftClick.Invoke();
                        leftHeld = true;
                    }
                    if (eventSystem.player.GetButtonDown(rightClick) && !rightHeld) {
                        onRightClick.Invoke();
                        rightHeld = true;
                    }
                }
            }
        }
    }
}
