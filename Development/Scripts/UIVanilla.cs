using UnityEngine;
using UnityEngine.UI;
using RoR2;
using R2API;
using System.Collections.Generic;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class UIVanilla : MonoBehaviour {
            static public GameObject logbookButton;
            static public RoR2.UI.MainMenu.MainMenuController menuController;
            static public RoR2.UI.MainMenu.BaseMainMenuScreen mainMenu;


            static public void GetObjectsFromScene() {
                menuController = GameObject.FindObjectOfType(typeof(RoR2.UI.MainMenu.MainMenuController)) as RoR2.UI.MainMenu.MainMenuController;

                Transform newObject = null;
                List<string> objectHierarchyA = new List<string>() { "MainMenu", "MENU: Title", "TitleMenu"};
                if (Util.GetObjectFromHierarchy(ref newObject, objectHierarchyA, 0, null)) {
                    mainMenu = newObject.GetComponent<RoR2.UI.MainMenu.BaseMainMenuScreen>();
                }
                List<string> objectHierarchyB = new List<string>() { "MainMenu", "MENU: Title", "TitleMenu", "SafeZone", "GenericMenuButtonPanel", "JuicePanel", "GenericMenuButton (Logbook)" };
                if (Util.GetObjectFromHierarchy(ref newObject, objectHierarchyB, 0, null)) {
                    logbookButton = newObject.gameObject;
                }
            }

            static public void CreateMenuButton() {
                if (logbookButton != null) {
                    GameObject button = ButtonCreator.SpawnBlueButton(logbookButton.transform.parent.gameObject, new Vector2(0, 1), new Vector2(320, 48), "Starting Items", TMPro.TextAlignmentOptions.Left, new List<Image>());
                    button.transform.SetSiblingIndex(logbookButton.transform.GetSiblingIndex());
                    float localScale = logbookButton.GetComponent<RectTransform>().rect.width / 320;
                    button.GetComponent<RectTransform>().localScale = new Vector3(localScale, localScale, 1);
                    button.GetComponent<RoR2.UI.HGButton>().onClick.AddListener(() => {
                        UIDrawer.SetMenuStartingItems();
                    });
                }
            }
        }
    }
}
