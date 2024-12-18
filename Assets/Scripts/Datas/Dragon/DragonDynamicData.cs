namespace Datas.Dragon {
    /// <summary>
    /// All dynamic data of dragon.
    /// </summary>
    public class DragonDynamicData {
        /// <summary>
        /// Dragon's dynamic data.
        /// Rank of qualities of dragon. 
        /// Dragon that have all qualities are maximum (5) will become golden dragon.
        /// Golden dragon will get more hp
        /// </summary>
        public class DragonQualitiesRank {
            /// <summary>
            /// increase atk as a percentage (const)
            /// </summary>
            public byte Atk { get; set; }

            /// <summary>
            /// increase the chance of getting the item as a percentage
            /// </summary>
            public byte Luck { get; set; }

            /// <summary>
            /// decrease digestion time (second)
            /// </summary>
            public byte Digestion { get; set; }

            /// <summary>
            /// increase the amount of experience that player gained when feeding the dragons
            /// </summary>
            public byte Exp { get; set; }

            /// <summary>
            /// increase the amount of total Gold that player gained when feeding the dragons
            /// </summary>
            public byte Gold { get; set; }
        }

        /// <summary>
        /// Dragon's Id is unique to each dragon. It's used to get raw data of dragon from firestore
        /// </summary>
        public ushort Id { get; set; }

        /// <summary>
        /// the level of the dragon
        /// </summary>
        public byte Level { get; set; }

        /// <summary>
        /// Number of experience that the dragon have, dragons will level up if their experience reaches a certain milestone
        /// </summary>
        public ushort LevelProgress { get; set; }

        /// <summary>
        /// The larger the rank, the larger the level limit
        /// </summary>
        public byte Rank { get; set; }

        /// <summary>
        /// The rank of qualidies of dragon
        /// </summary>
        public DragonQualitiesRank QualidiesRank { get; set; }

        /// <summary>
        /// information about 3 rune that dragon was equiped
        /// </summary>
        public Rune.RuneDynamicData[] Runes { get; set; }

        /// <summary>
        /// Initial dynamic data of dragon at id
        /// </summary>
        public static DragonDynamicData New(ushort id) {
            DragonDynamicData tmp = new() {
                Id = id,
                QualidiesRank = new() {
                    Atk = 1,
                    Luck = 1,
                    Digestion = 1,
                    Exp = 1,
                    Gold = 1
                },
            };

            return tmp;
        }
    }
}