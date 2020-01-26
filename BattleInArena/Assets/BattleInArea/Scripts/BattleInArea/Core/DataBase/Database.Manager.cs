

using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Core
{
    public partial class DataBase
    {
        public abstract class Manager : Interfaces.IManager
        {
            public static Manager Single { private set; get; }

            public Manager()
            {
                Single = this;
            }

            protected Dictionary<string, ITable> tables = new Dictionary<string, ITable>();
            public bool IsLoaded { private set; get; }

            public T GetTable<T>(string key) where T : ITable
            {
                if (tables.ContainsKey(key))
                {
                    return (T)tables[key];
                }

                return default(T);
            }

            public ITable GetTable(string key) 
            {
                if (tables.ContainsKey(key))
                {
                    return tables[key];
                }

                return null;
            }
            public abstract void Prepare();

            protected void Add<T>(ITable table)
            {
                tables.Add(table.Key, table);
            }

            protected void Add(ITable table)
            {
                tables.Add(table.Key, table);
            }

            public IEnumerator LoadTables()
            {
                //Add<Table.Person>(new Table.Person());
                //Add<Table.Building>(new Table.Building());
                //Add<Table.Troop>(new Table.Troop());
                //Add<Table.WayPoint>(new Table.WayPoint());

                IsLoaded = false;

                using (Dictionary<string, ITable>.Enumerator e = tables.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        Coroutine.instance.StartCoroutine(e.Current.Value.Load());
                    }
                }

                while (true)
                {
                    yield return null;

                    bool complete = true;

                    using (Dictionary<string, ITable>.Enumerator e = tables.GetEnumerator())
                    {
                        while (e.MoveNext())
                        {
                            if (!e.Current.Value.isLoadComplete)
                            {
                                complete = false;
                                break;
                            }
                        }
                    }

                    /*
                    for (int i = 0, ii = tables.Count; ii > i; ++i)
                    {
                        if (!tables[i].isLoadComplete)
                        {
                            complete = false;
                            break;
                        }
                    }
                    */

                    if (complete)
                        break;
                }

                IsLoaded = true;
            }
        }
    }
}