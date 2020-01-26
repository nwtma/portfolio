
using System;
using System.Linq;

namespace BattleInArea.Core
{
    public partial class Event
    {
        protected class Pool
        {
            public static T Pop<T>() where T : IData
            {
                return (T)Activator.CreateInstance(typeof(T));
            }

            public static void Push(IData evt)
            {

            }
        }
    }
}

