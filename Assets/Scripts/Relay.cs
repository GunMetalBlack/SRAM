using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
public class Relay : MonoBehaviour
{
    [SerializeField] GameObject NetUI;
    [SerializeField] TextMeshProUGUI joinCodeText;
     [SerializeField] TextMeshProUGUI joinCodeTextInput;
    public string JoinCodeFromInput;
    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed In" + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

    }

    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            joinCodeText.text = "JoinCode:"+ joinCode;
            Debug.Log("Join Code: " + joinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
               allocation.RelayServer.IpV4,
               (ushort)allocation.RelayServer.Port,
               allocation.AllocationIdBytes,
               allocation.Key,
               allocation.ConnectionData,
               true
            );
            NetworkManager.Singleton.StartHost();
            NetUI.SetActive(false);
        }
        catch (RelayServiceException e) { Debug.Log(e); }
       
       
    }
    public void setJoinCode(){
        JoinCodeFromInput = joinCodeTextInput.text;
        Debug.Log("Join Code: " + JoinCodeFromInput);
    }
    public async void JoinRelay()
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(JoinCodeFromInput);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
            );
            NetworkManager.Singleton.StartClient();
            NetUI.SetActive(false);
        }
        catch (RelayServiceException e) { Debug.Log(e);  }
    }

}
