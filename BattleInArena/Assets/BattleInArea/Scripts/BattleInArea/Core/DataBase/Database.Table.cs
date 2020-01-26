

using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Core
{
    public partial class DataBase
    {
        public abstract partial class Table : ITable
        {

            /// <summary>
            /// RowData Collection
            /// </summary>
            protected class RowDataCollection : Dictionary<string, IRowData>
            {
            }

            /// <summary>
            /// 
            /// </summary>
            protected Table(string Key)
            {
                mKey = Key;
            }

            private string mKey;
            public string Key
            {
                get { return mKey; }
            }

            private bool mIsLoadComplete;
            public bool isLoadComplete
            {
                get { return mIsLoadComplete; }
            }

            protected RowDataCollection collection = new RowDataCollection();

            public IRowData GetRowData(string Key)
            {
                if (collection.ContainsKey(Key))
                {
                    return collection[Key];
                }
                return null;
            }

            public abstract IEnumerator Load();

            protected void LoadComplete()
            {
                mIsLoadComplete = true;
            }
        }
    }
}