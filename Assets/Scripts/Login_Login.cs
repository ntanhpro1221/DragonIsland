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
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using UnityEngine.UI;

public class Login_Login : MonoBehaviour
{
    ////Initialize
    //private List<AsyncOperation> TaskStack = new List<AsyncOperation>();//???????

    //string IdToken;
    //Text
    ////Coming soon: sign in with account
    //private void Start()
    //{
    //    UnityServices.InitializeAsync().ContinueWith(task =>
    //    {
    //        AuthenticationService.Instance.SignedIn += () =>
    //        {
    //            Debug.Log("Signed in with id: " + AuthenticationService.Instance.PlayerId);
    //        };
    //    });

    //    var config = new PlayGamesClientConfiguration.Builder().RequestIdToken().Build();
    //    PlayGamesPlatform.InitializeInstance(config);
    //    PlayGamesPlatform.DebugLogEnabled = true;
    //    PlayGamesPlatform.Activate();
    //    Social.localUser.Authenticate(success =>
    //    {
    //        if (success)
    //            Debug.Log("Login with google done. IdToken: " + ((PlayGamesLocalUser)Social.localUser).GetIdToken());
    //        else
    //            Debug.LogError("Login google is Unsuccessful");
    //    });
    //    IdToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
    //}

    //[Command]
    //async void SignInGoogle()
    //{
    //    try
    //    {
    //        await AuthenticationService.Instance.SignInWithGoogleAsync(IdToken);
    //        Debug.Log("Sign in is successful");
    //    }
    //    catch (AuthenticationException e)
    //    {
    //        Debug.LogException(e);
    //        Debug.LogError("Sign in is unsuccessful");
    //    }
    //    catch (RequestFailedException e)
    //    {
    //        Debug.LogException(e);
    //        Debug.LogError("Sign in is unsuccessful");
    //    }
    //}


    //private PlayGamesClientConfiguration clientConfiguration;

    //private void Start()
    //{
    //    ConfigureGPGS();
    //    SignIntoGPGS(SignInInteractivity.CanPromptOnce, clientConfiguration);
    //}

    //public void ConfigureGPGS()
    //{
    //    clientConfiguration = new PlayGamesClientConfiguration.Builder().Build();
    //}

    //public void SignIntoGPGS(SignInInteractivity interactivity, PlayGamesClientConfiguration configuration)
    //{
    //    PlayGamesPlatform.InitializeInstance(configuration);
    //    PlayGamesPlatform.Activate();
    //    PlayGamesPlatform.Instance.Authenticate(interactivity, code =>
    //    {
    //        Debug.Log("Authenticating...");
    //        if (code == SignInStatus.Success)
    //        {
    //            Debug.Log("Successfully Authenticated");
    //            Debug.Log("Hello" + Social.localUser.userName + " You have an ID of: " + Social.localUser.id);
    //        }
    //        else
    //        {
    //            Debug.LogError("Failed to authenticate due to: " + code);
    //        }
    //    });

    //}

    //[Command]
    //public void SignInBtn()
    //{
    //    SignIntoGPGS(SignInInteractivity.CanPromptAlways, clientConfiguration);
    //}

    //[Command]
    //public void SignOutBtn()
    //{
    //    PlayGamesPlatform.Instance.SignOut();
    //}






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
