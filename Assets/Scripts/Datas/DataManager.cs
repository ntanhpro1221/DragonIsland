using System.Threading.Tasks;
using UnityEngine;
using Firebase.Database;
using Firebase.Firestore;
using Firebase.Auth;
using QFSW.QC;
using Newtonsoft.Json;
using System.Linq;
using Datas.Dragon;
using Datas.Rune;
using System.Collections;
using Extensions.StringExten;

namespace Datas {
    //#nullable enable
    public class DataManager : MonoBehaviour {
        #region Initialize
        public static DataManager Instance { get; private set; }

        //Reference to json tree
        private DatabaseReference rootRef;
        private DatabaseReference userRef;

        //Data
        private DynamicData dynamicData;
        private StaticData staticData;

        #endregion

        #region Event
#if UNITY_EDITOR
        private void OnEnable() {
#else
        private void Awake() {
#endif
            DontDestroyOnLoad(gameObject);
            if (Instance != null) Destroy(Instance);//I dont know why so dont ask me about his existence :v
            Instance = this;
            rootRef = FirebaseDatabase.DefaultInstance.RootReference;

        }

        /// <summary>
        /// Initialize somthing after user signed in
        /// </summary>
        public void OnSignedIn() {
            userRef = rootRef.Child("users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        }
        #endregion

        #region Method
        /// <summary>
        /// Load base important data of game that is hosted on the server.
        /// Called it immediately when game start
        /// </summary>
        public async Task LoadStaticDataAsync() {
            staticData = new();
            await Task.WhenAll(
                //Load Rune Static Data
                Task.Run(async () => staticData.RuneData =
                    (await FirebaseFirestore.DefaultInstance.Collection("Runes").Document("Nameless").GetSnapshotAsync()).ToDictionary()
                    .ToDictionary(pair => pair.Key.Enum<RuneType>(), pair => JsonConvert.DeserializeObject<RuneStaticData>(JsonConvert.SerializeObject(pair.Value)))),

                //Load Dragon Static Data
                Task.Run(async () => staticData.DragonData =
                    (await FirebaseFirestore.DefaultInstance.Collection("Dragons").GetSnapshotAsync())
                    .Documents.Select(doc => doc.ConvertTo<DragonStaticData>()).ToArray())
            );
        }

        /// <summary>
        /// Load user data.
        /// Called it after user signed in
        /// </summary>
        public async Task LoadDynamicDataAsync() {
            string json = (await userRef.GetValueAsync()).GetRawJsonValue();

            //Create new database for users if they are newbie
            if (json == null) {
                dynamicData = DynamicData.New();
                await userRef.SetRawJsonValueAsync(JsonConvert.SerializeObject(dynamicData));
            } else dynamicData = JsonConvert.DeserializeObject<DynamicData>(json);
        }
        
        /// <summary>
        /// Write or override data to key (tip: use nameof(key_variable) for key param)
        /// </summary>
        public async Task SetData(string key, object val) {
            //Server-Side
            Task task = Task.Run(() => userRef.Child(key).SetValueAsync(val));

            //Client-Side
            typeof(DynamicData).GetProperty(key).SetValue(dynamicData, val);

            await task;
        }
        
        /// <summary>
        /// push data to a list type of key (tip: use nameof(key_variable) for key param)
        /// </summary>
        public async Task PushData(string key, object val) {
            IList list = typeof(DynamicData).GetProperty(key).GetValue(dynamicData) as IList;

            //Server-Side
            Task task = Task.Run(() => userRef.Child(key + "/" + list.Count).SetRawJsonValueAsync(JsonConvert.SerializeObject(val)));

            //Client-Side
            list.Add(val);

            await task;
        }
        
        [Command]
        public void add(DatabaseReference hihi) {
            print(nameof(hihi));
        }
        [Command]
        public void show() {
            add(userRef);
        }

        #endregion
    }
}