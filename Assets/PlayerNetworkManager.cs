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
    public override void OnNetworkSpawn(){
        //Kills Any Instance of PlayerNetworkManager thats not the current
        if(instance != null && instance != this){Destroy(this);} 
        // Find the players then split them from each script
        Players = GameObject.FindGameObjectsWithTag("Player");
        PlayerNetwork hostPlayer = Players[0].GetComponent<PlayerNetwork>();
        PlayerNetwork clientPlayer = Players[1].GetComponent<PlayerNetwork>();
        //Cursed Player Data Swapping Test
        NetworkPlayerTurn.OnValueChanged += (bool wasTurn, bool IsTurn)=>
        {
            if(IsTurn)
            {
                hostPlayer.NetworkPlayerData.Value;
            }
            else
            {
             
            }
        };
    }
    [ServerRpc]
    private void PlayerTurnServerRpc(bool IsTurn)
    {
        NetworkPlayerTurn.Value = IsTurn;
    }
}
