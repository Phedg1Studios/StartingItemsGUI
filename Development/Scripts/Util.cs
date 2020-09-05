using System;
using System.IO;
using System.Collections.Generic;
using R2API;
using UnityEngine;
using UnityEngine.UI;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class Util : MonoBehaviour {

            void Update() {
                if (Input.GetKeyDown(KeyCode.F2)) {
                    /*
                    Transform newObject = null;
                    List<string> objectHierarchyB = new List<string>() { "MainMenu", "MENU: Title", "TitleMenu", "SafeZone", "GenericMenuButtonPanel", "JuicePanel", "GenericMenuButton (Logbook)" };
                    if (Util.util.GetObjectFromHierarchy(ref newObject, objectHierarchyB, 0, null)) {
                        //newObject.gameObject;
                    }
                    */
                    //SaveSceneHierarchy();
                    //LogComponentsOfObject(GameObject.Find("ItemEntryIcon(Clone)"));
                    //LogComponentsOfType(typeof(RoR2.UI.TooltipController));
                }
            }

            static public string TrimString(string givenString) {
                if (givenString.Length > 0) {
                    return givenString.Substring(0, givenString.Length - 1);
                }
                return "";
            }

            static public string MultilineToSingleLine(string givenFallback, string givenPath) {
                if (givenPath != "null") {
                if (File.Exists(givenPath)) {
                        StreamReader reader = new StreamReader(givenPath);
                        string line = "";
                        while (reader.Peek() >= 0) {
                            string newString = reader.ReadLine().Replace(Data.splitChar, Data.dictChar);
                            line += newString + Data.splitChar;
                        }
                        reader.Close();
                        line = TrimString(line);
                        return line;
                    }
            }
                return givenFallback;
            }

            static public string ListToString(List<int> givenList) {
                string newString = "";
                foreach (int item in givenList) {
                    newString += item.ToString() + Data.splitChar;
                }
                newString = TrimString(newString);
                return newString;
            }

            static public string DictToString(List<Dictionary<int, int>> givenList) {
                string newString = "";
                foreach (Dictionary<int, int> item in givenList) {
                    foreach (int key in item.Keys) {
                        newString += key.ToString() + Data.dictChar + item[key].ToString() + Data.splitChar;
                    }
                    newString = TrimString(newString);
                    newString += Data.profileChar;
                }
                newString = TrimString(newString);
                return newString;
            }

            static public string GetConfig(Dictionary<string, string> config, List<string> keys) {
                foreach (string key in keys) {
                    if (config.ContainsKey(key)) {
                        return config[key];
                    }
                }
                return "";
            }

            static public void LogComponentsOfObject(GameObject givenObject) {
                if (givenObject != null) {
                    Component[] components = givenObject.GetComponents(typeof(Component));
                    foreach (Component component in components) {
                        print(component);
                        RoR2.GivePickupsOnStart castedComponent = component as RoR2.GivePickupsOnStart;
                        if (castedComponent != null) {
                            print(castedComponent.enabled);
                            print(castedComponent.equipmentString);
                            
                        }
                    };
                }
            }

            static public void LogComponentsOfType(Type givenType) {
                UnityEngine.Object[] sceneObjects = GameObject.FindObjectsOfType(givenType);
                print(sceneObjects.Length);
                foreach (UnityEngine.Object sceneObject in sceneObjects) {
                    RoR2.UI.TooltipController controller = sceneObject as RoR2.UI.TooltipController;
                    print(controller.GetComponent<Canvas>());
                    print(controller.GetComponent<Canvas>().sortingOrder);
                    print(sceneObject.name);
                }
            }

            static public void SaveSceneHierarchy() {
                GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
                string sceneTree = "";
                foreach (GameObject rootObject in rootObjects) {
                    sceneTree = MapHierarchy(rootObject.transform, 0, sceneTree);
                }
                string sceneTreePath = BepInEx.Paths.BepInExRootPath + "/" + "config" + "/" + Data.modFolder + "/" + "SceneTree.txt";
                StreamWriter writer = new StreamWriter(sceneTreePath, false);
                writer.Write(sceneTree);
                writer.Close();
            }

            static string MapHierarchy(Transform givenTransform, int givenDepth, string givenTree) {
                string workingString = "";
                for (int spaceNumber = 0; spaceNumber < givenDepth * 4; spaceNumber++) {
                    workingString += " ";
                }
                workingString += givenTransform.gameObject.name + "\n";
                givenTree += workingString;
                for (int childIndex = 0; childIndex < givenTransform.childCount; childIndex++) {
                    givenTree = MapHierarchy(givenTransform.GetChild(childIndex), givenDepth + 1, givenTree);
                }
                return givenTree;
            }

            static public bool GetObjectFromHierarchy(ref Transform desiredObject, List<string> hierarchy, int hierarchyIndex, Transform parent) {
                bool childFound = false;
                if (hierarchyIndex == 0) {
                    GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
                    for (int rootIndex = 0; rootIndex < rootObjects.Length; rootIndex++) {
                        if (rootObjects[rootIndex].name == hierarchy[hierarchyIndex]) {
                            parent = rootObjects[rootIndex].transform;
                            childFound = true;
                        }
                    }
                } else {
                    for (int childIndex = 0; childIndex < parent.childCount; childIndex++) {
                        if (parent.GetChild(childIndex).name == hierarchy[hierarchyIndex]) {
                            parent = parent.GetChild(childIndex);
                            childFound = true;
                        }
                    }
                }
                if (childFound) {
                    if (hierarchyIndex == hierarchy.Count - 1) {
                        desiredObject = parent;
                        return true;
                    } else {
                        return GetObjectFromHierarchy(ref desiredObject, hierarchy, hierarchyIndex + 1, parent);
                    }
                }
                return false;
            }

            static public void LogAllCharacters(GameObject givenPrefab = null) {
                RoR2.CharacterSpawnCard[] allCharacters = UnityEngine.Resources.LoadAll<RoR2.CharacterSpawnCard>("SpawnCards/CharacterSpawnCards");
                foreach (RoR2.CharacterSpawnCard spawnCard in allCharacters) {
                    print(spawnCard.name);
                    print(spawnCard.prefab.name);
                    /*
                    if (spawnCard.prefab == givenPrefab) {
                        print("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        print(spawnCard.name);
                        if (spawnCard.name == "cscScavLunar") {
                            print(spawnCard.prefab.name);
                        }
                    }
                    */
                }
                print("----");
            }
        }
    }
}
