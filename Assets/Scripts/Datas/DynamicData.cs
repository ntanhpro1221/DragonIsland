using System.Collections.Generic;

namespace Datas {
    public class DynamicData {
        #region CORE
        /// <summary>
        /// Player's rank in thunder arena
        /// </summary>
        public short Rank { get; set; }

        /// <summary>
        /// Player's gold amount
        /// </summary>
        public ulong Gold { get; set; }

        /// <summary>
        /// Player's diamond amount
        /// </summary>
        public ulong Diamond { get; set; }

        /// <summary>
        /// Player's level
        /// </summary>
        public byte Level { get; set; }

        /// <summary>
        /// Number of experience that player have, player will level up if their experience reaches a certain milestone
        /// </summary>
        public uint LevelProgress { get; set; }

        /// <summary>
        /// Player's master's Id
        /// </summary>
        public string IdOfMaster { get; set; }
        #endregion

        #region ITEM
        /// <summary>
        /// Worst food for dragon
        /// </summary>
        public ulong Fruit { get; set; }

        /// <summary>
        /// Normal food for dragon
        /// </summary>
        public ulong Meat { get; set; }

        /// <summary>
        /// Best food for dragon
        /// </summary>
        public ulong Beef { get; set; }

        /// <summary>
        /// Coming soon
        /// </summary>
        public ulong MysticalCrystal { get; set; }

        /// <summary>
        /// Be used to join the expedition
        /// </summary>
        public ulong DimentionalFragments { get; set; }

        /// <summary>
        /// Be used to make love ( 1 -> 15 stars)
        /// </summary>
        public ulong Ambe { get; set; }

        /// <summary>
        /// Be used to make love ( 16 - 24 stars)
        /// </summary>
        public ulong Quart { get; set; }

        /// <summary>
        /// Make gold by the time
        /// </summary>
        public ulong GoldMine { get; set; }

        /// <summary>
        /// Make experience by the time
        /// </summary>
        public ulong ExpMine { get; set; }

        /// <summary>
        /// Make amber by the time
        /// </summary>
        public ulong AmberMine { get; set; }

        /// <summary>
        /// Make quartz by the time
        /// </summary>
        public ulong QuartzMine { get; set; }
        #endregion

        public List<Rune.RuneDynamicData> Runes { get; set; }

        public List<Dragon.DragonDynamicData> Dragons { get; set; }

        /// <summary>
        /// Default dynamic data for new player
        /// </summary>
        public static DynamicData New() {
            DynamicData tmp = new() {
                Rank = 1000,
                Gold = 10000,
                Diamond = 10,
                Level = 1,
                Fruit = 5,
                Runes = new(),
                Dragons = new() {
                    Dragon.DragonDynamicData.New(1),
                    Dragon.DragonDynamicData.New(2),
                }
            };

            return tmp;
        }
    }
}