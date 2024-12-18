using QFSW.QC;
using System.Collections;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase;
using Google;
using UnityEngine.SceneManagement;
using Datas;
using Firebase.Database;

namespace Scenes.Login {
    public class Login : MonoBehaviour {
        /// <summary>
        /// Control how much load bar fill
        /// </summary>
        [SerializeField]
        private Image LoadBarImg;

        /// <summary>
        /// Load Base Scene async task. execute after user signed in
        /// </summary>
        private AsyncOperation sceneLoading;

        /// <summary>
        /// User Data Loading task. execute after user signed in
        /// </summary>
        private Task dynamicDataLoading;

        private async void Start() {
            try {
                print("UIManager.Instance.LoadScreen()");
                UIManager.Instance.LoadScreen();
                print("LoadBarImg.fillAmount = 0.2f");
                LoadBarImg.fillAmount = 0.2f;
                print("Task initFbTask = InitializeFirebaseAsync()");
                Task initFbTask = InitializeFirebaseAsync();
                print("InitializeGoogle()");
                InitializeGoogle();
                print("await initFbTask");
                await initFbTask;

                print("LoadBarImg.fillAmount = 0.4f");
                LoadBarImg.fillAmount = 0.4f;
                print("await DataManager.Instance.LoadStaticDataAsync();");
                await DataManager.Instance.LoadStaticDataAsync();

                print("LoadBarImg.fillAmount = 0.9f;");
                LoadBarImg.fillAmount = 0.9f;
                print("if (auth.CurrentUser == null) {");
                if (auth.CurrentUser == null) {
                    print("UIManager.Instance.LoginScreen();");
                    UIManager.Instance.LoginScreen();
                } else {
                    print("user = auth.CurrentUser;");
                    user = auth.CurrentUser;

                    print("avatarText.text = \"Continue with \" + user.DisplayName;");
                    avatarText.text = "Continue with " + user.DisplayName;
                    print("UIManager.Instance.ProfileMenuScreen();");
                    UIManager.Instance.ProfileMenuScreen();
                }
            } catch (System.Exception ex) {
                print("UIManager.Instance.VoidScreen();");
                UIManager.Instance.VoidScreen();
                print("Debug.LogError(ex);");
                Debug.LogError(ex);
                print("cout(ex.Message);");
                cout(ex.Message);
            }
        }


        private IEnumerator OnSignedInAsync() {
            //change to loading UI
            UIManager.Instance.LoadScreen();
            LoadBarImg.fillAmount = 0;

            //Load scene async
            sceneLoading = SceneManager.LoadSceneAsync("BaseScene");
            sceneLoading.allowSceneActivation = false;

            //
            DataManager.Instance.OnSignedIn();

            //Load user data async
            dynamicDataLoading = DataManager.Instance.LoadDynamicDataAsync();

            float asyncMethodCnt = 2f;
            //Wait for all async task completed except sceneloading
            while (!(dynamicDataLoading.IsCompleted)) {
                LoadBarImg.fillAmount = (sceneLoading.progress
                    + (dynamicDataLoading.IsCompleted ? 1f : 0f)
                    ) / asyncMethodCnt;
                yield return null;
            }

            //check for exception
            if (!dynamicDataLoading.IsCompletedSuccessfully) {
                cout(dynamicDataLoading.Exception.Message);
                print(dynamicDataLoading.Exception);
                yield break;
            }

            //wait for sceneloading task completed
            sceneLoading.allowSceneActivation = true;
            while (!sceneLoading.isDone) {
                LoadBarImg.fillAmount = (asyncMethodCnt - 1f + sceneLoading.progress) / asyncMethodCnt;
                yield return null;
            }
        }

        private void cout(string content, NGDS.Type type = NGDS.Type.Error) {
            NGDS.Notification.Instance.Notify(content, type);
        }

