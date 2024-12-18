using Firebase.Firestore;

namespace Datas {
    [FirestoreData]
    public class Potential<T> {
        [FirestoreProperty]
        public T Def { get; set; }
        [FirestoreProperty]
        public T Bonus { get; set; }
    }
}