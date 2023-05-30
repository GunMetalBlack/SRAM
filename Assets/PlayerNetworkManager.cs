using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerNetworkManager : NetworkBehaviour
{
    //Uber Cursed Script Grab
    GameObject HostPlayer = GameObject.FindGameObjectWithTag("PlayerHost");
    private NetworkVariable<bool> NetworkPlayerTurn = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    public override void OnNetworkSpawn(){
        //Cursed Player Data Swapping Test
        NetworkPlayerTurn.OnValueChanged += (bool wasTurn, bool IsTurn)=>
        {
            if(IsTurn)
            {
                PlayerAttackClientRpc(new ClientRpcParams{Send = new ClientRpcSendParams{TargetClientIds = new List<ulong>{0}}});
                PlayerDefenseClientRpc(new ClientRpcParams{Send = new ClientRpcSendParams{TargetClientIds = new List<ulong>{1}}});
            }
            else
            {
                PlayerAttackClientRpc(new ClientRpcParams{Send = new ClientRpcSendParams{TargetClientIds = new List<ulong>{1}}});
                PlayerDefenseClientRpc(new ClientRpcParams{Send = new ClientRpcSendParams{TargetClientIds = new List<ulong>{0}}});
            }
        };
    }
    [ServerRpc]
    private void PlayerTurnServerRpc(bool IsTurn)
    {
        NetworkPlayerTurn.Value = IsTurn;
    }

    [ClientRpc]
    private void PlayerDefenseClientRpc(ClientRpcParams clientRpcParams)
    {
        attackDefendTextMesh.text = "Defend";
    }
    [ClientRpc]
    private void PlayerAttackClientRpc(ClientRpcParams clientRpcParams)
    {
        attackDefendTextMesh.text = "Attack";
    }
}
