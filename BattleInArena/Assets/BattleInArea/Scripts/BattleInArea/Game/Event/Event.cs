


using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{
    public partial class Event
    {
        /// <summary>
        /// 선택 이벤트
        /// </summary>
        public class Selected : Core.Event.IData
        {
            public int ID
            {
                get { return (int)Event.ID.Selected; }
            }

            public Hashtable hashTable = new Hashtable();
        }

        public class PageModify : Core.Event.IData
        {
            public int ID
            {
                get { return (int)Event.ID.PageModify; }
            }

            public Hashtable hashtable = new Hashtable();
        }

        public class TurnModify : Core.Event.IData
        {
            public int ID
            {
                get { return (int)Event.ID.TurnModify; }
            }

            public Hashtable hashtable = new Hashtable();
        }
        
        public class Item : Core.Event.IData
        {
            public int ID
            {
                get { return (int)Event.ID.Item; }
            }

            public Hashtable hashTable = new Hashtable();
        }

        public class GameResult : Core.Event.IData
        {
            public int ID
            {
                get { return (int)Event.ID.GameResult; }
            }

            public Hashtable hashTable = new Hashtable();
        }

        public class OnEvent : Core.Event.IData
        {
            public int ID
            {
                get { return (int)Event.ID.Event; }
            }

            public Hashtable hashTable = new Hashtable();
        }

        public class EffectModify : Core.Event.IData
        {
            public int ID
            {
                get { return (int)Event.ID.EffectModigy; }
            }

            public Hashtable hashTable = new Hashtable();
        }
        public class Room : Core.Event.IData
        {
            public int ID
            {
                get { return (int)Event.ID.Room; }
            }
            public Hashtable hashTable = new Hashtable();
        }
    }
}