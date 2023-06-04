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

    bool Init = true;

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
        public bool endTurn;
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
            serializer.SerializeValue(ref endTurn);
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
                    TurnPoints = NetworkPlayerData.Value.TurnPoints,
                    endTurn = NetworkPlayerData.Value.endTurn

                };

                playerData2 = new PlayerData()
                {
                    Id = playerNetworkData2.OwnerClientId,
                    Health = playerNetworkData2.NetworkPlayerData.Value.Health,
                    GameState = playerNetworkData2.NetworkPlayerData.Value.GameState,
                    Energy = playerNetworkData2.NetworkPlayerData.Value.Energy,
                    Block = playerNetworkData2.NetworkPlayerData.Value.Block,
                    TurnPoints = playerNetworkData2.NetworkPlayerData.Value.TurnPoints,
                    endTurn = playerNetworkData2.NetworkPlayerData.Value.endTurn
                };
            }
            catch (Exception e) { Debug.Log(e); }

            NetworkPlayerData.OnValueChanged += (PlayerData oldValue, PlayerData newValue) =>
            {
                //ClientInitServerRpc();  

                //Todo Do all of the Player Update Stuff In here Please
                Debug.Log(OwnerClientId + "GameState: " + newValue.GameState + " Health: " + newValue.Health);


                if (newValue.GameState == 1)
                {
                    attackDefendTextMesh.text = "Attack";
                }
                else if (newValue.GameState == 2)
                {
                    attackDefendTextMesh.text = "Defense";
                }
                else if (newValue.GameState == 3)
                {
                    Debug.Log("Waiting");
                }

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
                    TurnPoints = NetworkPlayerData.Value.TurnPoints,
                    endTurn = NetworkPlayerData.Value.endTurn

                };

                playerData2 = new PlayerData()
                {
                    Id = playerNetworkData2.OwnerClientId,
                    Health = playerNetworkData2.NetworkPlayerData.Value.Health,
                    GameState = playerNetworkData2.NetworkPlayerData.Value.GameState,
                    Energy = playerNetworkData2.NetworkPlayerData.Value.Energy,
                    Block = playerNetworkData2.NetworkPlayerData.Value.Block,
                    TurnPoints = playerNetworkData2.NetworkPlayerData.Value.TurnPoints,
                    endTurn = playerNetworkData2.NetworkPlayerData.Value.endTurn
                };
            }
            catch (Exception e) { Debug.Log(e); }
            // Call your ServerRpc method here or trigger an event to notify other scripts

        }
    }

    public void endTurn()
    {
        NetworkPlayerData.Value = new PlayerData()
        {
            Id = OwnerClientId,
            Health = NetworkPlayerData.Value.Health,
            GameState = NetworkPlayerData.Value.GameState,
            Energy = NetworkPlayerData.Value.Energy,
            Block = NetworkPlayerData.Value.Block,
            TurnPoints = NetworkPlayerData.Value.TurnPoints,
            endTurn = true

        };
    }
    void InitPlayerTurns()
    {
        if (NetworkManager.Singleton.LocalClientId == 0 && Init == true)
        {
            NetworkPlayerData.Value = new PlayerData
            {
                Id = OwnerClientId,
                Health = NetworkPlayerData.Value.Health,
                GameState = 1,
                Energy = NetworkPlayerData.Value.Energy,
                Block = NetworkPlayerData.Value.Block,
                TurnPoints = NetworkPlayerData.Value.TurnPoints,
                endTurn = false

            };
            Init = false;
        }
        if (NetworkManager.Singleton.LocalClientId == 1 && Init == true)
        {
            NetworkPlayerData.Value = new PlayerData
            {
                Id = OwnerClientId,
                Health = NetworkPlayerData.Value.Health,
                GameState = 2,
                Energy = NetworkPlayerData.Value.Energy,
                Block = NetworkPlayerData.Value.Block,
                TurnPoints = NetworkPlayerData.Value.TurnPoints,
                endTurn = false

            };
            Init = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) { Canvas.SetActive(false); }
        if (!IsOwner) return;
        playerNetworkData2 = Player2[0].GetComponent<PlayerNetwork>();
        InitPlayerTurns();
        if (NetworkPlayerData.Value.endTurn == true)
        {
            if (NetworkPlayerData.Value.endTurn == true && playerNetworkData2.NetworkPlayerData.Value.endTurn == true)
            {
                int changeGameState = -1;
                if (NetworkPlayerData.Value.GameState == 1)
                {
                    changeGameState = 2;
                }
                else if (NetworkPlayerData.Value.GameState == 2)
                {
                    changeGameState = 1;
                }
                Debug.Log("Player state changed");
                NetworkPlayerData.Value = new PlayerData()
                {
                    Id = OwnerClientId,
                    Health = NetworkPlayerData.Value.Health,
                    GameState = changeGameState,
                    Energy = NetworkPlayerData.Value.Energy,
                    Block = NetworkPlayerData.Value.Block,
                    TurnPoints = NetworkPlayerData.Value.TurnPoints,
                    endTurn = false

                };

            }
        }
        else
        {

        }

        Debug.Log("ID:" + OwnerClientId + "Health: " + NetworkPlayerData.Value.Health + "GameState: " + NetworkPlayerData.Value.GameState);
        Debug.Log("ID:" + playerNetworkData2.NetworkPlayerData.Value.Id + "Health: " + playerNetworkData2.NetworkPlayerData.Value.Health + "GameState:" + playerNetworkData2.NetworkPlayerData.Value.GameState);



    }
    //! Use this function to Test if the player Is able to Talk to the server
    // [ServerRpc]
    // public void HelloWorldServerRpc(){
    //     Debug.Log("HelloWorldServerRpc; Owner:"+ OwnerClientId );
    // }

    //!The Following Functions are hacky Client functions that allow me to talk to the Player Manager Please Forgive Me for my Sins!




}

