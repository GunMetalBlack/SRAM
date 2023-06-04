using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

using System;

public class PlayerNetwork : NetworkBehaviour
{
    //Some Scetch Vars For UI
    [SerializeField] TextMeshProUGUI attackDefendTextMesh;
    [SerializeField] GameObject[] PlayerGraphicCards;
    [SerializeField] GameObject Canvas;

    public PlayerData playerData2;
    public PlayerNetwork playerNetworkData2;

    public List<GameObject> Player2 = new List<GameObject>();
    // Creates network variable for the player data with readable everyone and Only server and Owner Can Edit
    public NetworkVariable<PlayerData> NetworkPlayerData = new NetworkVariable<PlayerData>(
   new PlayerData
   {
       Energy = 3,
       Health = 100,
       GameState = 0,
       Block = 0,
       TurnPoints = 0

   }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);



    // Player Data Networking Stores All Major Stats 
    public struct PlayerData : INetworkSerializable
    {
        public int Energy;
        public ulong Id;
        public int Health;

        public int GameState;

        public int Block;
        public int TurnPoints;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Energy);
            serializer.SerializeValue(ref Health);
            serializer.SerializeValue(ref GameState);
            serializer.SerializeValue(ref Block);
            serializer.SerializeValue(ref TurnPoints);
            serializer.SerializeValue(ref Id);
        }
    }
    //Cringe Network Update
    public override void OnNetworkSpawn()
    {

        //Update Player Data Networking
        if (IsOwner)
        {

            if (IsClient)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            }

            try
            {
                Debug.Log("Client Init ");
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (go.Equals(this.gameObject))
                        continue;
                    Player2.Add(go);

                }
                 playerNetworkData2 = Player2[0].GetComponent<PlayerNetwork>();

                NetworkPlayerData.Value = new PlayerData()
                {
                    Id = OwnerClientId,
                    Health = NetworkPlayerData.Value.Health,
                    GameState = NetworkPlayerData.Value.GameState,
                    Energy = NetworkPlayerData.Value.Energy,
                    Block = NetworkPlayerData.Value.Block,
                    TurnPoints = NetworkPlayerData.Value.TurnPoints

                };

                playerData2 = new PlayerData()
                {
                    Id = playerNetworkData2.OwnerClientId,
                    Health = playerNetworkData2.NetworkPlayerData.Value.Health,
                    GameState = playerNetworkData2.NetworkPlayerData.Value.GameState,
                    Energy = playerNetworkData2.NetworkPlayerData.Value.Energy,
                    Block = playerNetworkData2.NetworkPlayerData.Value.Block,
                    TurnPoints = playerNetworkData2.NetworkPlayerData.Value.TurnPoints
                };
            }
            catch (Exception e) { Debug.Log(e); }

            NetworkPlayerData.OnValueChanged += (PlayerData oldValue, PlayerData newValue) =>
            {
                //ClientInitServerRpc();  

                //Todo Do all of the Player Update Stuff In here Please
                Debug.Log(OwnerClientId + "GameState: " + newValue.GameState + " Health: " + newValue.Health);
                NetworkPlayerData.Value = new PlayerData()
                {
                    Id = OwnerClientId,
                    Health = newValue.Health,
                    GameState = newValue.GameState,
                    Energy = newValue.Energy,
                    Block = newValue.Block,
                    TurnPoints = newValue.TurnPoints

                };

            };

            //Update Player Card Graphics on Player Init
            PlayerGraphicCards[0].GetComponent<cardDisplay>().UpdateCardDisplay();
            PlayerGraphicCards[1].GetComponent<cardDisplay>().UpdateCardDisplay();
            PlayerGraphicCards[2].GetComponent<cardDisplay>().UpdateCardDisplay();

        }

    }

    private void OnClientConnected(ulong clientId)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("Client connected. You can now call ServerRpc methods.");
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (go.Equals(this.gameObject))
                    continue;
                Player2.Add(go);

            }
            try
            {
                 playerNetworkData2 = Player2[0].GetComponent<PlayerNetwork>();

                NetworkPlayerData.Value = new PlayerData()
                {
                    Id = OwnerClientId,
                    Health = NetworkPlayerData.Value.Health,
                    GameState = NetworkPlayerData.Value.GameState,
                    Energy = NetworkPlayerData.Value.Energy,
                    Block = NetworkPlayerData.Value.Block,
                    TurnPoints = NetworkPlayerData.Value.TurnPoints

                };

                playerData2 = new PlayerData()
                {
                    Id = playerNetworkData2.OwnerClientId,
                    Health = playerNetworkData2.NetworkPlayerData.Value.Health,
                    GameState = playerNetworkData2.NetworkPlayerData.Value.GameState,
                    Energy = playerNetworkData2.NetworkPlayerData.Value.Energy,
                    Block = playerNetworkData2.NetworkPlayerData.Value.Block,
                    TurnPoints = playerNetworkData2.NetworkPlayerData.Value.TurnPoints
                };
            }
            catch (Exception e) { Debug.Log(e); }
            // Call your ServerRpc method here or trigger an event to notify other scripts

        }
    }




    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) { Canvas.SetActive(false); }
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            NetworkPlayerData.Value = new PlayerData
            {
                Id = OwnerClientId,
                Energy = 3,
                Health = UnityEngine.Random.Range(0, 199),
                GameState = 0,
                Block = 0,
                TurnPoints = 0

            };
            var Player2Network = Player2[0].GetComponent<PlayerNetwork>();
            Debug.Log(OwnerClientId + "Health: " + NetworkPlayerData.Value.Health);
            Debug.Log(playerNetworkData2.NetworkPlayerData.Value.Id + "Health: " + playerNetworkData2.NetworkPlayerData.Value.Health);
        }



    }
    //! Use this function to Test if the player Is able to Talk to the server
    // [ServerRpc]
    // public void HelloWorldServerRpc(){
    //     Debug.Log("HelloWorldServerRpc; Owner:"+ OwnerClientId );
    // }

    //!The Following Functions are hacky Client functions that allow me to talk to the Player Manager Please Forgive Me for my Sins!




}

