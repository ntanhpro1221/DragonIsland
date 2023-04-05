using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class Login : MonoBehaviour
{
    private async void Start()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            // Initialize to use its function
            await UnityServices.InitializeAsync();
            AuthenticationService.Instance.SignedIn += () =>
            {
                print("Signed in with id: " + AuthenticationService.Instance.PlayerId);
            };
            // Sign in Anonymously (sign in with account function will be add in not far feature ;)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
    [Command]
    async void host(int serverSize)
    {
        try
        {
            // Clean up
            NetworkManager.Singleton.Shutdown();

            // Set server data
            // create allocation because we need it to create server data ( i dont know why but i dont care :v)
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(serverSize);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            print("join code: " + joinCode);
            // create server data from allocation
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            // set server data
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            // Start server
            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            print(e);
        }
    }

    [Command]
    async void client(string joinCode)
    {
        try
        {
            // Clean up
            NetworkManager.Singleton.Shutdown();

            // Set server data
            // get allocation from joinCode
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            // create server data from allocation
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            // set server data
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            // Start connect to server
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            print(e);
            print("Invalid join code or you was lost your connection");
        }
        NetworkManager.Singleton.StartClient();
    }

    [Command]
    void stop()
    {
        NetworkManager.Singleton.Shutdown();
    }
}
