using UnityEngine;
using UnityEditor;


namespace BattleInArea.Core
{

    public partial class Event
    {
        public interface IReceiveable
        {
            void Receive(IData evt);
        }
    }
}
