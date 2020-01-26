

using System.Collections;
using System.Collections.Generic;
namespace BattleInArea.Game
{
    public partial class Helper
    {
        public class DataBase
        {
            /// <summary>
            /// 모든 데이터 테이블을 로드합니다.
            /// </summary>
            public static IEnumerator LoadTables()
            {
                yield return Core.Coroutine.instance.StartCoroutine(Game.DataBase.Manager.Single.LoadTables());
            }

            /// <summary>
            /// 테이터 테이블 로드가 완료 되었습니까?
            /// </summary>
            public static bool IsLoaded
            {
                get { return Core.DataBase.Manager.Single.IsLoaded; }
            }

            public static Core.DataBase.ITable GetTable(string key) 
            {
                return Game.DataBase.Manager.Single.GetTable(key);
            }
        }
    }
}