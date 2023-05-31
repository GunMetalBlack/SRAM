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
    // Creates network variable for the player data with readable everyone and Only server and Owner Can Edit
     public NetworkVariable<PlayerData> NetworkPlayerData = new NetworkVariable<PlayerData> (
    new PlayerData
    {
        Energy = 0,
        Health = 0,
        GameState = 0,

    },NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    

    // Player Data Networking Stores All Major Stats 
    public struct PlayerData : INetworkSerializable
    {
        public int Energy;

        public int Health;

        public int GameState;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
           serializer.SerializeValue(ref Energy);
           serializer.SerializeValue(ref Health);
           serializer.SerializeValue(ref GameState);
        }
    }
    //Cringe Network Update
    public override void OnNetworkSpawn()
    {
 
        //Update Player Data Networking
        NetworkPlayerData.OnValueChanged += (PlayerData oldValue, PlayerData newValue) =>
        {
             //Todo Do all of the Player Update Stuff In here Please
             Debug.Log(OwnerClientId + ": Network, Value: " + "; " + newValue.Energy + "; " + newValue.Health);
             if(newValue.GameState == 1)
             {
                attackDefendTextMesh.text = "Attack";
             }else
             {
                attackDefendTextMesh.text = "Defense";
             }

        };
        
        //Cursed On Init Values
        if(IsOwner)
        {   
            //Default Client Values?
            NetworkPlayerData.Value = new PlayerData
            {
               Energy = 20,
               Health = 100,
               GameState = 0,
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        //Test Code For client to Network Player Data
        if(Input.GetKeyDown(KeyCode.T))
        {
            NetworkPlayerData.Value = new PlayerData
            {
               Energy = 20,
               Health = 100,
            };
        }
        
    }
    
    public void setGameState(int state)
    {
        if(!IsOwner) return;
         NetworkPlayerData.Value = new PlayerData{GameState = state,};
    }

}

