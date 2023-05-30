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
    private NetworkVariable<PlayerData> NetworkPlayerData = new NetworkVariable<PlayerData> (
    new PlayerData
    {
        Energy = 0,
        Health = 0,
        AttackDefendText = "",

    },NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    

    // Player Data Networking Stores All Major Stats 
    public struct PlayerData : INetworkSerializable
    {
        public int Energy;

        public int Health;

        public FixedString32Bytes AttackDefendText;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
           serializer.SerializeValue(ref Energy);
           serializer.SerializeValue(ref Health);
           serializer.SerializeValue(ref AttackDefendText);
        }
    }
    //Cringe Network Update
    public override void OnNetworkSpawn()
    {

        //Update Player Data Networking
        NetworkPlayerData.OnValueChanged += (PlayerData oldValue, PlayerData newValue) =>
        {
             Debug.Log(OwnerClientId + ": Old Network, Value: " +  "; " + oldValue.Energy + "; " + oldValue.Health);
            Debug.Log(OwnerClientId + ": Network, Value: " + "; " + newValue.Energy + "; " + newValue.Health);
        };
        
        //Cursed On Init Values
        if(IsOwner)
        {   
            //Default Client Values?
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
        // No Shame Please
        attackDefendTextMesh.text = NetworkPlayerData.Value.AttackDefendText.ToString();
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
    


}

