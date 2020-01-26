

using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{
    public partial class DataBase
    {
        public class CharacterTable : Core.DataBase.Table
        {
            /// <summary>
            /// 
            /// </summary>
            public CharacterTable() : base(DataBase.CharacterTableKey)
            {
            }

            public override IEnumerator Load()
            {
                List<Dictionary<string, object>> temp = new List<Dictionary<string, object>>();

                Core.Reader.CSV.Read("Tables/Character", temp);

                for (int i = 0, ii = temp.Count; ii > i; ++i)
                {
                    yield return null;
                    RowData.ChrarcterRowData row = new RowData.ChrarcterRowData();
                    row.Key = temp[i]["Key"].ToString();
                    row.type = temp[i]["type"].ToString();
                    row.hp = (int)temp[i]["hp"];
                    row.ap = (int)temp[i]["ap"];
                    row.maxap = (int)temp[i]["maxap"];
                    row.turnap = (int)temp[i]["turnap"];
                    row.skill01ap = (int)temp[i]["skill01ap"];
                    row.skill02ap = (int)temp[i]["skill02ap"];
                    row.skill03ap = (int)temp[i]["skill03ap"];
                    row.skill01dmg = (int)temp[i]["skill01dmg"];
                    row.skill02dmg = (int)temp[i]["skill02dmg"];
                    row.skill03dmg = (int)temp[i]["skill03dmg"];

                    collection.Add(row.Key, row);

#if UNITY_EDITOR
                    
                    UnityEngine.Debug.LogFormat("log ! : add character row data {0}", row.type);
#endif
                }

                yield return null;

                temp.Clear();
                temp = null;

                base.LoadComplete();
            }
        }
    }
}