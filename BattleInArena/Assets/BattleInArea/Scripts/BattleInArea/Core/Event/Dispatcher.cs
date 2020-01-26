using UnityEngine;
using UnityEditor;


namespace BattleInArea.Core
{
    public partial class Event
    {
        public class Dispatcher
        {
            private static Dispatcher dispatcher;
            
            public static void Dispatch(IData evt)
            {
                dispatcher.receiver.Receive(evt);

                Setter.Set(evt);
            }

            public Dispatcher(IReceiveable receiver)
            {
                dispatcher = this;

                this.receiver = receiver;
            }

            private IReceiveable receiver;
        }
    }
}