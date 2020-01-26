using UnityEngine;
using UnityEditor;

namespace BattleInArea.Core
{
    public partial class Event
    {
        public interface ISendable
        {
            void Send(IData evt);
        }
    }
}
