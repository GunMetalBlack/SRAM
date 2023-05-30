using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
public class PlayerNetwork : NetworkBehaviour
{
    //Some Scetch Vars For UI
    TextMeshProUGUI attackDefendTextMesh;
    // Creates network variable for the player data with readable everyone and Only server and Owner Can Edit
    private NetworkVariable<PlayerData> NetworkPlayerData = new NetworkVariable<PlayerData> (
    new PlayerData
    {
        Energy = 0,
        Health = 0,

    },NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    private NetworkVariable<bool> NetworkPlayerTurn = new NetworkVariable<bool>(true,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    // Player Data Networking Stores All Major Stats 
    public struct PlayerData : INetworkSerializable
    {
        public int Energy;

        public int Health;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
           serializer.SerializeValue(ref Energy);
           serializer.SerializeValue(ref Health);
        }
    }
    //Cringe Network Update
    public override void OnNetworkSpawn()
    {
        NetworkPlayerData.OnValueChanged += (PlayerData oldValue, PlayerData newValue) =>
        {
             Debug.Log(OwnerClientId + ": Old Network, Value: " +  "; " + oldValue.Energy + "; " + oldValue.Health);
            Debug.Log(OwnerClientId + ": Network, Value: " + "; " + newValue.Energy + "; " + newValue.Health);
        };
        NetworkPlayerTurn.OnValueChanged += (bool wasTurn, bool IsTurn)=>
        {
            if(IsTurn == true)
            {
                attackDefendTextMesh.text = "Attack";
            }
            else
            {
                attackDefendTextMesh.text = "Defend";
            }
        };
        //Cursed On Init Values
        if(IsOwner)
        {
            //Default Network Values?
            NetworkPlayerData.Value = new PlayerData
            {
               Energy = 20,
               Health = 100,
            };
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        //Test Code For Update Network Player Data
        if(Input.GetKeyDown(KeyCode.T))
        {
            NetworkPlayerData.Value = new PlayerData
            {
               Energy = 20,
               Health = 100,
            };
        }
        
    }
    [ServerRpc]
    private void PlayerTurnServerRpc(bool IsTurn)
    {
        NetworkPlayerTurn.Value = IsTurn;
    }
}

