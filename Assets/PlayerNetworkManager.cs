using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerNetworkManager : NetworkBehaviour
{
    public GameObject[] Players;
    public static PlayerNetworkManager instance;
    //Uber Cursed Script Grab
    private void Update()
    {
       if(Players.Length < 2){Players = GameObject.FindGameObjectsWithTag("Player");}
    }
    private NetworkVariable<bool> NetworkPlayerTurn = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    public override void OnNetworkSpawn()
    {
        //Kills Any Instance of PlayerNetworkManager thats not the current
        if(instance != null && instance != this){Destroy(this);} 
        // Find the players then split them from each script
        Players = GameObject.FindGameObjectsWithTag("Player");
        if(Players.Length == 1)
        {
            var client = NetworkManager.ConnectedClients[0];
            var host = client.PlayerObject.GetComponent<PlayerNetwork>();
            PlayerTurnServerRpc(true);
            //Cursed Player Data Swapping Test
            NetworkPlayerTurn.OnValueChanged += (bool wasTurn, bool IsTurn)=>
            {
                if(IsTurn)
                {
                    Debug.Log("Test player ");
                    host.PlayerDataSet("Attack");
                   
                }
                else
                {
                    host.PlayerDataSet("Defense");
                    
                }
            };
        }
    }
    [ServerRpc]
    public void PlayerTurnServerRpc(bool IsTurn)
    {
        NetworkPlayerTurn.Value = IsTurn;
    }
}
