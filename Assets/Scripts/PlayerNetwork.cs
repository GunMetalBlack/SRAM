using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;

public class PlayerNetwork : NetworkBehaviour
{
    //Some Scetch Vars For UI
    [SerializeField] TextMeshProUGUI attackDefendTextMesh;
    [SerializeField] GameObject[] PlayerGraphicCards;
    [SerializeField] GameObject Canvas;
    // Creates network variable for the player data with readable everyone and Only server and Owner Can Edit
    public NetworkVariable<PlayerData> NetworkPlayerData = new NetworkVariable<PlayerData>(
   new PlayerData
   {
       Energy = 0,
       Health = 0,
       GameState = 0,

   }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);



    // Player Data Networking Stores All Major Stats 
    public struct PlayerData : INetworkSerializable
    {
        public int Energy;

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
        }
    }
    //Cringe Network Update
    public override void OnNetworkSpawn()
    {   
        //Update Player Data Networking
        if (IsOwner)
        {
            NetworkPlayerData.OnValueChanged += (PlayerData oldValue, PlayerData newValue) =>
            {
                 //ClientInitServerRpc();  
                //Todo Do all of the Player Update Stuff In here Please
                Debug.Log(OwnerClientId + "GameState: " + newValue.GameState);
                Debug.Log(OwnerClientId + "Host CV");
                if (newValue.GameState == 1)
                {
                    attackDefendTextMesh.text = "Attack";
                }
                else if(newValue.GameState == 2)
                {
                    attackDefendTextMesh.text = "Defense";
                }else if(newValue.GameState == 3)
                {
                    Debug.Log("Waiting");
                }
                
            };
            
            //Update Player Card Graphics on Player Init
            PlayerGraphicCards[0].GetComponent<cardDisplay>().UpdateCardDisplay();
            PlayerGraphicCards[1].GetComponent<cardDisplay>().UpdateCardDisplay();
            PlayerGraphicCards[2].GetComponent<cardDisplay>().UpdateCardDisplay();

            //Default Client Values?
            ClientInitServerRpc();
        }
                
    }
    [ServerRpc]
    public void changeTurnServerRpc()
    {
        if(IsHost)
        {
            Debug.Log("TurnServerRpc" + NetworkPlayerData.Value.GameState);
            if(NetworkPlayerData.Value.GameState == 1)
            {
                NetworkPlayerData.Value = new PlayerData { GameState = 2};
                ClientInitTurnClientRpc(1);
            }else if(NetworkPlayerData.Value.GameState == 2)
            {
                NetworkPlayerData.Value = new PlayerData { GameState = 1};
                ClientInitTurnClientRpc(2);
            }
        }

    }


    [ClientRpc]
    public void ClientInitTurnClientRpc(int setGameState)
    {
        if(!IsOwner){return;}
        NetworkPlayerData.Value = new PlayerData { GameState = setGameState};
    }
     [ServerRpc]
     public void ClientInitServerRpc()
     {
        Debug.Log("Well Piss");
            var host = NetworkManager.Singleton.ConnectedClients[0];
            var hostPlayerNetwork = host.PlayerObject.GetComponent<PlayerNetwork>();
            if(hostPlayerNetwork.NetworkPlayerData.Value.GameState == 1)
            {
                ClientInitTurnClientRpc(2);
            }
     }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) { Canvas.SetActive(false); }
        if (!IsOwner) return;


    }
    //! Use this function to Test if the player Is able to Talk to the server
    // [ServerRpc]
    // public void HelloWorldServerRpc(){
    //     Debug.Log("HelloWorldServerRpc; Owner:"+ OwnerClientId );
    // }

    //!The Following Functions are hacky Client functions that allow me to talk to the Player Manager Please Forgive Me for my Sins!
 

    public void setGameState(int state)
    {

            NetworkPlayerData.Value = new PlayerData { GameState = state, };
  
    }

}

