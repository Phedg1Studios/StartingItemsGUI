using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using R2API.Networking.Interfaces;

namespace Phedg1Studios {
    namespace StartingItemsGUI {
        public class Connection : INetMessage {
            public uint _connectionID = 0;

            public void Serialize(NetworkWriter writer) {
                writer.Write(_connectionID);
            }

            public void Deserialize(NetworkReader reader) {
                _connectionID = reader.ReadUInt32();
            }

            public void OnReceived() {
                GameManager.FinalizePurchases(this);
            }
        }

        public class ItemPurchased : INetMessage {
            public int _itemID = -1;
            public int _itemCount = 0;
            public uint _connectionID = 0;

            public void Serialize(NetworkWriter writer) {
                writer.Write(_itemID);
                writer.Write(_itemCount);
                writer.Write(_connectionID);
            }

            public void Deserialize(NetworkReader reader) {
                _itemID = reader.ReadInt32();
                _itemCount = reader.ReadInt32();
                _connectionID = reader.ReadUInt32();
            }

            public void OnReceived() {
                GameManager.ReceiveItem(this);
            }
        }

        public class SpawnItems : INetMessage {
            public int _mode = -1;
            public uint _connectionID = 0;

            public void Serialize(NetworkWriter writer) {
                writer.Write(_mode);
                writer.Write(_connectionID);
            }

            public void Deserialize(NetworkReader reader) {
                _mode = reader.ReadInt32();
                _connectionID = reader.ReadUInt32();
            }

            public void OnReceived() {
                GameManager.AttemptSpawnItems(this);
            }
        }

        public class GameManager : MonoBehaviour {
            static public Dictionary<uint, List<bool>> status = new Dictionary<uint, List<bool>>();
            static public Dictionary<uint, int> modes = new Dictionary<uint, int>();
            static public Dictionary<uint, Dictionary<int, int>> items = new Dictionary<uint, Dictionary<int, int>>();
            static public Dictionary<uint, CharacterMaster> characterMasters = new Dictionary<uint, CharacterMaster>();
            static public List<Coroutine> spawnItemCoroutines = new List<Coroutine>();

            static public void SetCharacterMaster(uint netId, CharacterMaster characterMaster) {
                characterMasters.Add(netId, characterMaster);
                status[netId][1] = true;
                SpawnItems(netId);
            }

            static public void SendItems(NetworkUser networkUser) {
                Data.RefreshInfo(networkUser.localUser.userProfile.fileName);
                if (Data.modEnabled) {
                    Dictionary<int, int> itemsPurchased = new Dictionary<int, int>();
                    if (Data.mode == DataEarntConsumable.mode) {
                        itemsPurchased = DataEarntConsumable.itemsPurchased[Data.profile[Data.mode]];
                    } else if (Data.mode == DataEarntPersistent.mode) {
                        itemsPurchased = DataEarntPersistent.itemsPurchased[Data.profile[Data.mode]];
                    } else if (Data.mode == DataFree.mode) {
                        itemsPurchased = DataFree.itemsPurchased[Data.profile[Data.mode]];
                    }
                    foreach (int itemID in itemsPurchased.Keys) {
                        ItemPurchased itemPurchased = new ItemPurchased { _itemID = itemID, _itemCount = itemsPurchased[itemID], _connectionID = networkUser.netId.Value };
                        itemPurchased.Send(R2API.Networking.NetworkDestination.Server);
                    }
                    SpawnItems spawnItems = new SpawnItems { _mode = Data.mode, _connectionID = networkUser.netId.Value };
                    spawnItems.Send(R2API.Networking.NetworkDestination.Server);
                }
            }

            static public void ReceiveItem(ItemPurchased givenItem) {
                if (givenItem._itemID != -1 && givenItem._itemCount != 0 && givenItem._connectionID != 0) {
                    if (!status[givenItem._connectionID][0]) {
                        if (!items[givenItem._connectionID].ContainsKey(givenItem._itemID)) {
                            if (Data.ItemExists(givenItem._itemID)) {
                                items[givenItem._connectionID].Add(givenItem._itemID, givenItem._itemCount);
                            }
                        }
                    }
                }
            }

            static public void AttemptSpawnItems(SpawnItems spawnInfo) {
                if (spawnInfo._mode != -1 && spawnInfo._connectionID != 0) {
                    if (!status[spawnInfo._connectionID][0]) {
                        modes[spawnInfo._connectionID] = spawnInfo._mode;
                        status[spawnInfo._connectionID][0] = true;
                        SpawnItems(spawnInfo._connectionID);
                    }
                }
            }

            static public void SpawnItems(uint connectionID) {
                if (status[connectionID][0] && status[connectionID][1]) {
                    if (!status[connectionID][2]) {
                        status[connectionID][2] = true;
                        if (Data.modEnabled && Data.mode == modes[connectionID]) {
                            spawnItemCoroutines.Add(StartingItemsGUI.startingItemsGUI.StartCoroutine(SpawnItemsCoroutine(connectionID)));
                        }
                    }
                }
            }

            static IEnumerator<int> SpawnItemsCoroutine(uint connectionID) {
                while(characterMasters[connectionID].GetBody() == null) {
                    yield return 0;
                }

                foreach (int itemID in items[connectionID].Keys) {
                    for (int itemCount = 0; itemCount < items[connectionID][itemID]; itemCount++) {
                        if (Data.allItemIDs.ContainsKey(itemID)) {
                            characterMasters[connectionID].GetBody().inventory.GiveItem(Data.allItemIDs[itemID]);
                        } else if (Data.allEquipmentIDs.ContainsKey(itemID)) {
                            characterMasters[connectionID].GetBody().inventory.SetEquipmentIndex(Data.allEquipmentIDs[itemID]);
                        }
                    }
                    PickupIndex pickupIndex = new PickupIndex();
                    bool pickupCreated = false;
                    if (Data.allItemIDs.ContainsKey(itemID)) {
                        pickupCreated = true;
                        pickupIndex = new PickupIndex(Data.allItemIDs[itemID]);
                    } else if (Data.allEquipmentIDs.ContainsKey(itemID)) {
                        pickupCreated = true;
                        pickupIndex = new PickupIndex(Data.allEquipmentIDs[itemID]);
                    }
                    if (pickupCreated) {
                        Chat.AddPickupMessage(characterMasters[connectionID].GetBody(), pickupIndex.GetPickupNameToken(), pickupIndex.GetPickupColor(), System.Convert.ToUInt32(items[connectionID][itemID]));
                    }
                }
                if (Data.mode == DataEarntConsumable.mode) {
                    Connection connection = new Connection { _connectionID = connectionID };
                    connection.Send(R2API.Networking.NetworkDestination.Clients);
                }
                yield return 0;
            }

            static public void FinalizePurchases(Connection connection) {
                if (Data.mode == DataEarntConsumable.mode) {
                    foreach (NetworkUser networkUser in NetworkUser.readOnlyInstancesList) {
                        if (networkUser.netId.Value == (uint)connection._connectionID) {
                            if (networkUser.isLocalPlayer) {
                                Data.RefreshInfo(networkUser.localUser.userProfile.fileName);
                                DataEarntConsumable.FinalizePurchases();
                            }
                            break;
                        }
                    }
                }
            }

            static public void ClearItems() {
                status.Clear();
                modes.Clear();
                items.Clear();
                characterMasters.Clear();
                foreach(Coroutine coroutine in spawnItemCoroutines) {
                    StartingItemsGUI.startingItemsGUI.StopCoroutine(coroutine);
                }
                spawnItemCoroutines.Clear();
            }
        }
    }
}