        #region FireBase Authentication
        [Space]
        [Header("Input")]
        [SerializeField]
        private TMP_InputField lEmailInp;
        [SerializeField]
        private TMP_InputField lPasswordInp;
        [SerializeField]
        private TMP_InputField rNameInp;
        [SerializeField]
        private TMP_InputField rEmailInp;
        [SerializeField]
        private TMP_InputField rPasswordInp;
        [SerializeField]
        private TMP_InputField rConfirmInp;

        [Space]
        [Header("Another things")]
        [SerializeField]
        private Sprite avatarImg;
        [SerializeField]
        private TextMeshProUGUI avatarText;

        private FirebaseAuth auth;
        private FirebaseUser user = null;

        private async Task InitializeFirebaseAsync() {
            Task<DependencyStatus> task = FirebaseApp.CheckAndFixDependenciesAsync();
            await task;
            DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus != DependencyStatus.Available) {
                Debug.LogError("Could not resolved all Firebase dependencies: " + dependencyStatus);
            } else {
                auth = FirebaseAuth.DefaultInstance;
            }
        }

        private IEnumerator RegisterWithEmailPasswordAsync() {
            if (rNameInp.text == "") {
                cout("Missing user name!", NGDS.Type.Error);
            } else if (rPasswordInp.text != rConfirmInp.text) {
                cout("Password does not match!", NGDS.Type.Error);
            } else {
                Task<AuthResult> task = auth.CreateUserWithEmailAndPasswordAsync(rEmailInp.text, rPasswordInp.text);
                yield return new WaitUntil(() => task.IsCompleted);

                if (task.Exception != null) //Handle if there're some exeption
                {
                    //Get exeption backend
                    Debug.LogWarning($"Failed to register due to: {task.Exception}");
                    FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                    //Show exeption to user
                    NGDS.Notification.Instance.Notify(
                    errorCode switch {
                        AuthError.MissingEmail => "Missing Email!",
                        AuthError.MissingPassword => "Missing Password!",
                        AuthError.WeakPassword => "Weak Password!",
                        AuthError.EmailAlreadyInUse => "Email already in use!",
                        _ => "Register failed due to: " + firebaseEx.Message,
                    },
                    NGDS.Type.Error);
                } else {
                    user = task.Result.User;
                    if (user != null) {
                        //Create a user profile with input name
                        UserProfile profile = new UserProfile { DisplayName = rNameInp.text };

                        //Update user name via user profile
                        Task profileTask = user.UpdateUserProfileAsync(profile);
                        yield return new WaitUntil(() => profileTask.IsCompleted);

                        if (profileTask.Exception != null) {
                            Debug.LogWarning($"Failed to set user name due to: " + profileTask.Exception);
                            cout("Failed to set user name!\n" + profileTask.Exception.Message, NGDS.Type.Error);
                        } else {
                            StartCoroutine(OnSignedInAsync());
                        }
                    } else {
                        Debug.LogError("I just show this case because the tutorial that i followed does it.\n" +
                            "I dont think that this case will occur but anyway\n" +
                            "Just fix it, good luck");
                    }
                }
            }
        }

        private IEnumerator SignInWithEmailPasswordAsync() {
            Task<AuthResult> task = auth.SignInWithEmailAndPasswordAsync(lEmailInp.text, lPasswordInp.text);
            yield return new WaitUntil(() => task.IsCompleted);

            if (task.Exception != null)// Handle if there are some exception
            {
                //Get exception backend
                Debug.LogWarning($"Failed to sign in due to: {task.Exception}");
                FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                //Show exception to user
                NGDS.Notification.Instance.Notify(
                errorCode switch {
                    AuthError.MissingEmail => "Missing Email!",
                    AuthError.MissingPassword => "Missing Password!",
                    AuthError.WrongPassword => "wrong Password!",
                    AuthError.InvalidEmail => "Invalid Email!",
                    AuthError.UserNotFound => "This account does not exist!",
                    _ => "Failed to signed in due to: " + firebaseEx.Message,
                },
                NGDS.Type.Error);
            } else {
                user = task.Result.User;
                StartCoroutine(OnSignedInAsync());
            }
        }

