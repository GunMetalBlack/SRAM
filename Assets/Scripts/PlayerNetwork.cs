using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerNetwork : NetworkBehaviour
{
// Creates network variable for the player data with readable everyone and Only server and Owner Can Edit
    private NetworkVariable<PlayerData> NetworkPlayerData = new NetworkVariable<PlayerData> (
    new PlayerData
    {
        IsPlayerTurn = true,
        Energy = 30,

    },NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    // Player Data Networking Stores All Major Stats 
    public struct PlayerData : INetworkSerializable
    {
        public bool IsPlayerTurn;
        public int Energy;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
           serializer.SerializeValue(ref IsPlayerTurn);
           serializer.SerializeValue(ref Energy);
        }
    }
    //Cringe Network Update
    public override void OnNetworkSpawn()
    {
        NetworkPlayerData.OnValueChanged += (PlayerData oldValue, PlayerData newValue) =>
        {
            Debug.Log(OwnerClientId + ": Network, Value: " + newValue.IsPlayerTurn + "; " + newValue.Energy);
        };
    }


    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        if(Input.GetKeyDown(KeyCode.T))
        {
            NetworkPlayerData.Value = new PlayerData
            {
               IsPlayerTurn = false,
               Energy = 20,

            };
        }
        
    }
}

