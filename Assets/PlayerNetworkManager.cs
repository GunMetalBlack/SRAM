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
            PlayerUpdateTurnServerRpc();
        }
        //Sets the current Host and Game State
        PlayerUpdateTurnClientRpc();
    }

    [ClientRpc]
    public void PlayerUpdateTurnClientRpc()
    {
        if(NetworkManager.ConnectedClients.ContainsKey(1))
        {
            var client = NetworkManager.ConnectedClients[1];
            var clientPlayerNetwork = client.PlayerObject.GetComponent<PlayerNetwork>();
            
            var host = NetworkManager.ConnectedClients[0];
            var hostPlayerNetwork = host.PlayerObject.GetComponent<PlayerNetwork>();
            if(hostPlayerNetwork.NetworkPlayerData.Value.GameState == 0)
            {
                clientPlayerNetwork.setGameState(1);
            }
            else if(hostPlayerNetwork.NetworkPlayerData.Value.GameState == 1)
            {
                clientPlayerNetwork.setGameState(0);
            }
        }
    }

    [ServerRpc]
    public void PlayerUpdateTurnServerRpc()
    {
        var host = NetworkManager.ConnectedClients[0];
        var hostPlayerNetwork = host.PlayerObject.GetComponent<PlayerNetwork>();
        if (NetworkManager.ConnectedClients.ContainsKey(0) && Init == true) { hostPlayerNetwork.setGameState(1); Init = false; }
        // Find the players then split them from each script
    }
}