        private IEnumerator SignInWithGoogleAsync() {
            //Sign out before sign in to ensure that user can choose their google account again
            Task taskSignOut = SignOutGoogle();
            while (!taskSignOut.IsCompleted) yield return null;

            //Sign in
            Task<GoogleSignInUser> taskAuthGG = GoogleSignIn.DefaultInstance.SignIn();
            while (!taskAuthGG.IsCompleted) yield return null;
            if (taskAuthGG.IsCanceled) {
                cout("Sign in Google was canceled!");
            } else if (taskAuthGG.IsFaulted) {
                cout("Sign in Google was faulted!\n" + taskAuthGG.Exception.Message);
                Debug.Log("Sign in Google was faulted!\n" + taskAuthGG.Exception);
            } else {
                Credential credential = GoogleAuthProvider.GetCredential(taskAuthGG.Result.IdToken, null);
                Task<FirebaseUser> taskAuthFb = auth.SignInWithCredentialAsync(credential);
                yield return new WaitUntil(() => taskAuthFb.IsCompleted);
                if (taskAuthFb.IsCanceled) {
                    cout("Sign in with Google was canceled!");
                } else if (taskAuthFb.IsFaulted) {
                    cout("Sign in with Google was faulted!\n" + taskAuthFb.Exception.Message);
                    Debug.Log("Sign in with Google was faulted!\n" + taskAuthFb.Exception.Message);
                } else {
                    user = taskAuthFb.Result;
                    StartCoroutine(OnSignedInAsync());
                }
            }
        }

        private IEnumerator SignInWithFacebookAsync() {
            yield return null;
        }

        private IEnumerator SignOut() {
            Task taskSignOutGG = SignOutGoogle();
            auth.SignOut();
            while (!taskSignOutGG.IsCompleted) yield return null;
            UIManager.Instance.LoginScreen();
        }

        #region test
        //public void CheckExistUser() {
        //    if (auth.CurrentUser != null) {
        //        user = auth.CurrentUser;
        //        StartCoroutine(OnSignedInAsync());
        //    } else {
        //        cout("Not exist any user");
        //    }
        //}

        //public void SignInWithGooglesilently() {
        //    StartCoroutine(SignInWithGooglesilentlyAsync());
        //}

        //private IEnumerator SignInWithGooglesilentlyAsync() {
        //    Task<GoogleSignInUser> taskAuthGG = GoogleSignIn.DefaultInstance.SignInSilently();
        //    yield return new WaitUntil(() => taskAuthGG.IsCompleted);
        //    if (taskAuthGG.IsCanceled) {
        //        cout("Sign in Google was canceled!");
        //    } else if (taskAuthGG.IsFaulted) {
        //        cout("Sign in Google was faulted!\n" + taskAuthGG.Exception.Message);
        //        Debug.Log("Sign in Google was faulted!\n" + taskAuthGG.Exception);
        //    } else {
        //        Credential credential = GoogleAuthProvider.GetCredential(taskAuthGG.Result.IdToken, null);
        //        Task<FirebaseUser> taskAuthFb = auth.SignInWithCredentialAsync(credential);
        //        yield return new WaitUntil(() => taskAuthFb.IsCompleted);
        //        if (taskAuthFb.IsCanceled) {
        //            cout("Sign in with Google was canceled!");
        //        } else if (taskAuthFb.IsFaulted) {
        //            cout("Sign in with Google was faulted!\n" + taskAuthFb.Exception.Message);
        //            Debug.Log("Sign in with Google was faulted!\n" + taskAuthFb.Exception.Message);
        //        } else {
        //            user = taskAuthFb.Result;
        //            StartCoroutine(OnSignedInAsync());
        //        }
        //    }
        //}

        //public void GoogleSignOut() {
        //    GoogleSignIn.DefaultInstance.SignOut();
        //}

        //public void GoogleDisconnect() {
        //    GoogleSignIn.DefaultInstance.Disconnect();
        //}

        //public void FirebaseSignOut() {
        //    auth.SignOut();
        //}

        #endregion

        public void OnClickavatar() {
            StartCoroutine(OnSignedInAsync());
        }

