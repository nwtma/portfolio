


using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleInArea.Core
{
    public partial class DataBase
    {
        public interface IRowData
        {
            string Key
            {
                get;
                set;
            }
        }
    }
}