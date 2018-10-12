

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.UI;

    // Handler for UI buttons on the scene.  Also performs some
    // necessary setup (initializing the firebase app, etc) on
    // startup.
    public class MainHandler : MonoBehaviour
    {
        public Text TextCorreo;
        public Text TextContrasena;
        protected Firebase.Auth.FirebaseAuth auth;
        protected Firebase.Auth.FirebaseAuth otherAuth;
        protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
          new Dictionary<string, Firebase.Auth.FirebaseUser>();
        private string logText = "";
        protected string email = "";
        protected string password = "";
        protected string displayName = "";
        protected string phoneNumber = "";
        protected string receivedCode = "";
        protected bool signInAndFetchProfile = false;

        private bool fetchingToken = false;




        const int kMaxLogSize = 16382;
        Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

        // When the app starts, check to make sure that we have
        // the required dependencies to use Firebase, and if not,
        // add them if possible.
        public virtual void Start()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError(
                      "No esposible conectar con las dependencias de Firebase: " + dependencyStatus);
                }
            });
        }

        // Handle initialization of the necessary firebase modules:
        protected void InitializeFirebase()
        {
            DebugLog("Configurando Firebase Auth");
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.StateChanged += AuthStateChanged;
            auth.IdTokenChanged += IdTokenChanged;
            AuthStateChanged(this, null);
        }

        // Exit if escape (or back, on mobile) is pressed.
        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        void OnDestroy()
        {
            auth.StateChanged -= AuthStateChanged;
            auth.IdTokenChanged -= IdTokenChanged;
            auth = null;
        }

        void DisableUI()
        {
            
        }

        void EnableUI()
        {
            
        }

        // Output text to the debug log text field, as well as the console.
        public void DebugLog(string s)
        {
            Debug.Log(s);
            logText += s + "\n";

            while (logText.Length > kMaxLogSize)
            {
                int index = logText.IndexOf("\n");
                logText = logText.Substring(index + 1);
            }
        }

        // Display additional user profile information.
        protected void DisplayProfile<T>(IDictionary<T, object> profile, int indentLevel)
        {
            string indent = new String(' ', indentLevel * 2);
            foreach (var kv in profile)
            {
                var valueDictionary = kv.Value as IDictionary<object, object>;
                if (valueDictionary != null)
                {
                    DebugLog(String.Format("{0}{1}:", indent, kv.Key));
                    DisplayProfile<object>(valueDictionary, indentLevel + 1);
                }
                else
                {
                    DebugLog(String.Format("{0}{1}: {2}", indent, kv.Key, kv.Value));
                }
            }
        }

        // Display user information reported
        protected void DisplaySignInResult(Firebase.Auth.SignInResult result, int indentLevel)
        {
            string indent = new String(' ', indentLevel * 2);
            DisplayDetailedUserInfo(result.User, indentLevel);
            var metadata = result.Meta;
            if (metadata != null)
            {
                DebugLog(String.Format("{0}Created: {1}", indent, metadata.CreationTimestamp));
                DebugLog(String.Format("{0}Last Sign-in: {1}", indent, metadata.LastSignInTimestamp));
            }
            var info = result.Info;
            if (info != null)
            {
                DebugLog(String.Format("{0}Additional User Info:", indent));
                DebugLog(String.Format("{0}  User Name: {1}", indent, info.UserName));
                DebugLog(String.Format("{0}  Provider ID: {1}", indent, info.ProviderId));
                DisplayProfile<string>(info.Profile, indentLevel + 1);
            }
        }

        // Display user information.
        protected void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel)
        {
            string indent = new String(' ', indentLevel * 2);
            var userProperties = new Dictionary<string, string> {
        {"Display Name", userInfo.DisplayName},
        {"Email", userInfo.Email},
        {"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
        {"Provider ID", userInfo.ProviderId},
        {"User ID", userInfo.UserId}
      };
            foreach (var property in userProperties)
            {
                if (!String.IsNullOrEmpty(property.Value))
                {
                    DebugLog(String.Format("{0}{1}: {2}", indent, property.Key, property.Value));
                }
            }
        }

        // Display a more detailed view of a FirebaseUser.
        protected void DisplayDetailedUserInfo(Firebase.Auth.FirebaseUser user, int indentLevel)
        {
            string indent = new String(' ', indentLevel * 2);
            DisplayUserInfo(user, indentLevel);
            DebugLog(String.Format("{0}Anonymous: {1}", indent, user.IsAnonymous));
            DebugLog(String.Format("{0}Email Verified: {1}", indent, user.IsEmailVerified));
            DebugLog(String.Format("{0}Phone Number: {1}", indent, user.PhoneNumber));
            var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
            var numberOfProviders = providerDataList.Count;
            if (numberOfProviders > 0)
            {
                for (int i = 0; i < numberOfProviders; ++i)
                {
                    DebugLog(String.Format("{0}Provider Data: {1}", indent, i));
                    DisplayUserInfo(providerDataList[i], indentLevel + 2);
                }
            }
        }

        // Track state changes of the auth object.
        void AuthStateChanged(object sender, System.EventArgs eventArgs)
        {
            Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
            Firebase.Auth.FirebaseUser user = null;
            if (senderAuth != null) userByAuth.TryGetValue(senderAuth.App.Name, out user);
            if (senderAuth == auth && senderAuth.CurrentUser != user)
            {
                bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
                if (!signedIn && user != null)
                {
                    DebugLog("Signed out " + user.UserId);
                }
                user = senderAuth.CurrentUser;
                userByAuth[senderAuth.App.Name] = user;
                if (signedIn)
                {
                    DebugLog("Signed in " + user.UserId);
                    displayName = user.DisplayName ?? "";
                    DisplayDetailedUserInfo(user, 1);
                }
            }
        }

        // Track ID token changes.
        void IdTokenChanged(object sender, System.EventArgs eventArgs)
        {
            Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
            if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken)
            {
                senderAuth.CurrentUser.TokenAsync(false).ContinueWith(
                  task => DebugLog(String.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
            }
        }

        // Log the result of the specified task, returning true if the task
        // completed successfully, false otherwise.
        protected bool LogTaskCompletion(Task task, string operation)
        {
            bool complete = false;
            if (task.IsCanceled)
            {
                DebugLog(operation + " canceled.");
            }
            else if (task.IsFaulted)
            {
                DebugLog(operation + " encounted an error.");
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    string authErrorCode = "";
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        authErrorCode = String.Format("AuthError.{0}: ",
                          ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                    }
                    DebugLog(authErrorCode + exception.ToString());
                }
            }
            else if (task.IsCompleted)
            {
                DebugLog(operation + " completed");
                complete = true;
            }
            return complete;
        }

        // Create a user with the email and password.
        public void CreateUserWithEmailAsync()
        {
            email = TextCorreo.text;
            password = TextContrasena.text;
            DebugLog(String.Format("Attempting to create user {0}...", email));
            DisableUI();


            string newDisplayName = displayName;
            auth.CreateUserWithEmailAndPasswordAsync(email, password)
              .ContinueWith((task) =>
              {
                  EnableUI();
                  if (LogTaskCompletion(task, "Creación de usuario"))
                  {
                      var user = task.Result;
                      DisplayDetailedUserInfo(user, 1);
                      return UpdateUserProfileAsync(newDisplayName: newDisplayName);
                  }
                  return task;
              }).Unwrap();
        }


        public Task UpdateUserProfileAsync(string newDisplayName = null)
        {
            if (auth.CurrentUser == null)
            {
                DebugLog("Not signed in, unable to update user profile");
                return Task.FromResult(0);
            }
            displayName = newDisplayName ?? displayName;
            DebugLog("Updating user profile");
            DisableUI();
            return auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile
            {
                DisplayName = displayName,
                PhotoUrl = auth.CurrentUser.PhotoUrl,
            }).ContinueWith(task =>
            {
                EnableUI();
                if (LogTaskCompletion(task, "User profile"))
                {
                    DisplayDetailedUserInfo(auth.CurrentUser, 1);
                }
            });
        }


        public void SigninWithEmailAsync()
        {
            email = TextCorreo.text;
            password = TextContrasena.text;
            DebugLog(String.Format("Attempting to sign in as {0}...", email));
            DisableUI();
            if (signInAndFetchProfile)
            {
                auth.SignInAndRetrieveDataWithCredentialAsync(
                  Firebase.Auth.EmailAuthProvider.GetCredential(email, password)).ContinueWith(
                    HandleSignInWithSignInResult);
            }
            else
            {
                auth.SignInWithEmailAndPasswordAsync(email, password)
                 .ContinueWith(HandleSignInWithUser);
            }
        }


        public Task SigninWithEmailCredentialAsync()
        {
            DebugLog(String.Format("Attempting to sign in as {0}...", email));
            DisableUI();
            if (signInAndFetchProfile)
            {
                return auth.SignInAndRetrieveDataWithCredentialAsync(
                  Firebase.Auth.EmailAuthProvider.GetCredential(email, password)).ContinueWith(
                    HandleSignInWithSignInResult);
            }
            else
            {
                return auth.SignInWithCredentialAsync(
                  Firebase.Auth.EmailAuthProvider.GetCredential(email, password)).ContinueWith(
                    HandleSignInWithUser);
            }
        }


        // Called when a sign-in without fetching profile data completes.
        void HandleSignInWithUser(Task<Firebase.Auth.FirebaseUser> task)
        {
            EnableUI();
            if (LogTaskCompletion(task, "Sign-in"))
            {
                DebugLog(String.Format("{0} signed in", task.Result.DisplayName));
            }
        }

        // Called when a sign-in with profile data completes.
        void HandleSignInWithSignInResult(Task<Firebase.Auth.SignInResult> task)
        {
            EnableUI();
            if (LogTaskCompletion(task, "Sign-in"))
            {
                DisplaySignInResult(task.Result, 1);
            }
        }



        // Reauthenticate the user with the current email / password.
        protected Task ReauthenticateAsync()
        {
            var user = auth.CurrentUser;
            if (user == null)
            {
                DebugLog("Not signed in, unable to reauthenticate user.");
                var tcs = new TaskCompletionSource<bool>();
                tcs.SetException(new Exception("Not signed in"));
                return tcs.Task;
            }
            DebugLog("Reauthenticating...");
            DisableUI();
            Firebase.Auth.Credential cred = Firebase.Auth.EmailAuthProvider.GetCredential(email, password);
            if (signInAndFetchProfile)
            {
                return user.ReauthenticateAndRetrieveDataAsync(cred).ContinueWith(task =>
                {
                    EnableUI();
                    if (LogTaskCompletion(task, "Reauthentication"))
                    {
                        DisplaySignInResult(task.Result, 1);
                    }
                });
            }
            else
            {
                return user.ReauthenticateAsync(cred).ContinueWith(task =>
                {
                    EnableUI();
                    if (LogTaskCompletion(task, "Reauthentication"))
                    {
                        DisplayDetailedUserInfo(auth.CurrentUser, 1);
                    }
                });
            }
        }

        // Reload the currently logged in user.
        public void ReloadUser()
        {
            if (auth.CurrentUser == null)
            {
                DebugLog("Not signed in, unable to reload user.");
                return;
            }
            DebugLog("Reload User Data");
            auth.CurrentUser.ReloadAsync().ContinueWith(task =>
            {
                if (LogTaskCompletion(task, "Reload"))
                {
                    DisplayDetailedUserInfo(auth.CurrentUser, 1);
                }
            });
        }

        // Fetch and display current user's auth token.
        public void GetUserToken()
        {
            if (auth.CurrentUser == null)
            {
                DebugLog("Not signed in, unable to get token.");
                return;
            }
            DebugLog("Fetching user token");
            fetchingToken = true;
            auth.CurrentUser.TokenAsync(false).ContinueWith(task =>
            {
                fetchingToken = false;
                if (LogTaskCompletion(task, "User token fetch"))
                {
                    DebugLog("Token = " + task.Result);
                }
            });
        }

        // Display information about the currently logged in user.
        void GetUserInfo()
        {
            if (auth.CurrentUser == null)
            {
                DebugLog("Not signed in, unable to get info.");
            }
            else
            {
                DebugLog("Current user info:");
                DisplayDetailedUserInfo(auth.CurrentUser, 1);
            }
        }

        // Unlink the email credential from the currently logged in user.
        protected Task UnlinkEmailAsync()
        {
            if (auth.CurrentUser == null)
            {
                DebugLog("Not signed in, unable to unlink");
                var tcs = new TaskCompletionSource<bool>();
                tcs.SetException(new Exception("Not signed in"));
                return tcs.Task;
            }
            DebugLog("Unlinking email credential");
            DisableUI();
            return auth.CurrentUser.UnlinkAsync(
              Firebase.Auth.EmailAuthProvider.GetCredential(email, password).Provider)
                .ContinueWith(task =>
                {
                    EnableUI();
                    LogTaskCompletion(task, "Unlinking");
                });
        }

        // Sign out the current user.
        protected void SignOut()
        {
            DebugLog("Signing out.");
            auth.SignOut();
        }


        // Show the providers for the current email address.
        protected void DisplayProvidersForEmail()
        {
            auth.FetchProvidersForEmailAsync(email).ContinueWith((authTask) =>
            {
                if (LogTaskCompletion(authTask, "Fetch Providers"))
                {
                    DebugLog(String.Format("Email Providers for '{0}':", email));
                    foreach (string provider in authTask.Result)
                    {
                        DebugLog(provider);
                    }
                }
            });
        }

        // Send a password reset email to the current email address.
        protected void SendPasswordResetEmail()
        {
            auth.SendPasswordResetEmailAsync(email).ContinueWith((authTask) =>
            {
                if (LogTaskCompletion(authTask, "Send Password Reset Email"))
                {
                    DebugLog("Password reset email sent to " + email);
                }
            });
        }









    }
