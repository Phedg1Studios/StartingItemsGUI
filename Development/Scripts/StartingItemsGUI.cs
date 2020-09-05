using BepInEx;
using RoR2;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        [BepInDependency("com.bepis.r2api")]
        [R2API.Utils.R2APISubmoduleDependency("PrefabAPI")]
        [R2API.Utils.R2APISubmoduleDependency("ResourcesAPI")]
        [R2API.Utils.R2APISubmoduleDependency("NetworkingAPI")]
        [BepInPlugin(PluginGUID, "StartingItemsGUI", "1.1.13")]
        

        public class StartingItemsGUI : BaseUnityPlugin {
            public const string PluginGUID = "com.Phedg1Studios.StartingItemsGUI";

            static public StartingItemsGUI startingItemsGUI;
            List<Coroutine> characterMasterCoroutines = new List<Coroutine>();

            void OnceSetup() {
                startingItemsGUI = this;
                gameObject.AddComponent<Util>();
                Resources.LoadResources();
                Data.PopulateItemCatalogues();
                SceneLoadSetup();
                R2API.Networking.NetworkingAPI.RegisterMessageType<Connection>();
                R2API.Networking.NetworkingAPI.RegisterMessageType<ItemPurchased>();
                R2API.Networking.NetworkingAPI.RegisterMessageType<SpawnItems>();
            }

            void SceneLoadSetup() {
                UIDrawer.CreateCanvas();
                UIVanilla.GetObjectsFromScene();
                UIVanilla.CreateMenuButton();
            }

            void Start() {
                OnceSetup();
                On.RoR2.PreGameController.OnEnable += ((orig, preGameController) => {
                    Data.localUsers.Clear();
                    Data.SetForcedMode(-1);
                    GameManager.ClearItems();
                    orig(preGameController);
                });
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, mode) => {
                    if (scene.name == "title") {
                        Data.localUsers.Clear();
                        Data.SetForcedMode(-1);
                        GameManager.ClearItems();
                        SceneLoadSetup();
                    }
                };
                RoR2.Run.onRunStartGlobal += (run) => {
                    Data.RefreshInfo();
                    Data.SetForcedMode(Data.mode);
                    foreach (Coroutine coroutine in characterMasterCoroutines) {
                        if (coroutine != null) {
                            StopCoroutine(coroutine);
                        }
                    }
                    characterMasterCoroutines.Clear();
                    if (NetworkServer.active) {
                        foreach (NetworkUser networkUser in RoR2.NetworkUser.readOnlyInstancesList) {
                            GameManager.status.Add(networkUser.netId.Value, new List<bool>() { false, false, false });
                            GameManager.items.Add(networkUser.netId.Value, new Dictionary<int, int>());
                            GameManager.modes.Add(networkUser.netId.Value, -1);
                            characterMasterCoroutines.Add(StartCoroutine(GetMasterController(networkUser)));
                        }
                    }
                    if (NetworkClient.active) {
                        foreach (NetworkUser networkUser in RoR2.NetworkUser.readOnlyInstancesList) {
                            if (networkUser.isLocalPlayer) {
                                Data.localUsers.Add(networkUser.localUser.userProfile.fileName);
                                GameManager.SendItems(networkUser);
                            }
                        }
                    }
                };
                RoR2.Run.onClientGameOverGlobal  += (run, runReport) => {
                    DataEarntConsumable.UpdateUserPointsStages(run, runReport);
                    DataEarntPersistent.UpdateUserPointsStages(run, runReport);
                };
                On.RoR2.CharacterBody.OnDeathStart += ((orig, characterBody) => {
                    DataEarntConsumable.UpdateUserPointsBoss(characterBody.name);
                    DataEarntPersistent.UpdateUserPointsBoss(characterBody.name);
                    orig(characterBody);
                });
                On.RoR2.UI.ScrollToSelection.ScrollToRect += (scrollToRect, scrollToSelection, transform) => {
                    scrollToRect(scrollToSelection, transform);

                    ScrollRect scrollRect = scrollToSelection.GetComponent<ScrollRect>();
                    if (!scrollRect.horizontal || !(bool)(Object)scrollRect.horizontalScrollbar) {
                        return;
                    }
                    Vector3[] targetWorldCorners = new Vector3[4];
                    Vector3[] viewPortWorldCorners = new Vector3[4];
                    Vector3[] contentWorldCorners = new Vector3[4];
                    scrollToSelection.GetComponent<RoR2.UI.MPEventSystemLocator>().eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().GetWorldCorners(targetWorldCorners);
                    scrollRect.viewport.GetWorldCorners(viewPortWorldCorners);
                    scrollRect.content.GetWorldCorners(contentWorldCorners);
                    float x5 = targetWorldCorners[2].x;
                    double x6 = (double)targetWorldCorners[0].x;
                    float x7 = viewPortWorldCorners[2].x;
                    float x8 = viewPortWorldCorners[0].x;
                    float x9 = contentWorldCorners[2].x;
                    float x10 = contentWorldCorners[0].x;
                    float num5 = x5 - x7;
                    double num6 = (double)x8;
                    float num7 = (float)(x6 - num6);
                    float num8 =  (x9 - x10) - (x7 - x8);
                    if ((double)num5 > 0.0)
                        scrollRect.horizontalScrollbar.value += num5 / num8;
                    if ((double)num7 >= 0.0)
                        return;
                    scrollRect.horizontalScrollbar.value += num7 / num8;
                };
            }

            IEnumerator<float> GetMasterController(NetworkUser networkUser) {
                PlayerCharacterMasterController masterController = networkUser.masterController;
                while (masterController == null) {
                    masterController = networkUser.masterController;
                    yield return 0;
                }
                GameManager.SetCharacterMaster(networkUser.netId.Value, networkUser.masterController.master);
            }
        }
    }
}
 