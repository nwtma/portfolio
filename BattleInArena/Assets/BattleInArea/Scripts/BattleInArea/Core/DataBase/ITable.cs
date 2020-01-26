


using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleInArea.Core
{
    public partial class DataBase
    {
        public interface ITable : Interfaces.IHasKey
        {
            bool isLoadComplete
            {
                get;
            }

            IRowData GetRowData(string Key);

            IEnumerator Load();
        }
    }
}