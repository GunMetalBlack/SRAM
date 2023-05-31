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
    private void Update()
    {


    }
    public override void OnNetworkSpawn()
    {
        //Kills Any Instance of PlayerNetworkManager thats not the current
        if (instance != null && instance != this) { Destroy(this); }
        //Cursed Player Data Swapping Test
        PlayerUpdateTurnServerRpc();
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

