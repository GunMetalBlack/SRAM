using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;

public class Relay : MonoBehaviour
{
    // Start is called before the first frame update
    private async void Start()
    {
       await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>{
            Debug.Log("Signed In" + AuthenticationService.Instance.PlayerId);
        };
       await AuthenticationService.Instance.SignInAnonymouslyAsync();

    }

    private void CreateRelay(){
        RelayService.Instance.CreateAllocationAsync();
    }

}
