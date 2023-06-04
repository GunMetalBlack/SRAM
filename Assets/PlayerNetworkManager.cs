using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerNetworkManager : NetworkBehaviour
{
    public static PlayerNetworkManager instance;
    //Uber Cursed Script Grab

    bool Init = true;
  
    public override void OnNetworkSpawn()
    {
        //Kills Any Instance of PlayerNetworkManager thats not the current
        if (instance != null && instance != this) { Destroy(this); }
        //Cursed Init Host Game State
        if(NetworkManager.IsHost)
        {
           // HostInitTurnServerRpc();
        }
        //Sets the current Host and Game State
      
    }

    void Update()
    {

        
    }
    

    // [ServerRpc]
    // public void PlayerUpdateTurnServerRpc()
    // {
    //     //Grabs the player instance and player data from the server
    //         var client = NetworkManager.Singleton.ConnectedClients[1];
    //         var clientPlayerNetwork = client.PlayerObject.GetComponent<PlayerNetwork>();
    //         var host = NetworkManager.Singleton.ConnectedClients[0];
    //         var hostPlayerNetwork = host.PlayerObject.GetComponent<PlayerNetwork>();
    //     //Will wait until both players are ready to switch turns
    //     while(clientPlayerNetwork.NetworkPlayerData.Value.GameState != 3 || hostPlayerNetwork.NetworkPlayerData.Value.GameState != 3 ){}
    //     //Switches Players Turn
    //     //* Do you can do Card Switch Here Or do it client side you Mango
    //     if(clientPlayerNetwork.NetworkPlayerData.Value.GameState == 3 || hostPlayerNetwork.NetworkPlayerData.Value.GameState == 3)
    //     {
    //         if(hostPlayerNetwork.NetworkPlayerData.Value.GameState == 0 && clientPlayerNetwork.NetworkPlayerData.Value.GameState == 1)
    //         {
    //             clientPlayerNetwork.setGameState(0);
    //             hostPlayerNetwork.setGameState(1);
    //         }
    //         else if(hostPlayerNetwork.NetworkPlayerData.Value.GameState == 1 && clientPlayerNetwork.NetworkPlayerData.Value.GameState == 0)
    //         {
    //             clientPlayerNetwork.setGameState(1);
    //             hostPlayerNetwork.setGameState(0);
    //         }
    //     }
    // }

    // [ServerRpc]
    // public void HostInitTurnServerRpc()
    // {
    //     var host = NetworkManager.Singleton.ConnectedClients[0];
    //     var hostPlayerNetwork = host.PlayerObject.GetComponent<PlayerNetwork>();
    //     if (NetworkManager.Singleton.ConnectedClients.ContainsKey(0) && Init == true) { hostPlayerNetwork.setGameState(1); Init = false; }
    //     // Find the players then split them from each script
    // }
}

