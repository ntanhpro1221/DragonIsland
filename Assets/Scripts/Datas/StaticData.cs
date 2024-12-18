using Firebase.Firestore;
using Datas.Dragon;
using Datas.Rune;
using System.Collections.Generic;

namespace Datas {
    [FirestoreData]
    public class StaticData {
        [FirestoreProperty]
        public Dictionary<RuneType, RuneStaticData> RuneData { get; set; }

        [FirestoreProperty]
        public DragonStaticData[] DragonData { get; set; }
    }
}
