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
using UnityEngine.SceneManagement;

public class Login_Login : MonoBehaviour
{
    //Initialize
    private List<AsyncOperation> TaskStack = new List<AsyncOperation>();//???????

    //Coming soon: sign in with account
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
            // Sign in Anonymously 
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        // set landscape mode to horizontal
        Screen.orientation = ScreenOrientation.LandscapeLeft;

    }















    //those codes below are for developer mode anh will be removed in future
    [Command]
    async void server(int serverSize)
    {
        try
        {
            // Clean up
            NetworkManager.Singleton.Shutdown();

            // Set server data
            // create allocation because we need it to create server data ( i dont know why but i dont care :v)
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(serverSize);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            //print join code to screen
            Destroy(PrintTool.spawnedObject);
            PrintTool.spawnedObject = PrintTool.std_cout("join Code:\n" + joinCode, Vector3.zero, Color.white);
            // create server data from allocation
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            // set server data
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            // Start server
            NetworkManager.Singleton.StartServer();
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

            // Change scene when connecting successfully
            NetworkManager.Singleton.OnClientConnectedCallback += (Id) =>
            {
                SceneManager.LoadScene("BaseScene");
            };

            // Start connect to server
            NetworkManager.Singleton.StartClient();

        }
        catch (RelayServiceException e)
        {
            print(e);
            print("Invalid join code or you was lost your connection");
        }
    }

    [Command]
    void stop()
    {
        NetworkManager.Singleton.Shutdown();
    }

}
