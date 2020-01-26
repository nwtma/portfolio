


namespace BattleInArea.Game
{
    public partial class DataBase
    {
        public class RowData
        {
            public class ChrarcterRowData : Core.DataBase.IRowData
            {
                public string Key
                {
                    get;
                    set;
                }
                public string type;
                public int hp;
                public int ap;
                public int maxap;
                public int turnap;
                public int skill01ap;
                public int skill02ap;
                public int skill03ap;
                public int skill01dmg;
                public int skill02dmg;
                public int skill03dmg;
            }
        }
    }
}