        public void OnClickRegister() {
            StartCoroutine(RegisterWithEmailPasswordAsync());
        }

        public void OnClickLogin() {
            StartCoroutine(SignInWithEmailPasswordAsync());
        }

        public void OnClickGoogle() {
            StartCoroutine(SignInWithGoogleAsync());
        }

        public void OnClickFacebook() {
            StartCoroutine(SignInWithFacebookAsync());
        }

        public void OnclickSignOut() {
            StartCoroutine(SignOut());
        }

        #endregion

#if UNITY_ANDROID
        #region Google
        private const string GoogleWebAPI = "503520547653-tj9fhj9aif1lmvji5olp0f89pu62hcch.apps.googleusercontent.com";
        private void InitializeGoogle() {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration {
                WebClientId = GoogleWebAPI,
                RequestIdToken = true,
                //RequestEmail = true,
                RequestProfile = true,
                //RequestAuthCode = true,
            };
        }

        private async Task SignOutGoogle() {
            await GoogleSignIn.DefaultInstance.SignInSilently();
            GoogleSignIn.DefaultInstance.SignOut();
        }
        #endregion

        //#region Facebook
        //private void InitializeFB() {
        //    if (!FB.IsInitialized) {
        //        FB.Init(
        //            () => {
        //                if (FB.IsInitialized) {
        //                    Debug.Log("Initialize the Facebook SDK successfuly");
        //                    FB.ActivateApp();
        //                } else {
        //                    Debug.LogError("Failed to initialize the Facebook SDK");
        //                }
        //            },
        //            isGameShow => {
        //                if (!isGameShow) {
        //                    Debug.Log("We're going to sign into facebook ;)");
        //                    Time.timeScale = 0;
        //                } else {
        //                    Debug.Log("We're back!");
        //                    Time.timeScale = 1;
        //                }
        //            }
        //            );
        //    } else {
        //        FB.ActivateApp();
        //    }
        //}
        //private void SignInFB() {
        //    if (FB.IsInitialized) {
        //        Debug.Log("FB SDK already initialized");
        //    } else {
        //        Debug.Log("FB SDK have not initialized, but I don't fucking care at all and we still sign into facebook ;)");
        //    }
        //    Debug.Log("Signing into facebook...");
        //    var perms = new List<string>() { "public_profile", "email", "user_friends" };
        //    FB.LogInWithReadPermissions(perms, result => {
        //        if (FB.IsLoggedIn) {
        //            Debug.Log("Sign into facebook SUCCESSFULY!!!");
        //            var aToken = AccessToken.CurrentAccessToken;
        //            //IdTokenFB = aToken.TokenString;
        //            Debug.Log(aToken.UserId);
        //            foreach (string perm in aToken.Permissions) {
        //                Debug.Log(perm);
        //            }
        //        } else {
        //            Debug.Log("[Facebook login] User cancelled login");
        //        }
        //    });
        //}
        //private void SignOutFB() {
        //    Debug.Log("Signing out...");
        //    FB.LogOut();
        //    Debug.Log("You already signed out Facebook");
        //}
        //#endregion
#endif

        #region Client - Server 
        #region Deverloper
        [Command]
        private void ChangeConnectionData(string ServerIpAddress, ushort ServerPort) {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ServerIpAddress, ServerPort);
            Debug.Log("Connection data changed to:\n" +
                NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address + "\n" +
                NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port);
        }
        #endregion
        /// <summary>
        /// Remember to check "Allow remote connection" box in inspector for server build
        /// </summary>
        [Command]
        private void StartServer() {
            NetworkManager.Singleton.StartServer();
        }

        /// <summary>
        /// Remember to uncheck "Allow remote connection" box in inspector for client build ( for the reason that i don't know yet:v)
        /// </summary>
        [Command]
        private void StartClient() {
            NetworkManager.Singleton.StartClient();
        }

        [Command]
        void stop() {
            NetworkManager.Singleton.Shutdown();
        }
        #endregion
    }
}
