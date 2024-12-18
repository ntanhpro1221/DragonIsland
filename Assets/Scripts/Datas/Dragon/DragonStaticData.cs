using Firebase.Firestore;

namespace Datas.Dragon {
    /// <summary>
    /// Include important default information about dragon.
    /// That is why it is hosted on the sever instead of on the client.
    /// </summary>
    [FirestoreData]
    public class DragonStaticData {
        /// <summary>
        /// Potential of Health point.
        /// Total = (Def.Def + Def.Bonus * Rank of this dragon) + (Bonus.Def + Bonus.Bonus * Rank of this dragon) * Level of this dragon
        /// </summary>
        [FirestoreProperty]
        public Potential<Potential<byte>> Hp { get; set; }

        /// <summary>
        /// Potential of Attack point.
        /// Total = (Def.Def + Def.Bonus * Rank of this dragon) + (Bonus.Def + Bonus.Bonus * Rank of this dragon) * Level of this dragon
        /// </summary>
        [FirestoreProperty]
        public Potential<Potential<byte>> Atk { get; set; }

        /// <summary>
        /// Potential of Attack speed.
        /// Total = (Def.Def + Def.Bonus * Rank of this dragon) + (Bonus.Def + Bonus.Bonus * Rank of this dragon) * Level of this dragon
        /// </summary>
        [FirestoreProperty]
        public Potential<Potential<float>> Speed { get; set; }

        /// <summary>
        /// Potential of Increasing the chance of getting more items when go exploring.
        /// Total = Def + Bonus * (Rank of this quality - 1)
        /// </summary>
        [FirestoreProperty]
        public Potential<byte> Luck { get; set; }

        /// <summary>
        /// Potential of reducing digestion time.
        /// Total = Def + Bonus * (Rank of this quality - 1)
        /// </summary>
        [FirestoreProperty]
        public Potential<byte> Digestion { get; set; }

        /// <summary>
        /// Potential of increasing the experience that player gained when feeding the dragons.
        /// Total = Def + Bonus * (Rank of this quality - 1)
        /// </summary>
        [FirestoreProperty]
        public Potential<byte> Exp { get; set; }

        /// <summary>
        /// Potential of increasing the total gold that player gained when feeding the dragons.
        /// Total = Def + Bonus * (Rank of this quality - 1)
        /// </summary>
        [FirestoreProperty]
        public Potential<byte> Gold { get; set; }
    }
}