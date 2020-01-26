using UnityEngine;
using UnityEditor;


namespace BattleInArea.Core
{
    public partial class Event
    {
        protected class Setter
        {
            public static void Set(IData evt)
            {
                Pool.Push(evt);
            }
        }
    }
}
