
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BattleInArea.Game
{
    public partial class Page
    {
       

        /// <summary>
        /// Battle In Area UI Manager
        /// </summary>
        public class Lobby : Core.Page.IPage
        {

            public int ID
            {
                get { return (int)Page.ID.Lobby; }
            }


            public IEnumerator OnPreEnter()
            {
                while(true)
                {
                    yield return null;

                    if (Helper.Sever.IsConnected)
                        break;
                }
                
            }

            public void OnEnter()
            {
                Event.PageModify evt = Core.Event.Getter.Get<Event.PageModify>();
                evt.hashtable["page"] = Page.ID.Lobby;
                evt.hashtable["state"] = Event.PageState.OnEnter;
                Core.Event.Dispatcher.Dispatch(evt);
                
            }

            public void OnExecute()
            {
                Event.PageModify evt = Core.Event.Getter.Get<Event.PageModify>();
                evt.hashtable["page"] = Page.ID.Lobby;
                evt.hashtable["state"] = Event.PageState.OnExecute;
                Core.Event.Dispatcher.Dispatch(evt);

            }

            public void OnExit()
            {
                Event.PageModify evt = Core.Event.Getter.Get<Event.PageModify>();
                evt.hashtable["page"] = Page.ID.Lobby;
                evt.hashtable["state"] = Event.PageState.OnExit;
                Core.Event.Dispatcher.Dispatch(evt);
            }
        }
    }
}