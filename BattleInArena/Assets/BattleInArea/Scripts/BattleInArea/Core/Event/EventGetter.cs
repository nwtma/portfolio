using UnityEngine;
using UnityEditor;


namespace BattleInArea.Core
{
    public partial class Event
    {
        public class Getter
        {
            public static T Get<T>() where T : IData
            {
                return Pool.Pop<T>();
            }
        }
    }
}
