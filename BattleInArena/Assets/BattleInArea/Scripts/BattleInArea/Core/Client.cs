

using UnityEngine;
using UnityEngine.Networking;


namespace BattleInArea.Core
{
    [RequireComponent(typeof(Coroutine))]
    public abstract class Client : MonoBehaviour, Event.IReceiveable
    {

        /// <summary>
        /// 
        /// </summary>
        protected abstract void Awake();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evt"></param>
        public abstract void Receive(Event.IData evt);
    }
}